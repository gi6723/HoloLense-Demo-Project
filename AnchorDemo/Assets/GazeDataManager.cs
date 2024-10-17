using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GazeDataManager : MonoBehaviour
{
    private EyeTrackingWebSocket _webSocketClient;
    private Camera _cam;
    public InputActionAsset eyeActionAsset;
    private InputActionMap _eyeActionMap;
    private Dictionary<string, InputAction> _eyeVectorActions;
    private Dictionary<string, InputAction> _eyeRotationActions;

    // These variables were missing. Add them back.
    private Vector3 _headPos;
    private Vector3 _headDir;

    void Start()
    {
        _cam = Camera.main;
        _headPos = _cam.transform.position; // Initialize head position
        _headDir = _cam.transform.forward;  // Initialize head direction
        _webSocketClient = FindObjectOfType<EyeTrackingWebSocket>(); // Reference the WebSocket class

        _eyeActionMap = eyeActionAsset.FindActionMap("EyeActions", true);
        _eyeActionMap.Enable();

        _eyeVectorActions = new Dictionary<string, InputAction>()
        {
            {"position",_eyeActionMap.FindAction("pose/Position")},
            {"velocity",_eyeActionMap.FindAction("pose/Velocity")},
            {"rightEyePosition",_eyeActionMap.FindAction("RightEyePosition")},
            {"leftEyePosition",_eyeActionMap.FindAction("LeftEyePosition")},
            {"centerEyePosition",_eyeActionMap.FindAction("CenterEyePosition")},
        };

        _eyeRotationActions = new Dictionary<string, InputAction>()
        {
            {"angularVelocity",_eyeActionMap.FindAction("CenterEyeAngularVel")},
            { "centerEyeRotation",_eyeActionMap.FindAction("CenterEyeRotation") },
            { "leftEyeRotation",_eyeActionMap.FindAction("LeftEyeRotation") },
            { "rightEyeRotation",_eyeActionMap.FindAction("RightEyeRotation") },
        };
    }

    void Update()
    {
        var currentPos = _cam.transform.position;
        if (ChangePosition(currentPos))
        {
            PrintHeadData(); // Debugging position if needed
        }

        var currentData = new EyeData(Time.time, _eyeVectorActions, _eyeRotationActions);
        
        // Convert the eye data to JSON
        string jsonData = currentData.ToJson();
        
        // Send the JSON data through WebSocket
        _webSocketClient?.SendEyeTrackingData(jsonData); // Ensure the WebSocket client exists
    }

    private bool ChangePosition(Vector3 contender)
    {
        var difference = contender - _headPos;
        if (difference.magnitude > 0.5f)
        {
            _headPos = contender;
            return true;
        }

        return false;
    }

    private void PrintHeadData()
    {
        Debug.Log($"Head pos: {_headPos}\tHead dir: {_headDir}\n");
    }
}
