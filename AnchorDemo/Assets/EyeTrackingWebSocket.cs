using UnityEngine;
using System.Text;
using NativeWebSocket; // Ensure you are using NativeWebSocket

public class EyeTrackingWebSocket : MonoBehaviour
{
    private NativeWebSocket.WebSocket _wsClient; // Specify NativeWebSocket.WebSocket
    private string _middlemanIp = "100.79.34.6";  // Your Middleman server IP
    private int _middlemanPort = 8181; // Port to connect to

    async void Start()
    {
        // Initialize WebSocket connection to Middleman
        string uri = $"ws://{_middlemanIp}:{_middlemanPort}"; 
        _wsClient = new NativeWebSocket.WebSocket(uri); // Specify NativeWebSocket.WebSocket

        // Subscribe to WebSocket events
        _wsClient.OnOpen += OnOpen;
        _wsClient.OnMessage += OnMessage;
        _wsClient.OnError += OnError;
        _wsClient.OnClose += OnClose;

        // Connect to the server
        await _wsClient.Connect();
        Debug.Log("Connecting to Middleman Server...");
    }

    private void OnOpen()
    {
        Debug.Log("Connected to Middleman Server");
    }

    private void OnMessage(byte[] data)
    {
        string message = Encoding.UTF8.GetString(data);
        Debug.Log($"Message received from Middleman: {message}");
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
        if (_wsClient.State == NativeWebSocket.WebSocketState.Open) // Specify NativeWebSocket.WebSocketState
        {
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            await _wsClient.Send(data);
            Debug.Log("Eye-tracking data sent to Middleman");
        }
        else
        {
            Debug.LogWarning("WebSocket is not connected. Cannot send data.");
        }
    }

    private async void OnApplicationQuit()
    {
        // Properly close the connection when exiting
        if (_wsClient != null)
        {
            await _wsClient.Close(); // Close the connection
        }
    }

    private void Update() // Change this to void instead of async void
    {
        // Update the WebSocket to process incoming messages
        if (_wsClient != null)
        {
            _wsClient.DispatchMessageQueue(); // Call DispatchMessageQueue without await
        }
    }
}




