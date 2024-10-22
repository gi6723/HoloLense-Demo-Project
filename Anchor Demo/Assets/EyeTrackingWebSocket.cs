using UnityEngine;
using System.Text;
using NativeWebSocket;

public class EyeTrackingWebSocket : MonoBehaviour
{
    private NativeWebSocket.WebSocket _wsClient;
    private string _middlemanIp = "100.79.34.6";  // Middleman server IP
    private int _middlemanPort = 8181;            // Port to connect to

    async void Start()
    {
        // Initialize WebSocket connection to Middleman
        string uri = $"ws://{_middlemanIp}:{_middlemanPort}"; 
        _wsClient = new NativeWebSocket.WebSocket(uri);

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
        // Log the incoming data from the Middleman
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
        Debug.Log($"Attempting to send data to Middleman: {jsonData}");  // Log before sending
    
        if (_wsClient.State == NativeWebSocket.WebSocketState.Open)
        {
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            await _wsClient.Send(data);
            Debug.Log("Data sent successfully.");
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





