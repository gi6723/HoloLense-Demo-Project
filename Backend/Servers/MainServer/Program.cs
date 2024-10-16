using WatsonWebsocket;
using System;

class Program
{
    static WatsonWsServer wsServer;

    static void Main(string[] args)
    {
        string serverIp = "100.121.5.87";  // IP of the MainServer (VM)
        int serverPort = 8181;             // Port the MainServer will listen on

        // Start the WebSocket server
        wsServer = new WatsonWsServer(serverIp, serverPort, false);
        wsServer.ClientConnected += ClientConnected;
        wsServer.MessageReceived += MessageReceived;
        wsServer.Start();

        Console.WriteLine($"Main WebSocket server started on {serverIp}:{serverPort}");
        Console.ReadLine();  // Keep the server running
    }

    static void ClientConnected(object sender, ClientConnectedEventArgs args)
    {
        Console.WriteLine($"Client connected: {args.IpPort}");
    }

    static void MessageReceived(object sender, MessageReceivedEventArgs args)
    {
        string receivedData = System.Text.Encoding.UTF8.GetString(args.Data);
        Console.WriteLine($"Data received from Middleman: {receivedData}");
    }
}

