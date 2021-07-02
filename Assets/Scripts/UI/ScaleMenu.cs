using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleMenu : MonoBehaviour
{
    public Slider ScaleSlider;
    public TMP_Text ScaleSliderText;

    public VRSketchingToolManager ToolManager;

    public void Start()
    {
        ScaleSlider.value = ToolManager.GetScale();
    }

    public void Update()
    {
        ScaleSlider.value = ToolManager.GetScale();
    }

    public void ScaleChange()
    {
        float scale = ScaleSlider.value;
        ScaleSliderText.SetText(scale.ToString("0.00") + "m");
        ToolManager.SetScale(scale);
    }
}