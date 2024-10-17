// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Microsoft.MixedReality.QR;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Microsoft.MixedReality.SampleQRCodes
{
    [RequireComponent(typeof(SpatialGraphNodeTracker))]
    public class QRButtonController : MonoBehaviour
    {
        /*
        public Microsoft.MixedReality.QR.QRCode qrCode;
        private GameObject qrCodeCube;
        public FarmhandManager client;
        public float PhysicalSize { get; private set; }
        public string CodeText { get; private set; }

        [SerializeField] private bool qrTracking = true;

        [SerializeField] public int current_state = 0;
        
        private long lastTimeStamp = 0;
        private Printer _AssociatedPrinter;
        [SerializeField] public TextMeshProUGUI textLabel;
        [SerializeField] private GameObject statusMenu;
        [SerializeField] private GameObject[] nextMenuList;
        
        public Printer AssociatedPrinter
        {
            get
            {
                return _AssociatedPrinter;
            }

            set
            {
                this.textLabel.text = "<size=8>" + value.name + "</size><size=6>" +
                                      "\n<alpha=#88>Press to login</size>";
                this.name = value.name;
                _AssociatedPrinter = value;
            }
        }
        // Use this for initialization
        void Start()
        {
            if (qrCode == null)
            {
                Debug.LogError("Tried to create QRController with empty qr code. Killing gameobject...");
                Destroy(this.gameObject);
            }
            CodeText = qrCode.Data;
            
            PhysicalSize = qrCode.PhysicalSideLength;
            

        }

        void UpdatePropertiesDisplay()
        {
            // Update properties that change
            if (qrCode != null && qrTracking && lastTimeStamp != qrCode.SystemRelativeLastDetectedTime.Ticks)
            {
                PhysicalSize = qrCode.PhysicalSideLength;
                //this.transform.localPosition = new Vector3(PhysicalSize / 2.0f, PhysicalSize / 2.0f, 0.0f);
                //qrCodeCube.transform.localScale = new Vector3(PhysicalSize, PhysicalSize, 0.005f);
                lastTimeStamp = qrCode.SystemRelativeLastDetectedTime.Ticks;
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePropertiesDisplay();
        }

        public void OnClicked()
        {
            //TODO: Farmhand manager needs to be made aware of all QR Codes in order to properly determine
            client.currentPrinter = this.AssociatedPrinter;
            client.requestPrinterSwitch();
            client.requestPrinterUpdate(this.AssociatedPrinter.name);
            textLabel.text = "<size=8>Loading...</size><size=6>" +
                             "\n<alpha=#88>Please wait.</size>";
            client.onPrinterUpdateReceived += OnPrinterReady;
        }

        void OnPrinterReady(Printer printer)
        {
            //If this event came because of a different printer, show ourselves available again
            if (client.currentPrinter != this.AssociatedPrinter)
            {
                //Sounds like a different printer has been activated, lets store whether we have been leveled or not and then disable.
                this.textLabel.text = "<size=8>" + this.AssociatedPrinter.name + "</size><size=6>" +
                                      "\n<alpha=#88>Press to login</size>";
                this.gameObject.SetActive(true);
                
                return;
            }
            else if (!this.gameObject.activeInHierarchy)
            {
                //if we aren't active, then we are the current login and don't need to pay attention to these calls
                return;
            }
            else
            {
                client.currentLogin = this;
            }
            
            if (printer.current_state == "standby" || printer.current_state == "cancelled" ||
                printer.current_state == "complete")
            {
                Debug.Log("Restoring state, loggin in");
                Instantiate(nextMenuList[current_state]);
            }
            else
            {
                //TODO: CHECK TO MAKE SURE PRINTER DOESNT ALREADY HAVE A STAT OVERLAY OPEN
                Debug.Log(client.currentPrinter.current_state);
                Instantiate(statusMenu);
            }
            //TODO: Need to implement proper handling of other recognized qr codes once one printer is selected
            this.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            client.onPrinterUpdateReceived -= OnPrinterReady;
        }
*/
    }
    
}
