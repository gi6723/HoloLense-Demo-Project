using System.Collections;
using System.Collections.Generic;
using MixedReality.Toolkit.Input;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class CubeGameManager : MonoBehaviour
{

    public GameObject CubePrefab;
    public float gazeDuration = 2f; // Time in seconds to gaze at the cube
    private FuzzyGazeInteractor fuzzyGazeInteractor;
    private GameObject currentCube = null;
    private bool isGazing;
    private float gazeTimer;
    private int iteration;
    
    // Start is called before the first frame update
    void Start()
    {
        gazeTimer = 0;
        fuzzyGazeInteractor = FindObjectOfType<FuzzyGazeInteractor>();
        iteration = 0;
    }

    private void SpawnCube()
    {
        Vector3 offset;
        switch (iteration % 4)
        {
            case 0:
                offset = Vector3.left;
                break;
            case 1:
                offset = Vector3.up;
                break;
            case 2:
                offset = Vector3.right;
                break;
            case 3:
                offset = Vector3.down;
                break;
            default:
                offset = Vector3.zero;
                break;
        }

        iteration++;
        currentCube = Instantiate(CubePrefab, transform.localPosition + (offset * .2f), Quaternion.identity);
    }


    public void StartLoop()
    {
        SpawnCube();
    }
    // Update is called once per frame
    void Update()
    {
        if (currentCube != null)
        {
            if (fuzzyGazeInteractor.rayEndTransform.name.Contains("Cube"))
            {
                isGazing = true;
            }
            else
            {
                isGazing = false;
            }
            if (isGazing)
            {
                gazeTimer += Time.deltaTime;
                if (gazeTimer >= gazeDuration)
                {
                    Destroy(currentCube); // Destroy the current cube after gazing
                    SpawnCube(); // Spawn the next cube above
                    gazeTimer = 0f;
                }
            }
            else
            {
                gazeTimer = 0f; // Reset the gaze timer if not gazing
            }
        }
    }
}
