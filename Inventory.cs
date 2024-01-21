using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
 [SerializeField] private InventoryItem itemPrefab;
 [SerializeField] private RectTransform contentPanel;
 [SerializeField] private InventoryDescription itemDescription;
[SerializeField] private MouseFollower mouseFollower;
 List<InventoryItem> listOfItems = new List<InventoryItem>();
 private int currentlyDraggedItemIndex = -1;

 public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
 public event Action<int, int> OnSwapItems;

 private void Awake()
 {
    Hide();
    mouseFollower.Toogle(false);
    itemDescription.ResetDescription();
 }

    public void InitializeInventory(int inventorysize)
    {
        for (int i = 0; i < inventorysize; i++)
        {
            InventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listOfItems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    
    }

    internal void ResetAllItems()
    {
        foreach (var item in listOfItems)
        {
            item.ResetData();
            item.Deselect();
        }
    }

    internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
    {
        itemDescription.SetDescription(itemImage, name, description);
        DeselectAllItems();
        listOfItems[itemIndex].Select();
    }

    public void UpdateData(int itemIndex, Sprite itemImage)
    {
        if(listOfItems.Count > itemIndex)
        {
            listOfItems[itemIndex].SetData(itemImage);
        }
    }

    private void HandleShowItemActions(InventoryItem obj)
    {
        int index = listOfItems.IndexOf(obj);
            if (index == -1)
            {
                return;
            }
            OnItemActionRequested?.Invoke(index);
    }
    private void HandleEndDrag(InventoryItem obj)
    {
        mouseFollower.Toogle(false);
        currentlyDraggedItemIndex = -1;
    }
    private void HandleSwap(InventoryItem obj)
    {
        int index = listOfItems.IndexOf(obj);
        if(index == -1)
        {
            return;
        }
        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        HandleItemSelection(obj);  
    }
    private void HandleBeginDrag(InventoryItem obj)
    {
        int index = listOfItems.IndexOf(obj);
        if(index == -1)
            return;
        currentlyDraggedItemIndex = index;
        HandleItemSelection(obj);
        OnStartDragging?.Invoke(index);
        
    }

    public void CreateDraggedItem(Sprite sprite)
    {
        mouseFollower.Toogle(true);
        mouseFollower.SetData(sprite);
    }
    private void HandleItemSelection(InventoryItem obj)
    {
        int index = listOfItems.IndexOf(obj);
        if(index == -1){
            return;
        }
        OnDescriptionRequested?.Invoke(index);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        ResetSelection();
    }
    public void ResetSelection()
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    

    private void DeselectAllItems()
    {
       foreach (InventoryItem item in listOfItems)
       {
            item.Deselect();
       }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        mouseFollower.Toogle(false);
        currentlyDraggedItemIndex = -1;
    }
}