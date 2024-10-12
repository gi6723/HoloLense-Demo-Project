using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR;
using InputDevice = UnityEngine.InputSystem.InputDevice;

public class GazeDataManager : MonoBehaviour
{
    // Start is called before the first frame update
    //TODO: Perform proper NULL checks. This implementation shouldn't do anything. If camera were to be Null, though it never will be, an error would occur despite checking it with null.
    private Vector3 _headPos { get; set; }
    private Camera _cam { get; set; }
    private Vector3 _headDir { get; set; }
    
    private EyeData _prevTime { get; set; }

    public XRInputValueReader eyeInputReader;
    
    public InputActionAsset eyeActionAsset;

    private InputActionMap _eyeActionMap;
    
    private Dictionary<string, InputAction> _eyeVectorActions;
    private Dictionary<string, InputAction> _eyeRotationActions;
    
    

    void Start()
    {
        _cam = Camera.main;
        _headPos = _cam.transform.position;
        _headDir = _cam.transform.forward;
        Debug.Log($"Starting head position: {_headPos}\tStarting head direction: {_headDir}\n");
        _eyeActionMap = eyeActionAsset.FindActionMap("EyeActions", true);
        _eyeActionMap.Enable();

        if (_eyeActionMap == null)
        {
            Debug.LogWarning("No eye actions map found.");
        }

        
        
        _eyeVectorActions = new Dictionary<string, InputAction>()
        {
            {"position",_eyeActionMap.FindAction("pose/Position")},
            {"rotation",_eyeActionMap.FindAction("pose/Rotation")},
            {"velocity",_eyeActionMap.FindAction("pose/Velocity")},
            {"rightEyePosition",_eyeActionMap.FindAction("RightEyePosition")},
            {"leftEyePosition",_eyeActionMap.FindAction("LeftEyePosition")},
            {"centerEyePosition",_eyeActionMap.FindAction("CenterEyePosition")},
        };

        if (_eyeVectorActions.Count == 0)
        {
            Debug.LogWarning("No eye vector actions found.");
            
        }

        
        _eyeRotationActions = new Dictionary<string, InputAction>()
        {
            {"angularVelocity",_eyeActionMap.FindAction("CenterEyeAngularVel")},
            { "centerEyeRotation",_eyeActionMap.FindAction("CenterEyeRotation") },
            { "leftEyeRotation",_eyeActionMap.FindAction("LeftEyeRotation") },
            { "rightEyeRotation",_eyeActionMap.FindAction("RightEyeRotation") },
        };
        if (_eyeRotationActions.Count == 0)
        {
            Debug.LogWarning("No eye rotation actions found.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Modified head position adjustment for debug log readability and easier testing. Implemented in a way such that head position will not change unless the
        var currentPos = _cam.transform.position;
        _headDir = _cam.transform.forward;
        if(ChangePosition(currentPos))
            PrintHeadData();
        
        var currentData = new EyeData(Time.time, _eyeVectorActions, _eyeRotationActions);
        Debug.Log(currentData.ToJson());
        
       
    }

    private bool ChangePosition(Vector3 contender)
    {
        var difference = contender - _headPos;
        if (difference.magnitude < 0.5f)
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