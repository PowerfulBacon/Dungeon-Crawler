using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUserInterfaceModule : PlayerModule
{

    private static bool inventoryOpen = false;
    private InventoryUi inventoryUi;
    private Hotbar hotbarParent;
    private GameObject cursorObject;

    //Dragging inventory items around interactions
    private bool isDragging;
    private Vector3 startingDragImagePosition;
    private Image draggingImage;
    private int dragStartIndex;

    public override void OnInitialise(Player parent)
    {
        //Hide the cursor.
        Cursor.visible = false;
        //Get the inventory
        inventoryUi = Object.FindObjectOfType<InventoryUi>();
        //Get the hotbar
        hotbarParent = Object.FindObjectOfType<Hotbar>();
        //Find the cursor object
        cursorObject = GameObject.FindGameObjectWithTag("Cursor");
    }

    public override void OnUpdate(Player parent)
    {
        //Get button
        if(Input.GetButtonDown("inventory"))
        {
            //Update the inventory open
            inventoryOpen = !inventoryOpen;
            //Set lockstate
            Cursor.lockState = inventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
            //Update inventory
            inventoryUi.SetOpen(inventoryOpen);
            //Put the cursor in the center of the screen.
            cursorObject.transform.localPosition = Vector3.zero;
        }
        //Update hotbar
        //TODO: Have this in controls or something
        if(Input.mouseScrollDelta.y > 0.2f)
        {
            parent.inventory.IncreaseHotbarIndex(hotbarParent);
        }
        else if(Input.mouseScrollDelta.y < -0.2f)
        {
            parent.inventory.DecreaseHotbarIndex(hotbarParent);
        }
        //Hotbar buttons
        HandleHotbarButtons(parent);
        //Update cursor
        if(inventoryOpen)
        {
            //Set the position of the cursor object
            cursorObject.transform.position = Input.mousePosition;
            //Perform actions!
            //===== Drag Handling =====
            //Todo: Dragging from the hotbar too
            if(Input.GetMouseButtonDown(0) && !isDragging)
            {
                //Get the pointer position
                Vector3 pointerPosition = Input.mousePosition;
                //Find the index of what we are over
                int index = InventoryUi.CursorPositionToIndex(pointerPosition.x, pointerPosition.y);
                //Check if something is even there
                if(index != -1 && parent.inventory.inventory[index] != null)
                {
                    //Find the image we are dragging
                    draggingImage = inventoryUi.GetComponentsInChildren<Image>()[index * 2 + 1];
                    startingDragImagePosition = draggingImage.transform.localPosition;
                    dragStartIndex = index;
                    //Begin dragging
                    isDragging = true;
                }
            }
            //If Dragging
            if(isDragging)
            {
                draggingImage.transform.position = Input.mousePosition;
            }
            //End Dragging
            if(Input.GetMouseButtonUp(0) && isDragging)
            {
                //Reset the image
                draggingImage.transform.localPosition = startingDragImagePosition;
                draggingImage = null;
                isDragging = false;
                //Apply effects of the drag
                //Get the pointer position
                Vector3 pointerPosition = Input.mousePosition;
                //Find the index of what we are over
                int index = InventoryUi.CursorPositionToIndex(pointerPosition.x, pointerPosition.y);
                if(index != -1 && parent.inventory.inventory[index] == null)
                {
                    parent.inventory.SwapInventoryIndexes(dragStartIndex, index);
                }
                else
                {
                    //Check for putting into the hotbar
                    int hotbarRequest = InventoryUi.CursorPositionToHotbarIndex(hotbarParent, pointerPosition.x, pointerPosition.y);
                    if(hotbarRequest != -1)
                    {
                        Log.PrintDebug($"{hotbarRequest}");
                        parent.inventory.InsertIntoHotbar(hotbarRequest, dragStartIndex);
                    }
                    else
                    {
                        //Dragged onto blank space, assume user wants to drop the item.
                        parent.inventory.DropItemAtIndex(dragStartIndex);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Stupid handling of hotkey buttons
    /// </summary>
    /// <param name="parent"></param>
    private void HandleHotbarButtons(Player parent)
    {
        if(Input.GetButtonDown("hb1"))
        {
            parent.inventory.SetHotbarIndex(hotbarParent, 0);
        }
        if(Input.GetButtonDown("hb2"))
        {
            parent.inventory.SetHotbarIndex(hotbarParent, 1);
        }
        if(Input.GetButtonDown("hb3"))
        {
            parent.inventory.SetHotbarIndex(hotbarParent, 2);
        }
        if(Input.GetButtonDown("hb4"))
        {
            parent.inventory.SetHotbarIndex(hotbarParent, 3);
        }
        if(Input.GetButtonDown("hb5"))
        {
            parent.inventory.SetHotbarIndex(hotbarParent, 4);
        }
        if(Input.GetButtonDown("hb6"))
        {
            parent.inventory.SetHotbarIndex(hotbarParent, 5);
        }
        if(Input.GetButtonDown("hb7"))
        {
            parent.inventory.SetHotbarIndex(hotbarParent, 6);
        }
        if(Input.GetButtonDown("hb8"))
        {
            parent.inventory.SetHotbarIndex(hotbarParent, 7);
        }
        if(Input.GetButtonDown("hb9"))
        {
            parent.inventory.SetHotbarIndex(hotbarParent, 8);
        }
        if(Input.GetButtonDown("hb0"))
        {
            parent.inventory.SetHotbarIndex(hotbarParent, 9);
        }
    }

    public static bool IsMouseLocked()
    {
        return !inventoryOpen;
    }

}
