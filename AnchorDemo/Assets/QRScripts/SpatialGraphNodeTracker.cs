// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using UnityEngine;

//#if MIXED_REALITY_OPENXR
using Microsoft.MixedReality.OpenXR;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;

//#else
//using SpatialGraphNode = Microsoft.MixedReality.SampleQRCodes.WindowsXR.SpatialGraphNode;
//#endif

namespace Microsoft.MixedReality.SampleQRCodes
{
    internal class SpatialGraphNodeTracker : MonoBehaviour
    {
        private SpatialGraphNode node;
        private Camera CameraCache;
        public System.Guid Id { get; set; }

        private Pose old_pose = Pose.identity;
        private void Start()
        {
            CameraCache = Camera.main;
        }

        void Update()
        {
            if (node == null || node.Id != Id)
            {
                node = (Id != System.Guid.Empty) ? SpatialGraphNode.FromStaticNodeId(Id) : null;
                //Debug.Log("Initialize SpatialGraphNode Id= " + Id);
            }

            if (node != null)
            {
//#if MIXED_REALITY_OPENXR
                if (node.TryLocate(FrameTime.OnUpdate, out Pose pose))
//#else
                //if (node.TryLocate(out Pose pose))
//#endif
                {
                    
                    // If there is a parent to the camera that means we are using teleport and we should not apply the teleport
                    // to these objects so apply the inverse
                    if (CameraCache.transform.parent != null)
                    {
                        pose = pose.GetTransformedBy(CameraCache.transform.parent);
                    }
                    
                    //Debug.Log(Vector3.Distance(old_pose.position, pose.position).ToString() + "len" + Math.Abs(Vector3.Distance(old_pose.rotation.eulerAngles, pose.rotation.eulerAngles)).ToString());
                    if (Vector3.Distance(old_pose.position, pose.position) > 0.1)
                    {
                    
                        pose.rotation *= Quaternion.Euler(180,0,0);
                        this.gameObject.transform.SetWorldPose(pose);
                        
                        old_pose = gameObject.transform.GetWorldPose();
                    
                    }
                    
                    //Debug.Log("Id= " + Id + " QRPose = " + pose.position.ToString("F7") + " QRRot = " + pose.rotation.ToString("F7"));
                }
                else
                {
                    Debug.LogWarning("Cannot locate " + Id);
                }
            }
        }
    }
}