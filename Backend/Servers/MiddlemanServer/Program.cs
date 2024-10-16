using WatsonWebsocket;
using System;

class Program
{
    static WatsonWsServer wsMiddleman;
    static WatsonWsClient wsClientToMainServer;

    static void Main(string[] args)
    {
        string middlemanIp = "100.79.34.6";  // IP of the MiddlemanServer (Desktop)
        int middlemanPort = 8181;            // Port the MiddlemanServer listens on

        string mainServerIp = "100.121.5.87";  // IP of the MainServer (VM)
        int mainServerPort = 8181;             // Port to forward data to

        // Start Middleman WebSocket Server
        wsMiddleman = new WatsonWsServer(middlemanIp, middlemanPort, false);
        wsMiddleman.ClientConnected += (sender, args) =>
        {
            Console.WriteLine($"Client connected: {args.IpPort}");
        };
        wsMiddleman.MessageReceived += (sender, e) => ForwardToMainServer(e.Data);
        wsMiddleman.Start();

        // Set up the connection to the MainServer
        wsClientToMainServer = new WatsonWsClient(new Uri($"ws://{mainServerIp}:{mainServerPort}"));
        wsClientToMainServer.ServerConnected += (sender, e) => Console.WriteLine("Connected to MainServer");
        wsClientToMainServer.ServerDisconnected += (sender, e) => Console.WriteLine("Disconnected from MainServer");
        wsClientToMainServer.Start();

        Console.WriteLine($"Middleman WebSocket server started on {middlemanIp}:{middlemanPort}");
        Console.ReadLine();  // Keep the server running
    }

    static void ClientConnected(object sender, ClientConnectedEventArgs args)
    {
        Console.WriteLine($"Client connected: {args.IpPort}");
    }

    static void ForwardToMainServer(byte[] data)
    {
        string message = System.Text.Encoding.UTF8.GetString(data);
        Console.WriteLine($"Received from client, forwarding to MainServer: {message}");

        if (wsClientToMainServer != null && wsClientToMainServer.Connected)
        {
            wsClientToMainServer.Send(data);
            Console.WriteLine("Data forwarded to MainServer");
        }
        else
        {
            Console.WriteLine("Failed to forward, MainServer is not connected.");
        }
    }
}

