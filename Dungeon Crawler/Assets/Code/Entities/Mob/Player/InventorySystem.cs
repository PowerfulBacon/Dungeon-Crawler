using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class InventorySystem
{

    //Inventory sizes. Do not change without UI changes.
    public const int INVENTORY_HEIGHT = 3;
    public const int INVENTORY_WIDTH = 10;
    public const int INVENTORY_SIZE = INVENTORY_HEIGHT * INVENTORY_WIDTH;

    public const int HOTBAR_SIZE = 10;

    public Item[] inventory = new Item[INVENTORY_SIZE];
    public int[] hotbarIndexes = new int[HOTBAR_SIZE];

    public int hotbarNum = 0;

    private Player parent;

    public InventorySystem(Player parent)
    {
        this.parent = parent;
        for(int i = 0; i < HOTBAR_SIZE; i ++)
        {
            hotbarIndexes[i] = -1;
        }
    }

    /// <summary>
    /// Adds an item to the players inventory.
    /// </summary>
    /// <param name="item">An item. We should have control of it at this point.</param>
    public bool AddItemToInventory(Item item)
    {
        Log.PrintDebug("Added item to inventory.");
        //Go and check free inventory slot
        for(int i = 0; i < INVENTORY_SIZE; i++)
        {
            if(inventory[i] == null)
            {
                inventory[i] = item;
                item.inventorySlot = i;
                //Horray, add the icon
                UpdateInventoryIcon(i);
                return true;
            }
        }
        //Error, drop the item.
        return false;
    }

    /// <summary>
    /// Removes an item from the players inventory
    /// </summary>
    /// <param name="item">An item. We should have control of it at this point.</param>
    public void RemoveItemFromInventory(Item item)
    {
        Log.PrintDebug("Removed item from inventory.");
        //Go and check free inventory slot
        for(int i = 0; i < INVENTORY_SIZE; i++)
        {
            if(inventory[i] == item)
            {
                inventory[i] = null;
                item.inventorySlot = -1;
                //Remove item from hotbar
                RemoveIndexFromHotbar(i, true);
                //Horray, add the icon
                UpdateInventoryIcon(i);
                return;
            }
        }
        //Error, drop the item.
        Log.PrintError("Failed to locate item in inventory.");
    }

    public void UpdateInventoryIcon(int index)
    {
        InventoryUi inventoryUi = Object.FindObjectOfType<InventoryUi>();
        //TODO: Caching
        if(inventory[index] != null)
            inventoryUi.GetComponentsInChildren<Image>()[index * 2 + 1].sprite = Resources.Load<Sprite>($"Icons/{inventory[index].iconName}");
        else
            inventoryUi.GetComponentsInChildren<Image>()[index * 2 + 1].sprite = Resources.Load<Sprite>($"Icons/icon_blank");
    }

    /// <summary>
    /// Swaps 2 indexes around.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public void SwapInventoryIndexes(int a, int b)
    {
        var temp = inventory[a];
        inventory[a] = inventory[b];
        InventoryItemMoved(b, a);
        inventory[b] = temp;
        InventoryItemMoved(a, b);
        //Update indexes
        if(inventory[b])
            inventory[b].inventorySlot = b;
        if(inventory[a])
            inventory[a].inventorySlot = a;
        //Update icons
        UpdateInventoryIcon(a);
        UpdateInventoryIcon(b);
    }

    public void InventoryItemMoved(int oldIndex, int newIndex)
    {
        HotbarItemMoved(oldIndex, newIndex);
    }

    //==============//
    // Hotbar Stuff //
    //==============//

    private int previousIndex = 0;

    private static Color32 COLOUR_HOTBAR_DEFAULT = new Color32(102, 102, 102, 255);
    private static Color32 COLOUR_HOTBAR_SELECTED = new Color32(255, 255, 255, 255);

    public void SetHotbarIndex(Hotbar inventory, int newValue)
    {
        hotbarNum = newValue % HOTBAR_SIZE;
        UpdateHotbarSelected(inventory);
    }
    
    public void IncreaseHotbarIndex(Hotbar inventory)
    {
        hotbarNum = (hotbarNum + 1) % HOTBAR_SIZE;
        UpdateHotbarSelected(inventory);
    }

    public void DecreaseHotbarIndex(Hotbar inventory)
    {
        //Hack to get around C# mod giving negative values
        hotbarNum = (hotbarNum + HOTBAR_SIZE - 1) % HOTBAR_SIZE;
        UpdateHotbarSelected(inventory);
    }

    public void UpdateHotbarSelected(Hotbar inventory)
    {
        inventory.GetComponentsInChildren<Image>()[previousIndex * 2].color = COLOUR_HOTBAR_DEFAULT;
        inventory.GetComponentsInChildren<Image>()[hotbarNum * 2].color = COLOUR_HOTBAR_SELECTED;
        previousIndex = hotbarNum;
        //Set the held item
        parent.SetHeldItem(GetHotbarItem(), false);
    }

    public void InsertIntoHotbar(int hotbarIndex, int inventoryIndex)
    {
        RemoveIndexFromHotbar(inventoryIndex, false);
        hotbarIndexes[hotbarIndex] = inventoryIndex;
        UpdateHotbarImage(hotbarIndex, false);
    }

    public void UpdateHotbarImage(int index, bool itemDropped)
    {
        int hotBarIndex = hotbarIndexes[index];
        Hotbar hotbar = Object.FindObjectOfType<Hotbar>();
        if(hotBarIndex != -1)
        {
            Item itemInSlot = inventory[hotBarIndex];
            hotbar.GetComponentsInChildren<Image>()[index * 2 + 1].sprite = Resources.Load<Sprite>($"Icons/{itemInSlot.iconName}");
        }
        else
        {
            hotbar.GetComponentsInChildren<Image>()[index * 2 + 1].sprite = Resources.Load<Sprite>($"Icons/icon_blank");
        }
        //If the item was inserted into the slot we are holding, pull it out.
        if(index == hotbarNum)
        {
            parent.SetHeldItem(GetHotbarItem(), itemDropped);
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    public void DropItemAtIndex(int index)
    {
        Item itemAtIndex = inventory[index];
        if(itemAtIndex != null)
        {
            //byeee
            itemAtIndex.DropItem();
        }
    }

    /// <summary>
    /// Removes the index of an item in the inventory from the hotbar
    /// </summary>
    public void RemoveIndexFromHotbar(int inventoryIndexToRemove, bool droppedItem)
    {
        for(int i = 0; i < InventorySystem.HOTBAR_SIZE; i++)
        {
            if(hotbarIndexes[i] == inventoryIndexToRemove)
            {
                hotbarIndexes[i] = -1;
                UpdateHotbarImage(i, droppedItem);
            }
        }
    }

    /// <summary>
    /// For when an item is moved in the inventory but we also want to update the hotbar.
    /// </summary>
    /// <param name="oldIndex"></param>
    /// <param name="newIndex"></param>
    public void HotbarItemMoved(int oldIndex, int newIndex)
    {
        for(int i = 0; i < InventorySystem.HOTBAR_SIZE; i++)
        {
            if(hotbarIndexes[i] == oldIndex)
            {
                hotbarIndexes[i] = newIndex;
                Log.Print($"Hey we just updated index in slot {i} (Updated {oldIndex} to {newIndex})");
            }
        }
    }

    /// <summary>
    /// Simply returns the item inside the inventory of what we have on the hotbar selected.
    /// Can return null.
    /// </summary>
    /// <returns></returns>
    public Item GetHotbarItem()
    {
        int invIndex = hotbarIndexes[hotbarNum];
        //Invalid index
        if(invIndex == -1)
        {
            return null;
        }
        //Inventory
        return inventory[invIndex];
    }

}
