using System;
using WatsonWebsocket;

namespace MiddlemanServer
{
    class Program
    {
        static WatsonWsServer wsServer;
        static WatsonWsClient wsClient;

        static void Main(string[] args)
        {
            // Start WebSocket server on desktop (middleman) to receive data from HoloLens
            string MiddleManIp = "100.79.34.6";  // Bind to all available IPs on the local network
            int localPort = 8081;        // Local port for the middleman

            wsServer = new WatsonWsServer(MiddleManIp, localPort, false);
            wsServer.ClientConnected += ClientConnected;
            wsServer.MessageReceived += MessageReceived;
            wsServer.Start();

            Console.WriteLine($"Middleman WebSocket server started on {MiddleManIp}:{localPort}");
            Console.ReadLine();
        }

        static void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
            Console.WriteLine($"Client connected: {args.IpPort}");
        }

        static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            string receivedData = System.Text.Encoding.UTF8.GetString(args.Data);
            Console.WriteLine($"Data received from client (HoloLens): {receivedData}");

            // Forward the received data to the WebSocket server on Ubuntu VM
            ForwardToUbuntuServer(receivedData);
        }

        static void ForwardToUbuntuServer(string data)
        {
            // Connect to the Watson WebSocket server on the Ubuntu VM
            if (wsClient == null || !wsClient.Connected)
            {
                string ubuntuIp = "100.66.109.48";  // Tailscale IP of the Ubuntu VM
                int ubuntuPort = 8080;              // Port on the Ubuntu VM

                wsClient = new WatsonWsClient(new Uri($"ws://{ubuntuIp}:{ubuntuPort}"));
                wsClient.ServerConnected += (sender, args) => Console.WriteLine("Connected to Ubuntu server");
                wsClient.ServerDisconnected += (sender, args) => Console.WriteLine("Disconnected from Ubuntu server");
                wsClient.Start();
            }

            if (wsClient.Connected)
            {
                wsClient.Send(data);
                Console.WriteLine($"Forwarded data to Ubuntu server: {data}");
            }
            else
            {
                Console.WriteLine("Failed to connect to Ubuntu server.");
            }
        }
    }
}
