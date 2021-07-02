using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognizedSketchText : MonoBehaviour
{
    public TextMesh sketchTextMesh;

    // Update text display with the lastest recognzied gesture (sketch)
    public void SetText(string sketchName, float sketchScore)
    {
        sketchTextMesh.text = "RECOGNIZED SKETCH \n" + sketchName + "\n" + sketchScore; ;
    }
}
