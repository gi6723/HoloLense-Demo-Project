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
            string middlemanIp = "100.79.34.6";  // IP of the MiddlemanServer (Desktop)
            int middlemanPort = 8181;            // Port the MiddlemanServer listens on

            string mainServerIp = "100.121.5.87";  // IP of the MainServer (VM)
            int mainServerPort = 8181;             // Port to forward data to

            // Open the CSV file to write incoming data
            csvWriter = new StreamWriter("eye_tracking_data.csv", append: true);
            csvWriter.WriteLine("Timestamp,Angular Velocity X,Angular Velocity Y,Angular Velocity Z,Position X,Position Y,Position Z,Right Eye Position X,Right Eye Position Y,Right Eye Position Z,Left Eye Position X,Left Eye Position Y,Left Eye Position Z,Center Eye Position X,Center Eye Position Y,Center Eye Position Z,Center Eye Rotation X,Center Eye Rotation Y,Center Eye Rotation Z,Center Eye Rotation W");  // CSV header

            // Start Middleman WebSocket Server
            wsMiddleman = new WatsonWsServer(middlemanIp, middlemanPort, false);
            wsMiddleman.ClientConnected += (sender, args) =>
            {
                Console.WriteLine("Client connected."); // Print the connection event
            };
            wsMiddleman.MessageReceived += (sender, e) => ProcessIncomingData(e.Data.ToArray());
            wsMiddleman.Start();

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
        Console.WriteLine($"Received from client: {jsonData}");

        // Forward the data to the MainServer
        ForwardToMainServer(data);

        // Convert the JSON data to CSV format and write it
        WriteToCsv(jsonData);
    }

    // Write the incoming JSON data to CSV
    static void WriteToCsv(string jsonData)
    {
        // Assuming the data follows the format of your EyeData class in JSON
        // Deserialize the JSON into an object
        var eyeData = Newtonsoft.Json.JsonConvert.DeserializeObject<EyeData>(jsonData);

        // Write data as CSV row
        string csvRow = eyeData.ToCsv();
        csvWriter.WriteLine(csvRow);
        csvWriter.Flush();
    }

    // Forward data to the MainServer
    static void ForwardToMainServer(byte[] data)
    {
        if (wsClientToMainServer != null && wsClientToMainServer.Connected)
        {
            wsClientToMainServer.SendAsync(data).Wait();  // Use SendAsync and wait for the task to complete
            Console.WriteLine("Data forwarded to MainServer");
        }
        else
        {
            Console.WriteLine("Failed to forward, MainServer is not connected.");
        }
    }
}

public class EyeData
{
    public float TimeStamp { get; set; }
    public Vector3 AngularVelocity { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 RightEyePosition { get; set; }
    public Vector3 LeftEyePosition { get; set; }
    public Vector3 CenterEyePosition { get; set; }
    public Quaternion CenterEyeRotation { get; set; }
}




