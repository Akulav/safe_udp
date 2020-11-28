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
            sc.ip = IPAddress.Parse("YOUR_IP");
            sc.send_port = SEND_PORT;
            sc.receive_port = RECEIVE_PORT;
            //sc.syncWithServer(); or sc.syncWithClient(); Depending on the role.
        }
    }
}
