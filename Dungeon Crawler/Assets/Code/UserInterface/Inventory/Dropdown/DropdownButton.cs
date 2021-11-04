using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public static DropdownButton hoveredButton;

    public delegate void DropdownPressDelegate();
    private DropdownPressDelegate _callback;

    [SerializeField]
    private TextMeshProUGUI textMesh;
    [SerializeField]
    private Image image;

    public Color defaultColor;
    public Color overColor;

    public void Setup(string displayText, DropdownPressDelegate pressCallback)
    {
        _callback = pressCallback;
        textMesh.text = displayText;
    }

    /// <summary>
    /// Interface between unity and delegate callback invokation.
    /// </summary>
    public void OnClick()
    {
        _callback?.Invoke();
    }

    //Handle mouse hovering.
    private void OnDestroy()
    {
        if(hoveredButton != this)
            return;
        hoveredButton = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoveredButton = this;
        image.color = overColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(hoveredButton != this)
            return;
        hoveredButton = null;
        image.color = defaultColor;
    }
}
