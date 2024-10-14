
using MixedReality.Toolkit.Input;
using Microsoft.MixedReality.OpenXR.ARSubsystems;
using UnityEngine;

public class CursorPosition : MonoBehaviour
{
    private FuzzyGazeInteractor _gazeInteractor;
    public GameObject cursorPrefab; // The cursor prefab to be displayed at the gaze point
    private GameObject _cursorInstance;
    private Camera _cam;
    
    void Start()
    {
        _gazeInteractor = FindObjectOfType<FuzzyGazeInteractor>();

        _cursorInstance = Instantiate(cursorPrefab);

        _cam = Camera.main;

        if (_gazeInteractor != null)
        {
            Debug.Log("Gaze Interactor found!");
        }
        else
        {
            Debug.Log("No Gaze Interactor found!");
        }
    }
    
    void Update()
    {
        if (_gazeInteractor != null)
        {
            Vector3 gazeDirection = _gazeInteractor.rayEndPoint;
            _cursorInstance.transform.position = gazeDirection;
            //Debug.Log($"Gaze Endpoint maybe: {gazeDirection}, Normalized: {gazeDirection.normalized}");
        }
    }
}
