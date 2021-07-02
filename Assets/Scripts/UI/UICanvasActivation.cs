using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasActivation : MonoBehaviour
{
    public GameObject UICanvas;
    public GameObject ScaleMenu;
    public GameObject ColorMenu;
    public GameObject ToolBoxMenu;
    public GameObject HelpMenu;
    public GameObject CommandsInformationDisplay;

    public void SetUICanvasActiveOrInactive(bool status)
    {
        UICanvas.SetActive(status);
    }

    public void SetScaleMenuActiveOrInactive()
    {
        SetUICanvasActiveOrInactive(true);

        if (ScaleMenu.activeSelf)
        {
            ScaleMenu.SetActive(false);
            SetUICanvasActiveOrInactive(false);
        }
        else
        {
            ScaleMenu.SetActive(true);
            SetUICanvasActiveOrInactive(true);

            ColorMenu.SetActive(false);
            ToolBoxMenu.SetActive(false);
            HelpMenu.SetActive(false);
        }
    }

    public void SetColorMenuActiveOrInactive()
    {
        SetUICanvasActiveOrInactive(true);
        if (ColorMenu.activeSelf)
        {
            ColorMenu.SetActive(false);
            SetUICanvasActiveOrInactive(false);
        }
        else
        {
            ColorMenu.SetActive(true);
            SetUICanvasActiveOrInactive(true);

            ScaleMenu.SetActive(false);
            ToolBoxMenu.SetActive(false);
            HelpMenu.SetActive(false);
        }
    }

    public void SetToolBoxMenuActiveOrInactive()
    {
        SetUICanvasActiveOrInactive(true);
        if (ToolBoxMenu.activeSelf)
        {
            ToolBoxMenu.SetActive(false);
            SetUICanvasActiveOrInactive(false);
        }
        else
        {
            ToolBoxMenu.SetActive(true);
            SetUICanvasActiveOrInactive(true);

            ScaleMenu.SetActive(false);
            ColorMenu.SetActive(false);
            HelpMenu.SetActive(false);
        }
    }

    public void SetCommandsInformationDisplayActiveOrInactive()
    {
        if (CommandsInformationDisplay.activeSelf)
        {
            CommandsInformationDisplay.SetActive(false);
            HelpMenu.SetActive(false);
            SetUICanvasActiveOrInactive(false);
        }
        else
        {
            CommandsInformationDisplay.SetActive(true);
            HelpMenu.SetActive(true);
            SetUICanvasActiveOrInactive(true);

            ColorMenu.SetActive(false);
            ToolBoxMenu.SetActive(false);
            ScaleMenu.SetActive(false);
        }
    }

    public void CloseUI()
    {
        SetUICanvasActiveOrInactive(false);
        HelpMenu.SetActive(false);
        ColorMenu.SetActive(false);
        ToolBoxMenu.SetActive(false);
        ScaleMenu.SetActive(false);
    }
}
