using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerInputActivation : MonoBehaviour
{
    public GameObject Pointer;
    public GameObject InputModule;

    public GameObject RightHand;
    public VRSketchingToolManager ToolManager;

    bool PointerInputActive;

    // Start is called before the first frame update
    void Start()
    {
        Pointer.SetActive(false);
        //InputModule.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(RightHand.transform.position, RightHand.transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "UICanvas" || hit.transform.tag == "ColorCircle")
            {
                Pointer.SetActive(true);
                PointerInputActive = true;
                InputModule.SetActive(true);
                ToolManager.SetVRSketchingToolBoxActiveOrInactive(false);
          

            }
            else if (PointerInputActive)
            {
                Pointer.SetActive(false);
                InputModule.SetActive(false);
                ToolManager.SetVRSketchingToolBoxActiveOrInactive(true);
                PointerInputActive = false;
            }
        }
        else if (PointerInputActive)
        {
            Pointer.SetActive(false);
            InputModule.SetActive(false);
            ToolManager.SetVRSketchingToolBoxActiveOrInactive(true);
            PointerInputActive = false;
        }
    }
}
