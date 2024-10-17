using System;
using UnityEngine;
using WatsonWebsocket;

namespace WatsonWebsocket
{
    public class WatsonWebSocketClient : MonoBehaviour
    {
        private WatsonWsClient _wsClient;

        void Start()
        {
            string middlemanIp = "100.79.34.6"; // IP of the MiddlemanServer (Desktop)
            int middlemanPort = 8181; // Port the MiddlemanServer listens on

            _wsClient = new WatsonWsClient(new Uri($"ws://{middlemanIp}:{middlemanPort}"));

            _wsClient.ServerConnected += (sender, e) => Debug.Log("Connected to Middleman Server");
            _wsClient.ServerDisconnected += (sender, e) => Debug.Log("Disconnected from Middleman Server");

            _wsClient.MessageReceived += (sender, e) =>
            {
                string message = System.Text.Encoding.UTF8.GetString(e.Data);
                Debug.Log($"Message received: {message}");
            };

            _wsClient.StartAsync(); // Ensure the client starts
        }
    }
}