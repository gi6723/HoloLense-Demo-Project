using WatsonWebsocket;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static WatsonWsServer wsMiddleman = null!;
    static WatsonWsClient wsClientToMainServer = null!;
    static StreamWriter csvWriter;

    static async Task Main(string[] args)
    {
        try
        {
            string middlemanIp = "100.79.34.6"; // Middleman server IP
            int middlemanPort = 8181;            // Port the Middleman listens on

            string mainServerIp = "100.121.5.87";  // Main server IP
            int mainServerPort = 8181;             // Port to forward data to

            // Open the CSV file to write incoming data
            csvWriter = new StreamWriter("eye_tracking_data.csv", append: true);
            csvWriter.WriteLine(EyeData.CsvHeader());  // Write CSV header

            // Start Middleman WebSocket Server
            wsMiddleman = new WatsonWsServer(middlemanIp, middlemanPort, false);
            wsMiddleman.ClientConnected += (sender, args) =>
            {
                Console.WriteLine($"Client connected: {args.Client.IpPort}");  // Log when a client connects
            };

            wsMiddleman.MessageReceived += (sender, e) =>
            {
                string message = Encoding.UTF8.GetString(e.Data.ToArray());
                Console.WriteLine($"Message received: {message}");  // Log received data
                ProcessIncomingData(e.Data.ToArray());  // Process the received data
            };


            // Set up the connection to the MainServer
            wsClientToMainServer = new WatsonWsClient(new Uri($"ws://{mainServerIp}:{mainServerPort}"));
            wsClientToMainServer.ServerConnected += (sender, e) => Console.WriteLine("Connected to MainServer");
            wsClientToMainServer.ServerDisconnected += (sender, e) => Console.WriteLine("Disconnected from MainServer");
            await wsClientToMainServer.StartAsync();

            Console.WriteLine($"Middleman WebSocket server started on {middlemanIp}:{middlemanPort}");
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

    // Process the incoming data
    static void ProcessIncomingData(byte[] data)
    {
        
        string jsonData = Encoding.UTF8.GetString(data);
        Console.WriteLine($"Received from client: {jsonData}");  // Log received data

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
            // Deserialize the JSON into an object
            var eyeData = Newtonsoft.Json.JsonConvert.DeserializeObject<EyeData>(jsonData);

            if (eyeData != null)
            {
                // Write data as CSV row
                string csvRow = eyeData.ToCsv();
                csvWriter.WriteLine(csvRow);
                csvWriter.Flush();
                Console.WriteLine("Data written to CSV");
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
            wsClientToMainServer.SendAsync(data).Wait();  // Forward data and wait for completion
            Console.WriteLine("Data forwarded to MainServer");
        }
        else
        {
            Console.WriteLine("Failed to forward, MainServer is not connected.");
        }
    }
}

