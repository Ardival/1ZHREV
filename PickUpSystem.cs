using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUpItem item = collision.GetComponent<PickUpItem>();
        if (item != null)
        {
            Debug.Log("Item detected: " + item.InventoryItem);
            inventoryData.AddItem(item.InventoryItem);
            // if (reminder == 0)
            item.DestroyItem();
            // else
            //     item.Quantity = reminder;
        }
    }
}