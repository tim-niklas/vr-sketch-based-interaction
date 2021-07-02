using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using VRSketchingGeometry;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.Commands.Ribbon;
using VRSketchingGeometry.SketchObjectManagement;

public class VRDrawRibbons : MonoBehaviour
{
    // Controller settings
    public SteamVR_Input_Sources handType; // Left or right hand
    public SteamVR_Action_Boolean actionDrawRibbons; // Action set (select key input of controller)
    public Transform movementSource; // Left or right hand transform
    private bool isPressed = false; // If controller key is pressed
    private bool isMoving = false; // If controller is movin

    // Point positions
    private List<Vector3> positionsList = new List<Vector3>(); // List of positions of the sketch
    public float newPositionTresholdDistance = 0.025f; // Min distance between the last and new points

    // VRSketchGeometry Framework
    public DefaultReferences Defaults;
    private RibbonSketchObject currentRibbonSketchObject;
    public List<RibbonSketchObject> listOfRibbonSketchObjects = new List<RibbonSketchObject>();
    private float ribbonSketchObjectScale = 0.05f;
    private Color ribbonSketchObjectColor = Color.black;

    public VRSketchingToolManager ToolManager; // ToolManager (getting scale and color)
    public VRSketchBasedCommander Commander; // Commander (access CommandInvoker)

    void Start()
    {
        // Set controller input listener
        //actionDrawRibbons.AddOnStateDownListener(TriggerDown, handType);
        //actionDrawRibbons.AddOnStateUpListener(TríggerUp, handType);
    }

    // If controller trigger button is pressed
    /*public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        isPressed = true;
    }

    public void TríggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        isPressed = false;
    }
    */

    private void OnDisable()
    {
        isPressed = false;
    }
    private void OnEnable()
    {
        isPressed = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (actionDrawRibbons.GetStateDown(handType))
        {
            isPressed = true;
        }

        if (actionDrawRibbons.GetStateUp(handType))
        {
            isPressed = false;
        }

        // Start the drawing
        if (!isMoving && isPressed)
        {
            StartDrawRibbon();
        }

        // Ending the drawing
        else if (isMoving && !isPressed)
        {
            EndDrawRibbon();
        }

        // Updating the drawing
        else if (isMoving && isPressed)
        {
            UpdateDrawRibbon();
        }
    }

    void StartDrawRibbon()
    {
        isMoving = true;

        // Set list of position points
        positionsList.Clear();
        positionsList.Add(movementSource.position);

        // Create RibbonSketchObject and store it in the list
        currentRibbonSketchObject = Instantiate(Defaults.RibbonSketchObjectPrefab).GetComponent<RibbonSketchObject>();
        listOfRibbonSketchObjects.Add(currentRibbonSketchObject);

        // Set material color
        SetRibbonSketchObjectColor();
        currentRibbonSketchObject.GetComponent<Renderer>().material.SetColor("_Color", ribbonSketchObjectColor);

        // Set width
        SetRibbonSketchObjectScale();
        currentRibbonSketchObject.SetRibbonScale(Vector3.one * ribbonSketchObjectScale);

        // Set first point of ribbon
        // Invoker.ExecuteCommand(new AddPointAndRotationCommand(currentRibbonSketchObject, movementSource.position, movementSource.rotation));
        currentRibbonSketchObject.AddControlPoint(movementSource.position, movementSource.rotation);

        // Add ribbonSketchObject to SketchWorld
        Commander.Invoker.ExecuteCommand(new AddObjectToSketchWorldRootCommand(currentRibbonSketchObject, ToolManager.SketchWorld));
    }

    void EndDrawRibbon()
    {
        isMoving = false;
    }

    void UpdateDrawRibbon()
    {
        Vector3 lastPosition = positionsList[positionsList.Count - 1];

        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionTresholdDistance)
        {
            positionsList.Add(movementSource.position);
            // Invoker.ExecuteCommand(new AddPointAndRotationCommand(currentRibbonSketchObject, movementSource.position, movementSource.rotation));
            currentRibbonSketchObject.AddControlPoint(movementSource.position, movementSource.rotation);
        }
    }

    public void SetRibbonSketchObjectScale()
    {
        ribbonSketchObjectScale = ToolManager.GetScale();
    }

    public void SetRibbonSketchObjectColor()
    {
        ribbonSketchObjectColor = ToolManager.GetColor();
    }
}

