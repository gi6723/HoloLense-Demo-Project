using System;
using UnityEngine;
using System.Text;
using NativeWebSocket;
using Newtonsoft.Json;

public class EyeTrackingWebSocket : MonoBehaviour
{
    private NativeWebSocket.WebSocket _wsClient;
    private string _middlemanIp = "100.79.34.6";  // Middleman server IP
    private int _middlemanPort = 8282;            // Port to connect to

    async void Start()
    {
        //Debug.Log("EyeTrackingWebSocket Start() called.");

        // Initialize WebSocket connection to Middleman
        string uri = $"ws://{_middlemanIp}:{_middlemanPort}";
        //Debug.Log($"Attempting to connect to WebSocket server at {uri}");
        _wsClient = new NativeWebSocket.WebSocket(uri);

        // Subscribe to WebSocket events
        _wsClient.OnOpen += OnOpen;
        _wsClient.OnMessage += OnMessage;
        _wsClient.OnError += OnError;
        _wsClient.OnClose += OnClose;

        try
        {
            // Connect to the server
            await _wsClient.Connect();
            //Debug.Log("WebSocket Connect() called.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"WebSocket connection failed: {ex.Message}");
        }

        Debug.Log("EyeTrackingWebSocket Start() completed.");
    }

    private void OnOpen()
    {
        Debug.Log("Connected to Middleman Server");
    }

    private void OnMessage(byte[] data)
    {
        // Log the incoming data from the Middleman
        string message = Encoding.UTF8.GetString(data);
        //Debug.Log($"Message received from Middleman: {message}");
    }

    private void OnError(string errorMsg)
    {
        Debug.LogError($"WebSocket error: {errorMsg}");
    }

    private void OnClose(WebSocketCloseCode closeCode)
    {
        Debug.Log($"Disconnected from Middleman Server. Close code: {closeCode}");
    }

    public async void SendEyeTrackingData(string jsonData)
    {
        // Log before sending
        Debug.Log($"Attempting to send data to Middleman: {jsonData}");

        if (_wsClient.State == NativeWebSocket.WebSocketState.Open)
        {
            try
            {
                // Wrap the eye tracking data with the message type
                var message = new
                {
                    type = "EyeData",
                    data = jsonData
                };
                // Serialize the wrapped message to JSON
                string wrappedMessage = JsonConvert.SerializeObject(message);
                byte[] data = Encoding.UTF8.GetBytes(wrappedMessage);
                // Send the serialized message
                await _wsClient.Send(data);
                //Debug.Log("Eye-tracking data sent successfully.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to send data: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("WebSocket is not connected. Cannot send data.");
        }
    }

    public async void SendGameEvent(string eventType, string timestamp, string details)
    {
        //Debug.Log($"Attempting to send game event to Middleman: {eventType}, {timestamp}, {details}");  // Log before sending

        if (_wsClient.State == WebSocketState.Open)
        {
            try
            {
                var eventMessage = new
                {
                    type = "GameEventData",
                    data = new
                    {
                        EventType = eventType,
                        Timestamp = timestamp,
                        Details = details
                    }
                };

                string jsonData = JsonConvert.SerializeObject(eventMessage);
                byte[] data = Encoding.UTF8.GetBytes(jsonData);
                await _wsClient.Send(data);
                Debug.Log("Game event data sent successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to send game event data: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("WebSocket is not connected. Cannot send game event data.");
        }
    }
    
    private async void OnApplicationQuit()
    {
        // Properly close the connection when exiting
        if (_wsClient != null)
        {
            await _wsClient.Close();
            Debug.Log("WebSocket connection closed.");
        }
    }

    private void Update()
    {
        // Update the WebSocket to process incoming messages
        if (_wsClient != null)
        {
            _wsClient.DispatchMessageQueue();
        }
    }
}






