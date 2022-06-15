
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using PandaTcpExam.Server;

public class Server
{
    public static async Task Main()
    {
        await RunAsync();
    }

    static async Task RunAsync()
    {
        var listener = new TcpListener(IPAddress.Any, 50000);
        Console.WriteLine("Server started: ");
        listener.Start();
        try
        {
            while (true)
                Accept(await listener.AcceptTcpClientAsync());
        }
        finally
        {
            listener.Stop();
        }
    }

    static async Task Accept(TcpClient client)
    {
        await Task.Yield();
        try
        {
            using (client)
            await using (NetworkStream n = client.GetStream())
            await using (StreamWriter writer = new StreamWriter(n))
            {
                var dataGenerator = new RandomDataGenerator();
                foreach (var data in dataGenerator.GetData())
                {
                    data.TickTime = DateTime.Now.Ticks;
                    var json = JsonConvert.SerializeObject(data);
                    await writer.WriteLineAsync(json);
                    Console.WriteLine($"Data Sent: {json}");
                    await Task.Delay(50); // Adds to the effect of the simulation
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}