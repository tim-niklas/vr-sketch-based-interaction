using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorPicker : MonoBehaviour
{
    public Color ButtonColor; // Select with color the button should set

    public Image ColorDisplayImage;
    public VRSketchingToolManager ToolManager;

    public void ChangeColor()
    {
        ToolManager.SetColor(ButtonColor);
        setColorDisplayImage();
    }

    public void setColorDisplayImage()
    {
        ColorDisplayImage.color = ToolManager.GetColor();
    }
}
