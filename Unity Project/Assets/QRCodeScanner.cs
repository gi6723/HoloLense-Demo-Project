using System;
using System.Collections.Generic;
using Microsoft.Azure.SpatialAnchors.Unity;
using UnityEngine;
using Microsoft.MixedReality.QR;
using UnityEngine.XR.Management;
using Microsoft.Windows.Perception.Spatial.Preview;
using Microsoft.Windows.Perception.Spatial;


public class QRCodeScanner : MonoBehaviour
{
    private QRCodeWatcher qrWatcher;
    private Dictionary<Guid, QRCode> qrCodesList = new Dictionary<Guid, QRCode>();

    // Start is called before the first frame update
    void Start()
    {
        // Check if QRCodeWatcher is supported on the device
        if (QRCodeWatcher.IsSupported())
        {
            // Initialize QRCodeWatcher
            qrWatcher = new QRCodeWatcher();

            // Subscribe to the events
            qrWatcher.Added += QRCodeAdded;
            qrWatcher.Updated += QRCodeUpdated;
            qrWatcher.Removed += QRCodeRemoved;

            // Start scanning
            qrWatcher.Start();
        }
        else
        {
            Debug.LogError("QRCodeWatcher is not supported on this device.");
        }
    }

    // Called when a new QR code is detected
    private void QRCodeAdded(object sender, QRCodeAddedEventArgs e)
    {
        Debug.Log($"QR Code Added: {e.Code.Data}");
        qrCodesList[e.Code.Id] = e.Code;
        UpdateQRCodeTransform(e.Code);
    }

    // Called when a QR code is updated
    private void QRCodeUpdated(object sender, QRCodeUpdatedEventArgs e)
    {
        Debug.Log($"QR Code Updated: {e.Code.Data}");
        qrCodesList[e.Code.Id] = e.Code;
        UpdateQRCodeTransform(e.Code);
    }

    // Called when a QR code is removed
    private void QRCodeRemoved(object sender, QRCodeRemovedEventArgs e)
    {
        Debug.Log($"QR Code Removed: {e.Code.Data}");
        qrCodesList.Remove(e.Code.Id);
    }
    
    // Update the QR code's transform (position and rotation)
    private void UpdateQRCodeTransform(QRCode qrCode)
    {
        // Retrieve the QR code's spatial information
        Vector3 position = qrCode.SpatialGraphNodeId.ToVector3();
        Quaternion rotation = qrCode.SpatialGraphNodeId.ToQuaternion();

        // Log or use the position and rotation
        Debug.Log($"QR Code Position: {position}, Rotation: {rotation}");
    }

    // Clean up when the object is destroyed
    void OnDestroy()
    {
        if (qrWatcher != null)
        {
            qrWatcher.Stop();
            qrWatcher.Added -= QRCodeAdded;
            qrWatcher.Updated -= QRCodeUpdated;
            qrWatcher.Removed -= QRCodeRemoved;
        }
    }
}

// Helper extension methods to convert SpatialGraphNodeId to Unity-friendly types
//In order to so, took advantage of spatial system of world coordinate perception.
public static class QRCodeTransformExtractor
{
    private static SpatialCoordinateSystem worldCoordinateSystem;

    
    public static Vector3 ToVector3(this Guid spatialGraphNodeId)
    {
        
        // Replace this with real implementation to retrieve the actual position
        // based on the spatialGraphNodeId, for now, returning a dummy position
        return new Vector3(0, 0, 0); // Placeholder
    }

    public static Quaternion ToQuaternion(this Guid spatialGraphNodeId)
    {
        // Replace this with real implementation to retrieve the actual rotation
        // based on the spatialGraphNodeId, for now, returning a dummy rotation
        return Quaternion.identity; // Placeholder
    }
}

