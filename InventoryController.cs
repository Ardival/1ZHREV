using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventorySO inventoryData;

    public List<InventoryItemSO> initialItems = new List<InventoryItemSO>();



    private void Start()
    {
        inventory.InitializeInventory(inventoryData.Size);
        this.inventory.OnDescriptionRequested += HandleDescriptionRequest;
        this.inventory.OnSwapItems += HandleSwapItems;
        this.inventory.OnStartDragging += HandleDragging;
        this.inventory.OnItemActionRequested += HandleItemActionRequest;
        PrepareInventoryData();
    }

    private void PrepareInventoryData()
    {
        inventoryData.Initialize();
        inventoryData.OnInventoryUpdated += UpdateInventoryUI;
        foreach (InventoryItemSO item in initialItems)
        {
            if (item.IsEmpty){
                continue;
            }
            inventoryData.AddItem(item.item);
        }
    }

    private void UpdateInventoryUI(Dictionary<int, InventoryItemSO> inventoryState)
    {
        inventory.ResetAllItems();
        foreach (var item in inventoryState)
        {
            inventory.UpdateData(item.Key, item.Value.item.ItemImage);
        }
    }

    private void HandleItemActionRequest(int itemIndex)
    {
        InventoryItemSO inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
            return;

        IItemAction itemAction = inventoryItem.item as IItemAction;
        if(itemAction != null)
        {
            itemAction.PerformAction(gameObject);
        }
        IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex);
            }
    }
    private void HandleDragging(int itemIndex)
    {
        InventoryItemSO inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty){
            return;
        }
        inventory.CreateDraggedItem(inventoryItem.item.ItemImage);
    }
    private void HandleSwapItems(int itemIndex1, int itemIndex2)
    {
        inventoryData.SwapItems(itemIndex1, itemIndex2);
    }
    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItemSO inventoryItem = inventoryData.GetItemAt(itemIndex);
        if(inventoryItem.IsEmpty)
        {
            inventory.ResetSelection();
            return;
        }
        Item item = inventoryItem.item;
        inventory.UpdateDescription(itemIndex, item.ItemImage, item.name, item.description);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)){
            if (inventory.isActiveAndEnabled == false)
            {
                inventory.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    inventory.UpdateData(item.Key, item.Value.item.ItemImage);
                }
            } else {
                inventory.Hide();
            }
        }
    }
}
