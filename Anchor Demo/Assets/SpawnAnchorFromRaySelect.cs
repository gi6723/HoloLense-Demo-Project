using System.Linq;
using MixedReality.Toolkit.Input;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Processing; // Requires XR Hands package
using UnityEngine.XR.Management;

public class PinchToPlaceAnchorXR : MonoBehaviour
{
    public ARRaycastManager raycastManager;    // AR Raycast Manager
    public ARAnchorManager anchorManager;      // AR Anchor Manager
    public GameObject anchorPrefab;            // Anchor prefab to instantiate (optional)

    private PokeInteractor rightHandPoke;// XR Hands Subsystem
    private PokeInteractor leftHandPoke;
    void Start()
    {
        // Get the XR Hands Subsystem
        var pokes = FindObjectsOfType<PokeInteractor>();
        
        if (pokes == null)
        {
            Debug.LogError("XRHandSubsystem not found. Ensure XR Hands package is installed.");
        }
        else
        {
            Debug.Log($"{pokes.Length} pokes.");
            rightHandPoke = pokes[0];
            leftHandPoke = pokes[1];
        }
        
    }

    void Update()
    {
        //determine whether a selection has been made.
        var place = rightHandPoke.isSelectActive;
        // Raycast to detect planes

        if (place)
        {
            Debug.Log($"{rightHandPoke.PokeTrajectory.End} is selected.");
        }
    }
/*
    // Detect pinch by checking the distance between thumb and index finger joints

    void PlaceAnchor(Pose pose)
    {
        if (anchorManager != null)
        {
            // Place an anchor at the detected plane's position
            ARAnchor anchor = anchorManager.AddAnchor(pose);

            if (anchor != null)
            {
                // Optionally, instantiate a prefab at the anchor position
                if (anchorPrefab != null)
                {
                    Instantiate(anchorPrefab, pose.position, pose.rotation);
                }
                Debug.Log("Anchor placed successfully.");
            }
            else
            {
                Debug.LogError("Failed to place anchor.");
            }
        }
    }*/
}



/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SpawnAnchorFromRaySelect : MonoBehaviour
{
    
    public XRRayInteractor rayInteractor;
    public ARAnchorManager anchorManager;
    public GameObject anchorPrefab;
    // Start is called before the first frame update
    void Start()
    {
        rayInteractor.selectEntered.AddListener(SpawnAnchor);
    }

    public void SpawnAnchor(BaseInteractionEventArgs eventArgs)
    {
        rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit);
        Debug.Log($"Ray Interactor: {hit.collider.gameObject.name}");
        //instantiate the anchor at the position of the ray hit and at a position orthogonal to the plane of contact. (hopefully)
        //anchorManager.AttachAnchor(hit.collider.attachedRigidbody, new Pose(hit.point, Quaternion.LookRotation(-hit.normal)))
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/
