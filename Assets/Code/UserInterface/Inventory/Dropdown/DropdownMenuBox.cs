using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownMenuBox : MonoBehaviour
{

    public const int startingPosition = 0;
    public const int optionDistance = -32;

    private static DropdownButton DropdownOption;
    
    private void FindDropdownOption()
    {
        DropdownOption = Resources.Load<DropdownButton>("UserInterface/DropdownButton");
    }   

    public void SetOptions(DropdownOption[] options)
    {
        //Check to make sure the dropdown option exists
        if(DropdownOption == null)
        {
            FindDropdownOption();
        }
        //Instantiate each option
        int i = 0;
        foreach(DropdownOption option in options)
        {
            DropdownButton createdMenu = Object.Instantiate<DropdownButton>(DropdownOption, transform);
            createdMenu.Setup(option.displayName, option.callback);
            //Make the button on top of everything so pointer intercepts work.
            createdMenu.transform.localPosition = new Vector3(0, startingPosition + optionDistance * (i++));
        }
    }

}
