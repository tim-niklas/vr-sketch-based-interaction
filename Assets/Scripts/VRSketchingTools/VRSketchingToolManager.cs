using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry;
using VRSketchingGeometry.SketchObjectManagement;

public class VRSketchingToolManager : MonoBehaviour
{
    public Color VRSketchingToolColor = Color.black; // color of all sketch tools
    public float VRSketchingToolScale = 0.05f; // Scale of all sketch tools

    public SketchWorld SketchWorld; // SketchWorld of scene
    public DefaultReferences Defaults;

    public GameObject[] VRSketchingTools;
    public GameObject VRSketchingToolBox;

    public GameObject[] VRDrawLinesTool;
    public GameObject[] VRDrawLinesToolAttachment;
    public GameObject[] VRDrawLinesToolAttachmentModels;

    public GameObject[] VRDrawRibbonsTool;
    public GameObject[] VRDrawRibbonsToolAttachment;
    public GameObject[] VRDrawRibbonsToolAttachmentModels;

    public GameObject[] VRSketchRecognizerAttachments;
    public GameObject[] VRSketchingToolAttachments;


    public void Start()
    {
        // Create a SketchWorld, many commands require a SketchWorld to be present
        SketchWorld = Instantiate(Defaults.SketchWorldPrefab).GetComponent<SketchWorld>();
    }

    public void DeleteSketchWorld()
    {
        foreach (Transform child in SketchWorld.transform)
        {
            Destroy(child.gameObject);
        }
        SketchWorld = Instantiate(Defaults.SketchWorldPrefab).GetComponent<SketchWorld>();
    }

    public void SetVRDrawLinesActiveOrInactive(bool status)
    {
        foreach (GameObject gameObject in VRDrawLinesTool)
        {
            gameObject.SetActive(status);
        }

        foreach (GameObject gameObject in VRDrawLinesToolAttachment)
        {
            gameObject.SetActive(status);
        }
    }

    public void SetVRDrawRibbonsActiveOrInactive(bool status)
    {
        foreach (GameObject gameObject in VRDrawRibbonsTool)
        {
            gameObject.SetActive(status);
        }
        foreach (GameObject gameObject in VRDrawRibbonsToolAttachment)
        {
            gameObject.SetActive(status);
        }
    }

    public void SetVRSketchRecognizerAttachmentActiveOrInactive(bool status)
    {
        foreach (GameObject gameObject in VRSketchRecognizerAttachments)
        {
            gameObject.SetActive(status);
        }
    }

    public void SetVRDrawLinesActive()
    {
        SetVRDrawRibbonsActiveOrInactive(false);
        SetVRDrawLinesActiveOrInactive(true);
    }

    public void SetVRDrawRibbonsActive()
    {

        SetVRDrawLinesActiveOrInactive(false);
        SetVRDrawRibbonsActiveOrInactive(true);
    }

    public void SetVRSketchingToolBoxActiveOrInactive(bool status)
    {
        VRSketchingToolBox.SetActive(status);
    }

    public void SetVRSketchRecognizerAttachmentActive()
    {
        SetVRSketchRecognizerAttachmentActiveOrInactive(true);

        foreach (GameObject gameObject in VRSketchingToolAttachments)
        {
            gameObject.SetActive(false);
        }

        SetVRSketchingToolBoxActiveOrInactive(false);
    }

    public void SetVRSketchRecognizerAttachmentInActive()
    {
        SetVRSketchRecognizerAttachmentActiveOrInactive(false);

        foreach (GameObject gameObject in VRSketchingToolAttachments)
        {
            gameObject.SetActive(true);
        }

        SetVRSketchingToolBoxActiveOrInactive(true);
    }


    public void SetColorOfToolAttachments()
    {
        foreach (GameObject attachment in VRDrawLinesToolAttachmentModels)
        {
            Color VRSketchingToolColorTransparent = VRSketchingToolColor;
            VRSketchingToolColorTransparent.a = 0.5f;
            attachment.GetComponent<Renderer>().material.SetColor("_Color", VRSketchingToolColorTransparent);
        }

        foreach (GameObject attachment in VRDrawRibbonsToolAttachmentModels)
        {
            Color VRSketchingToolColorTransparent = VRSketchingToolColor;
            VRSketchingToolColorTransparent.a = 0.5f;
            attachment.GetComponent<Renderer>().material.SetColor("_Color", VRSketchingToolColorTransparent);
        }
    }

    public void SetScaleOfToolAttachments()
    {
        foreach (GameObject attachment in VRDrawLinesToolAttachment)
        {
            attachment.transform.localScale = new Vector3(VRSketchingToolScale, VRSketchingToolScale, VRSketchingToolScale);
        }

        foreach (GameObject attachment in VRDrawRibbonsToolAttachment)
        {
            attachment.transform.localScale = new Vector3(VRSketchingToolScale, VRSketchingToolScale, VRSketchingToolScale);
        }
    }

    public void SetScaleAndColorOfToolAttachments()
    {
        SetScaleOfToolAttachments();
        SetColorOfToolAttachments();
    }

    public void SetAllToolsAttachmentApperances()
    {
        SetScaleAndColorOfToolAttachments();
        SetScaleAndColorOfToolAttachments();
    }

    public void SetColor(Color color)
    {
        VRSketchingToolColor = color;
        SetAllToolsAttachmentApperances();
    }

    public Color GetColor()
    {
        return VRSketchingToolColor;
    }

    public void SetScale(float scale)
    {
        VRSketchingToolScale = scale;
        SetAllToolsAttachmentApperances();
    }

    public void IncreaseScale()
    {
        if (VRSketchingToolScale <= 0.09f)
        {
            VRSketchingToolScale += 0.01f;
        }
        SetAllToolsAttachmentApperances();
    }

    public void DecreaseScale()
    {
        if (VRSketchingToolScale >= 0.02f)
        {
            VRSketchingToolScale -= 0.01f;
        }
        SetAllToolsAttachmentApperances();
    }

    public float GetScale()
    {
        return VRSketchingToolScale;
    }

}
