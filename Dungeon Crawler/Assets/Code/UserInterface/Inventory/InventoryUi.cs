using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUi : MonoBehaviour
{

    //WARNING: UNTESTED FOR OTHER VALUES
    //THERE SHOULD BE NO REASON TO CHANGE THESE THOUGH
    private const int UI_CENTER_X = 0;
    private const int UI_CENTER_Y = 470;

    private const int UI_OFFSET_X = 100;
    private const int UI_OFFSET_Y = -100; //inventory moves down

    private const int UI_BORDER = 10;

    private const int HUD_WIDTH = 1920;
    private const int HUD_HEIGHT = 1080;

    // Start is called before the first frame update
    void Start()
    {
        //Find the prefab border component.
        GameObject inventoryGameobject = Resources.Load<GameObject>("UserInterface/Border Component");
        //Calculate position of the top left component
        float halfWidth = (InventorySystem.INVENTORY_WIDTH - 1) / 2.0f;
        int topLeft_x = UI_CENTER_X - (int)(halfWidth * UI_OFFSET_X);
        int topLeft_y = UI_CENTER_Y;
        //Lets start instantiating the thingies.
        for(int x = 0; x < InventorySystem.INVENTORY_WIDTH; x ++)
        {
            for(int y = 0; y < InventorySystem.INVENTORY_HEIGHT; y ++)
            {
                GameObject newObject = Instantiate(inventoryGameobject, Vector3.zero, Quaternion.identity, transform);
                newObject.transform.localPosition = new Vector3(topLeft_x + x * UI_OFFSET_X, topLeft_y + y * UI_OFFSET_Y, 0);
            }
        }
        //Close the UI
        SetOpen(false);
    }

    public void SetOpen(bool opened)
    {
        foreach(Image subComponent in GetComponentsInChildren<Image>())
        {
            subComponent.enabled = opened;
        }
    }

    //Converts a cursor position into the index of thing being horvered.
    //Returns -1 if nothing is hovered over
    public static int CursorPositionToIndex(float cursorX, float cursorY)
    {
        float halfWidth = (InventorySystem.INVENTORY_WIDTH) / 2.0f;
        float halfHeight = (InventorySystem.INVENTORY_HEIGHT) / 2.0f;
        //Calculate the far left
        float left_border = (HUD_WIDTH * 0.5f) - (halfWidth * UI_OFFSET_X) + UI_CENTER_X;
        //Calculate far right
        float right_border = (HUD_WIDTH * 0.5f) + (halfWidth * UI_OFFSET_X) + UI_CENTER_X;
        //Have no idea why we need UI_OFFSET_Y but it works :^)
        //calculate top
        float top_border = (HUD_HEIGHT * 0.5f) + (halfHeight * UI_OFFSET_Y) + UI_CENTER_Y + UI_OFFSET_Y;
        //calculat bottom
        float bottom_border = (HUD_HEIGHT * 0.5f) - (halfHeight * UI_OFFSET_Y) + UI_CENTER_Y + UI_OFFSET_Y;
        //Range check
        if(left_border > cursorX || cursorX > right_border)
        {
            Log.Print("X failure");
            return -1;
        }
        if(bottom_border < cursorY || cursorY < top_border)
        {
            Log.Print("Y failure");
            return -1;
        }
        float xOffset = cursorX - left_border;
        float yOffset = cursorY - bottom_border;
        //Calculate index
        int xIndex = (int)xOffset / UI_OFFSET_X;
        int yIndex = (int)yOffset / UI_OFFSET_Y;
        //yayyy
        return yIndex + xIndex * InventorySystem.INVENTORY_HEIGHT;
    }

    /// <summary>
    /// Converts a mouse position into index of the hotbar.
    /// I am going to be lazy with this one and use an inefficient loop rather than maths.
    /// O(N) instead of O(1) but literally who cares. (I do otherwise I wouldn't be writting this.)
    /// TODO: This method is awful and needs revising but I will probably never end up doing it :^).
    /// </summary>
    /// <param name="cursorX"></param>
    /// <param name="cursorY"></param>
    /// <returns></returns>
    public static int CursorPositionToHotbarIndex(Hotbar hotbar, float cursorX, float cursorY)
    {
        //aww why :(
        Image[] cachedImages = hotbar.GetComponentsInChildren<Image>();
        for (int i = 0; i < InventorySystem.HOTBAR_SIZE; i ++)
        {
            Image hotbarCurrent = cachedImages[i * 2];
            RectTransform hotbarRectTransform = hotbarCurrent.transform as RectTransform;
            //Even worse
            Vector2 bottomLeft = new Vector2(hotbarRectTransform.position.x, hotbarRectTransform.position.y) - new Vector2(50, 50);
            Vector2 topRight = new Vector2(hotbarRectTransform.position.x, hotbarRectTransform.position.y) + new Vector2(50, 50);
            if(bottomLeft.x > cursorX || cursorX > topRight.x)
            {
                continue;
            }
            if(bottomLeft.y > cursorY || cursorY > topRight.y)
            {
                continue;
            }
            return i;
        }
        return -1;
    }

}
