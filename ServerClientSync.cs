using System;
using System.Net;
using System.Text;

namespace VODKA_MOSCOW_PROTOCOL
{
    class ServerClientSync
    {
        public int connected = 0;
        public IPAddress ip;
        public int receive_port;
        public int send_port;
        public string[] buffer;
        Packager pkg = new Packager();

        public void syncWithClient()
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            while (true)
            {
                buffer = ps.receiveData();
                Console.WriteLine(buffer[0]);
                Console.WriteLine(pkg.verifyPackage(buffer[0], buffer[1]));
                string input = Console.ReadLine();
                byte[] sendbuf = Encoding.ASCII.GetBytes(input);
                ps.sendData(pkg.pack(sendbuf));
            }
        }

        public void syncWithServer()
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            while (true)
            {
                string input = Console.ReadLine();
                byte[] sendbuf = Encoding.ASCII.GetBytes(input);
                ps.sendData(pkg.pack(sendbuf));
                buffer = ps.receiveData();
                Console.WriteLine(buffer[0]);
                Console.WriteLine(pkg.verifyPackage(buffer[0], buffer[1]));
            }
        }
    }
}
