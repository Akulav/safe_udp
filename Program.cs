using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace VODKA_MOSCOW_PROTOCOL
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerClientSync sc = new ServerClientSync();
            sc.ip = IPAddress.Parse("192.168.10.110");
            sc.send_port = 11001;
            sc.receive_port = 11000;
            sc.syncWithServer();
        }
    }
}
