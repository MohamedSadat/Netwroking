using System.Net.NetworkInformation;

namespace NetwrokStats
{
    internal class Program
    {
  
        static async Task Main(string[] args)
        {
            ShowStatistics(NetworkInterfaceComponent.IPv4);
            //  ShowStatistics(NetworkInterfaceComponent.IPv6);
           await CheckReachability();
            Console.ReadLine();
        }
        static async Task CheckReachability()
        {
            var ping = new Ping();
            try
            {
                string hostName = "kitchinohrro.ddns.net";
                PingReply reply = await ping.SendPingAsync(hostName);
                Console.WriteLine($"Ping status for ({hostName}): {reply.Status}");
                if (reply is { Status: IPStatus.Success })
                {
                    Console.WriteLine($"Address: {reply.Address}");
                    Console.WriteLine($"Roundtrip time: {reply.RoundtripTime}");
                    Console.WriteLine($"Time to live: {reply.Options?.Ttl}");
                    Console.WriteLine();
                }
                else if (reply is { Status: IPStatus.TimedOut })
                {
                    Console.WriteLine($"Timed out");
                }
                else if(reply is {Status:IPStatus.DestinationNetworkUnreachable })
                {
                    Console.WriteLine($"unreachable");

                }
            }
            catch (PingException ex)
            {
                Console.WriteLine(ex.Message);
            }
      
        }
        static void ShowStatistics(NetworkInterfaceComponent version)
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            var stats = version switch
            {
                NetworkInterfaceComponent.IPv4 => properties.GetTcpIPv4Statistics(),
                _ => properties.GetTcpIPv6Statistics()
            };

            Console.WriteLine($"TCP/{version} Statistics");
            Console.WriteLine($"  Minimum Transmission Timeout : {stats.MinimumTransmissionTimeout:#,#}");
            Console.WriteLine($"  Maximum Transmission Timeout : {stats.MaximumTransmissionTimeout:#,#}");
            Console.WriteLine("  Connection Data");
            Console.WriteLine($"      Current :                  {stats.CurrentConnections:#,#}");
            Console.WriteLine($"      Cumulative :               {stats.CumulativeConnections:#,#}");
            Console.WriteLine($"      Initiated  :               {stats.ConnectionsInitiated:#,#}");
            Console.WriteLine($"      Accepted :                 {stats.ConnectionsAccepted:#,#}");
            Console.WriteLine($"      Failed Attempts :          {stats.FailedConnectionAttempts:#,#}");
            Console.WriteLine($"      Reset :                    {stats.ResetConnections:#,#}");
            Console.WriteLine("  Segment Data");
            Console.WriteLine($"      Received :                 {stats.SegmentsReceived:#,#}");
            Console.WriteLine($"      Sent :                     {stats.SegmentsSent:#,#}");
            Console.WriteLine($"      Retransmitted :            {stats.SegmentsResent:#,#}");
            Console.WriteLine();
        }
    }
}