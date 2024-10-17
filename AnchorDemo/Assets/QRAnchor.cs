using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.MixedReality.OpenXR;
using Microsoft.MixedReality.SampleQRCodes;
using UnityEngine;
using QRCode = Microsoft.MixedReality.QR.QRCode;



public class QRAnchor : MonoBehaviour
{
    
    public QRCode qrCode;
    private SpatialGraphNodeTracker tracker;
    
    // Start is called before the first frame update
    void Start()
    {
        tracker = this.GetComponent<SpatialGraphNodeTracker>();
    }

    // Update is called once per frame
}
