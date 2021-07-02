using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry.Commands;
using TMPro;

public class VRSketchBasedCommander : MonoBehaviour
{
    public CommandInvoker Invoker; // CommandeInkover responsbile for the whole scene
    public VRSketchingToolManager ToolManager;
    public UICanvasActivation UIManager;

    public TMP_Text sketchTextMesh;
    public GameObject sketchTextCanvas;
    float textTime;

    public float sketchCounter = 0;
    public float sketchRecognizedFalseCounter = 0;

    public void Start()
    {
        Invoker = new CommandInvoker(); // Initialize CommandInvoker
    }

    // Call the specific command according to the recognized gesture (sketch)
    public void CallCommand(string sketchName, float score)
    {
        switch (sketchName)
        {
            case "Redo":
                Invoker.Redo();
                SetTextController("REDO");
                break;
            case "Undo":
                Invoker.Undo();
                SetTextController("UNDO");
                break;
            case "ColorMenu":
                UIManager.SetColorMenuActiveOrInactive();
                SetTextController("COLOR MENU");
                break;
            case "ScaleMenu":
                UIManager.SetScaleMenuActiveOrInactive();
                SetTextController("SCALE MENU");
                break;
            case "ToolboxMenu":
                UIManager.SetToolBoxMenuActiveOrInactive();
                SetTextController("TOOLBOX MENU");
                break;
            case "LineTool":
                ToolManager.SetVRDrawLinesActive();
                SetTextController("LINE TOOL");
                break;
            case "RibbonTool":
                ToolManager.SetVRDrawRibbonsActive();
                SetTextController("RIBBON TOOL");
                break;
            case "DeleteSketch":
                ToolManager.DeleteSketchWorld();
                SetTextController("DELETE SKETCH");
                break;
            case "ScaleIncrease":
                ToolManager.IncreaseScale();
                SetTextController("SCALE +");
                break;
            case "ScaleDecrease":
                ToolManager.DecreaseScale();
                SetTextController("SCALE -");
                break;
            case "HelpMenu":
                UIManager.SetCommandsInformationDisplayActiveOrInactive();
                SetTextController("HELP MENU");
                break;
            case "CloseUI":
                UIManager.CloseUI();
                SetTextController("CLOSE UI");
                break;
            default:
                SetTextController("NO COMMAND");
                break;
        }
        textTime = 0;
        StartCoroutine(TextActivation());
    }

    public void UndoSketch()
    {
        Invoker.Undo();
    }

    public void RedoSketch()
    {
        Invoker.Redo();
    }

    public void SetTextController(string sketchName)
    {
        sketchTextMesh.text = sketchName;
    }

    private IEnumerator TextActivation()
    {
        textTime = 0;
        sketchTextCanvas.SetActive(true);

        while (textTime < 2)
        {
            yield return null;
            textTime += Time.deltaTime;
        }
        sketchTextCanvas.SetActive(false);
    }
}
