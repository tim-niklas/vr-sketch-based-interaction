using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

/// <summary>
/// Modified script of the VRColorPicker template.
/// </summary>
public class CircleColorPicker : MonoBehaviour
{
    public GameObject colorPanel;
    public Transform thumb;
    public Image ColorBar;

    public SteamVR_Input_Sources handType; // Left or right hand
    public SteamVR_Action_Boolean actionSphereColorPicker; // Action set (select key input of controller)

    public Image ColorDisplayImage;
    public VRSketchingToolManager ToolManager;

    Color currentColor;
    bool pickerIsInCircle = false;

    [Header("Config")]
    public Transform Picker;

    [Range(0, 5)]
    public float offZ;

    [Header("Freeze posX, posY")]
    public bool fixX;
    public bool fixY;

    public void Start()
    {
        // Set controller input listener
        actionSphereColorPicker.AddOnStateUpListener(SelectColor, handType);
    }

    public void SelectColor(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (pickerIsInCircle)
        {
            currentColor = getImageColor(thumb.localPosition);
            currentColor.a = 1f;
            ToolManager.SetColor(currentColor);

            setColorDisplayImage();
        }
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(Picker.position, Picker.forward);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "ColorCircle")
            {
                pickerIsInCircle = true;
                SetThumbPosition(hit.point);
            }
            else
            {
                pickerIsInCircle = false;
            }
        }
        else
        {
            pickerIsInCircle = false;
        }
    }

    private void SetThumbPosition(Vector3 point)
    {
        Vector3 temp = thumb.localPosition;
        thumb.position = point;
        thumb.localPosition = new Vector3(fixX ? temp.x : thumb.localPosition.x, fixY ? temp.y : thumb.localPosition.y, thumb.localPosition.z + offZ);
        getImageColor(thumb.localPosition);

        showImageColor(getImageColor(thumb.localPosition));
    }

    private Color getImageColor(Vector2 point)
    {
        Vector2 rectPostion = mousePosToImagePos(point);
        Sprite _sprite = colorPanel.GetComponent<Image>().sprite;
        Rect rect = colorPanel.GetComponentInParent<RectTransform>().rect;
        Color imageColor = _sprite.texture.GetPixel(Mathf.FloorToInt(rectPostion.x * _sprite.texture.width / (rect.width)),
                                                     Mathf.FloorToInt(rectPostion.y * _sprite.texture.height / (rect.height)));
        return imageColor;
    }

    private Vector2 mousePosToImagePos(Vector2 point)
    {
        Vector2 ImagePos = Vector2.zero;
        Rect rect = colorPanel.GetComponentInParent<RectTransform>().rect;
        ImagePos.x = point.x - colorPanel.transform.position.x + rect.width * 0.5f;
        ImagePos.y = point.y - colorPanel.transform.position.y + rect.height * 0.5f;
        return ImagePos;
    }

    private void showImageColor(Color _Color)
    {
        _Color.a = 1f;
        ColorBar.color = _Color;
    }

    public void setColorDisplayImage()
    {
        ColorDisplayImage.color = ToolManager.GetColor();
    }
}
