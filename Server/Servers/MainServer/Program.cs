using System;
using WatsonWebsocket;

namespace MainServer 
{
    public class Program
    {
        private static WatsonWSServer wsServer;

        static void Main(string[] args)
        {
            string serverIp = "100.66.109.48";
            int serverPort = 8080;
            
            wsServer = new WatsonWsServer(serverIp, serverPort, false);
            wsServer.ClientConnected += ClientConnected;
            wsServer.MessageRecieved += MessageReceived;
            wsServer.Start();
            
            Console.WriteLine($"WebSocket server started on {serverIp}:{serverPort}");
            Console.ReadLine();
        }

        static void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
            Console.WriteLine($"Client connected: {args.IpPort}")
        }
    }
    
    static void MessageReceived(object sender, MessageReceivedEventArgs args)
    {
        string receivedData = System.Text.Encoding.UTF8.GetString(args.Data);
        Console.WriteLine($"Data received: {receivedData}");
    }
}

