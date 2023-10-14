using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using TCPClient.Data;

namespace TCPClient
{
    internal class Program
    {
        static AppSetting appSetting = new AppSetting();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Client start!");
           await ReadConfig();
            IPAddress ipAddress = IPAddress.Parse(appSetting.IpAddress);
            var ipEndPoint = new IPEndPoint(ipAddress, appSetting.Port);
            try
            {
                using (TcpClient client = new())
                {
                    await client.ConnectAsync(ipEndPoint);
                    await using NetworkStream stream = client.GetStream();

                    var buffer = new byte[1_024];
                    int received = await stream.ReadAsync(buffer);

                    var message = Encoding.UTF8.GetString(buffer, 0, received);
                    Console.WriteLine($"Message received: \"{message}\"");
                }
            }
         catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();

        }

        static async Task ReadConfig()
        {
            string fileName = "appsettings.json";
            if (File.Exists(fileName) == true)
            {
                using FileStream openStream = File.OpenRead(fileName);
                appSetting = await JsonSerializer.DeserializeAsync<AppSetting>(openStream);
                Console.WriteLine("Config file opened!");

            }
            else
            {
                await WriteConfig();
            }

        }
        static async Task WriteConfig()
        {
            Console.WriteLine("Writing config!");
            string fileName = "appsettings.json";
            using FileStream createStream = File.Create(fileName);
            var options = new JsonSerializerOptions { WriteIndented = true };
            await JsonSerializer.SerializeAsync(createStream, appSetting, options);
            await createStream.DisposeAsync();

        }
    }
}