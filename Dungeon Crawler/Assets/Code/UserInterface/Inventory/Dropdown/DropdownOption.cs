using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DropdownOption
{

    public DropdownOption(string _displayName, DropdownButton.DropdownPressDelegate _callback)
    {
        displayName = _displayName;
        callback = _callback;
    }

    public string displayName;
    public DropdownButton.DropdownPressDelegate callback;

}
