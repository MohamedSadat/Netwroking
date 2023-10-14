using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Data
{
    internal class AppSetting
    {
        public string IpAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 5000;
    }
}
