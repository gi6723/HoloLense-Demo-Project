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
        static StreamWriter csvWriter = null!;
        static string csvFilePath = "eye_tracking_data.csv";

        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting Middleman Server...");

                string middlemanIp = "100.79.34.6"; // Middleman server IP
                int middlemanPort = 8282;           // Port the Middleman listens on
                string mainServerIp = "100.121.5.87";  // Main server IP
                int mainServerPort = 8181;             // Port to forward data to

                // Initialize CSV file: clear and write header
                InitializeCsv(csvFilePath);
                csvWriter = new StreamWriter(csvFilePath, append: true);

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
                    Console.WriteLine($"Message received: {message}");
                    ProcessIncomingData(e.Data.ToArray());
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
                // Close the CSV writer when the program exits
                if (csvWriter != null)
                {
                    csvWriter.Close();
                }
            }
        }

        static void InitializeCsv(string csvFilePath)
        {
            using (StreamWriter writer = new StreamWriter(csvFilePath, false))
            {
                writer.WriteLine(EyeData.CsvHeader());  // Write the header once
            }
        }

        // Process the incoming data
        static void ProcessIncomingData(byte[] data)
        {
            string jsonData = Encoding.UTF8.GetString(data);
            Console.WriteLine($"Received from client: {jsonData}");

            // Forward the data to the MainServer
            ForwardToMainServer(data);

            // Convert the JSON data to CSV format and write it
            WriteToCsv(jsonData);
        }

        // Write the incoming JSON data to CSV
        static void WriteToCsv(string jsonData)
        {
            try
            {
                var eyeData = JsonConvert.DeserializeObject<EyeData>(jsonData);

                if (eyeData != null)
                {
                    string csvRow = eyeData.ToCsv();
                    csvWriter.WriteLine(csvRow);
                    csvWriter.Flush();
                    Console.WriteLine("Data written to CSV");
                }
                else
                {
                    Console.WriteLine("Deserialized eyeData is null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to CSV: {ex.Message}");
            }
        }

        // Forward data to the MainServer
        static void ForwardToMainServer(byte[] data)
        {
            if (wsClientToMainServer != null && wsClientToMainServer.Connected)
            {
                wsClientToMainServer.SendAsync(data).Wait();
                Console.WriteLine("Data forwarded to MainServer");
            }
            else
            {
                Console.WriteLine("Failed to forward, MainServer is not connected.");
            }
        }
    }
}

