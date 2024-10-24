using WatsonWebsocket;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;  // Ensure this is included

namespace DefaultNamespace
{
    class Program
    {
        static WatsonWsServer wsMiddleman = null!;
        static WatsonWsClient wsClientToMainServer = null!;
        static StreamWriter eyeCsvWriter = null!;
        static StreamWriter eventCsvWriter = null!;
        static string eyeCsvFilePath = "eye_tracking_data.csv";
        static string eventCsvFilePath = "game_events.csv";

        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting Middleman Server...");

                string middlemanIp = "100.79.34.6"; // Middleman server IP
                int middlemanPort = 8282;           // Port the Middleman listens on
                string mainServerIp = "100.121.5.87";  // Main server IP
                int mainServerPort = 8181;             // Port to forward data to

                // Initialize CSV files: clear and write headers
                InitializeEyeCsv(eyeCsvFilePath);
                eyeCsvWriter = new StreamWriter(eyeCsvFilePath, append: true);

                InitializeEventCsv(eventCsvFilePath);
                eventCsvWriter = new StreamWriter(eventCsvFilePath, append: true);

                // Start Middleman WebSocket Server
                wsMiddleman = new WatsonWsServer(middlemanIp, middlemanPort, false);
                wsMiddleman.Start();

                Console.WriteLine($"Middleman Server started on {middlemanIp}:{middlemanPort}");

                wsMiddleman.ClientConnected += (sender, args) =>
                {
                    Console.WriteLine($"Client connected: {args.Client.IpPort}");
                };

                wsMiddleman.MessageReceived += (sender, e) =>
                {
                    string message = Encoding.UTF8.GetString(e.Data.ToArray());
                    //Console.WriteLine($"Message received: {message}");
                    ProcessIncomingData(message);
                };

                // Set up the connection to the MainServer
                wsClientToMainServer = new WatsonWsClient(new Uri($"ws://{mainServerIp}:{mainServerPort}"));
                wsClientToMainServer.ServerConnected += (sender, e) => Console.WriteLine("Connected to MainServer");
                wsClientToMainServer.ServerDisconnected += (sender, e) => Console.WriteLine("Disconnected from MainServer");
                await wsClientToMainServer.StartAsync();

                Console.ReadLine();  // Keep the server running
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
            finally
            {
                // Close the CSV writers when the program exits
                eyeCsvWriter?.Close();
                eventCsvWriter?.Close();
            }
        }

        static void InitializeEyeCsv(string csvFilePath)
        {
            using (StreamWriter writer = new StreamWriter(csvFilePath, false))
            {
                writer.WriteLine(EyeData.CsvHeader());  // Write the header once
            }
        }

        static void InitializeEventCsv(string csvFilePath)
        {
            using (StreamWriter writer = new StreamWriter(csvFilePath, false))
            {
                writer.WriteLine("EventType,Timestamp,Details");  // Write the header
            }
        }

        // Process the incoming data
        static void ProcessIncomingData(string jsonData)
        {
            try
            {
                var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(jsonData);
                if (baseMessage == null || string.IsNullOrEmpty(baseMessage.type))
                {
                    Console.WriteLine("Invalid message format: missing type.");
                    return;
                }

                if (baseMessage.data == null)
                {
                    Console.WriteLine("Invalid message format: missing data.");
                    return;
                }

                string dataAsString = baseMessage.data?.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(dataAsString))
                {
                    Console.WriteLine("Data is null or empty.");
                    return;
                }

                switch (baseMessage.type)
                {
                    case "EyeData":
                        var eyeData = JsonConvert.DeserializeObject<EyeData>(dataAsString);
                        if (eyeData != null && !float.IsNaN(eyeData.AngularVelocity[0]))
                        {
                            string eyeCsvRow = eyeData.ToCsv();
                            eyeCsvWriter.WriteLine(eyeCsvRow);
                            eyeCsvWriter.Flush();
                            Console.WriteLine("Eye-tracking data written to CSV.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid or incomplete eye-tracking data.");
                        }
                        break;

                    case "GameEventData":
                        var eventData = JsonConvert.DeserializeObject<GameEventData>(dataAsString);
                        if (eventData != null)
                        {
                            string eventCsvRow = $"{eventData.EventType},{eventData.Timestamp},{eventData.Details}";
                            eventCsvWriter.WriteLine(eventCsvRow);
                            eventCsvWriter.Flush();
                            Console.WriteLine("Game event data written to CSV.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to deserialize game event data.");
                        }
                        break;

                    default:
                        Console.WriteLine($"Unknown message type: {baseMessage.type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing incoming data: {ex.Message}");
            }
        }





        // Forward data to the MainServer
        static void ForwardToMainServer(byte[] data)
        {
            if (wsClientToMainServer != null && wsClientToMainServer.Connected)
            {
                wsClientToMainServer.SendAsync(data).Wait();
                //Console.WriteLine("Data forwarded to MainServer");
            }
            else
            {
                Console.WriteLine("Failed to forward, MainServer is not connected.");
            }
        }
    }

    // Base message structure to determine the message type
    public class BaseMessage
    {
        public string? type { get; set; }
        public object? data { get; set; }
    } 
}
