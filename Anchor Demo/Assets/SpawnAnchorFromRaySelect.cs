using System.Collections;
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
}
