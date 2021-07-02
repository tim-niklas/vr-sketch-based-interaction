using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;

public class VRSketchRecognizer : MonoBehaviour
{
    // Controller settings
    public SteamVR_Input_Sources handType; // Left or right hand
    public SteamVR_Action_Boolean actionRecognizeSketch; // Action set (select key input of controller)
    public Transform movementSource; // Left or right hand transform

    private bool isPressed = false; // If controller key is pressed
    private bool isMoving = false; // If controller is moving

    // Display of gesture (sketch) settings
    private LineRenderer lineRenderer; // lineRenderer draw lines accrding to the given positions
    public Gradient lineRendererGradientNeutral; // Color gradient of gesture (sketch) line
    public Gradient lineRendererGradientFalse; // Color gradient of gesture (sketch) line if there is no correct gesture recognized

    // Recognition settings
    private List<Gesture> trainingSet = new List<Gesture>(); // Training set of gestures
    private List<Vector3> positionsList = new List<Vector3>(); // List of positions of the sketch
    public float newPositionTresholdDistance = 0.025f; // Min distance between the last and new points
    public float recognitionTreshold = 0.80f; // Min value of the recogntion result score

    // Creation of training samples settings
    public bool creationMode = false; // Activate creation mode
    public string newGestureName; // Name of the new created gesture (sketch)

    public VRSketchingToolManager ToolManager;
    public VRSketchBasedCommander Commander;

    // Event system for calling the VRSketchBasedCommander
    [System.Serializable]
    public class UnityEventSketchRecognized : UnityEvent<string, float> { }
    public UnityEventSketchRecognized OnRecognized;

    // Start is called before the first frame update
    void Start()
    {
        // Set controller input listener
        actionRecognizeSketch.AddOnStateDownListener(SketchRecognizerButtonDown, handType);
        actionRecognizeSketch.AddOnStateUpListener(SketchRecognizerButtonUp, handType);

        // Reading gesture training set
        string[] gesturesFiles = Directory.GetFiles(Application.streamingAssetsPath + "/TrainingSet", "*.xml");
        foreach (var item in gesturesFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }

        // Get LineRenderer
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // If controller button is pressed
    public void SketchRecognizerButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        isPressed = true;
    }

    public void SketchRecognizerButtonUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        isPressed = false;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("SketchRecognizerCounter: " + Commander.sketchCounter);
        Debug.Log("SketchFalseRecognizedCounter: " + Commander.sketchRecognizedFalseCounter);
    }

    // Update is called once per frame
    void Update()
    {
        // Start the movement
        if (!isMoving && isPressed)
        {
            StartMovement();
        }
        // Ending the movement
        else if (isMoving && !isPressed)
        {
            EndMovement();
        }
        // Updating the movement
        else if (isMoving && isPressed)
        {
            UpdateMovement();
        }
    }

    void StartMovement()
    {
        isMoving = true;
        ToolManager.SetVRSketchRecognizerAttachmentActive();

        // Set list of position points
        positionsList.Clear();
        positionsList.Add(movementSource.position);

        // LineRenderer
        lineRenderer.positionCount = 0;
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(0, movementSource.position);
        lineRenderer.colorGradient = lineRendererGradientNeutral;
    }

    void EndMovement()
    {
        isMoving = false;
        ToolManager.SetVRSketchRecognizerAttachmentInActive();

        // Creates the gesture (stetch) from the positionsList
        Point[] pointArray = new Point[positionsList.Count];

        for (int i = 0; i < positionsList.Count; i++)
        {
            // Debug.Log(positionsList[i]);
            // Debug.Log(Camera.main.WorldToScreenPoint(positionsList[i]));

            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);

        }

        Gesture newGesture = new Gesture(pointArray);

        if (creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            string fileName = Application.streamingAssetsPath + "/TrainingSet/" + newGestureName + DateTime.Now.ToFileTime().ToString() + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);

            lineRenderer.positionCount = 0;
        }

        else
        {
            Commander.sketchCounter++;
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());

            Debug.Log("GestureClass " + result.GestureClass + " GestureScore " + result.Score);

            if (result.Score > recognitionTreshold) // If gesture score is over recognitionTreshold send gesture name to the VRSketchBasedCommander
            {
                OnRecognized.Invoke(result.GestureClass, result.Score);
                lineRenderer.positionCount = 0;
            }
            else
            {
                Commander.sketchRecognizedFalseCounter++;
                StartCoroutine(FalseRecognized());
            }
        }
    }

    void UpdateMovement()
    {
        Vector3 lastPosition = positionsList[positionsList.Count - 1];

        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionTresholdDistance)
        {
            positionsList.Add(movementSource.position);

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, movementSource.position);
        }
    }

    // If gesture score is under the recognitionTreshold color gradient is changed 
    private IEnumerator FalseRecognized()
    {
        lineRenderer.colorGradient = lineRendererGradientFalse;
        yield return new WaitForSeconds(0.5f);
        lineRenderer.positionCount = 0;
    }
}
