using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
  [SerializeField] private List<InventoryItemSO> inventoryItems;

  [field: SerializeField] public int Size {get; private set; } = 12;

  public event Action<Dictionary<int, InventoryItemSO>> OnInventoryUpdated;

  public void Initialize()
  {
    inventoryItems = new List<InventoryItemSO>();
    for(int i = 0; i < Size; i++)
    {
        inventoryItems.Add(InventoryItemSO.GetEmptyItem());
    }
  }
  public void RemoveItem(int itemIndex)
  {
      if (inventoryItems.Count > itemIndex)
      {
          if (inventoryItems[itemIndex].IsEmpty)
              return;
          inventoryItems[itemIndex] = InventoryItemSO.GetEmptyItem();

          InformAbooutChange();
      }
  }

  public void AddItem(Item item)
  {
    for(int i = 0; i < inventoryItems.Count; i++)
    {
        if(inventoryItems[i].IsEmpty)
        {
            inventoryItems[i] = new InventoryItemSO
            {
                item = item,
            };
            return;
        }
    }
  }

  public Dictionary<int, InventoryItemSO> GetCurrentInventoryState()
  {
    Dictionary<int, InventoryItemSO> returnValue = new Dictionary<int, InventoryItemSO>();
    for (int i = 0; i < inventoryItems.Count; i++)
    {
        if (inventoryItems[i].IsEmpty)
        {
            continue;
        }
        returnValue[i] = inventoryItems[i];
    }
    return returnValue;
  }

  public InventoryItemSO GetItemAt(int itemIndex)
  {
    return inventoryItems[itemIndex];
  }

  public void SwapItems(int itemIndex1, int itemIndex2)
  {
    InventoryItemSO item1 = inventoryItems[itemIndex1];
    inventoryItems[itemIndex1] = inventoryItems[itemIndex2];
    inventoryItems[itemIndex2] = item1;
    InformAbooutChange();
  }

  private void InformAbooutChange()
  {
    OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
  }
}

[Serializable]
public struct InventoryItemSO
{
    public Item item;
    public bool IsEmpty => item == null;

    public static InventoryItemSO GetEmptyItem()
    => new InventoryItemSO
    {
        item = null,
    };
}
