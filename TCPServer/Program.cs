using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using TCPServer.Data;

namespace TCPServer
{
    internal class Program
    {
        static AppSetting appSetting=new AppSetting();
        static async Task Main(string[] args)
        {
           await ReadConfig();
            Console.WriteLine("Welcome to the server!");
            IPAddress ipAddress = IPAddress.Parse(appSetting.IpAddress);

            var ipEndPoint = new IPEndPoint(ipAddress, appSetting.Port);
            TcpListener listener = new(ipEndPoint);

            try
            {
                listener.Start();

                using (TcpClient handler = await listener.AcceptTcpClientAsync())
                {
                    string clientIP = ((IPEndPoint)handler.Client.RemoteEndPoint).Address.ToString();
                    Console.WriteLine($"Connected! Client IP: {clientIP}");

                    await using NetworkStream stream = handler.GetStream();

                    var message = $"📅 {DateTime.Now} 🕛";
                    var dateTimeBytes = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(dateTimeBytes);

                    Console.WriteLine($"Sent message: \"{message}\"");
                    Console.WriteLine("write message");
                    message=Console.ReadLine();
                    dateTimeBytes = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(dateTimeBytes);
                    // Sample output:
                    //     Sent message: "📅 8/22/2022 9:07:17 AM 🕛"
                }
                Console.ReadLine();


            }
            finally
            {
                listener.Stop();
            }

        }
      static  async Task ReadConfig()
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