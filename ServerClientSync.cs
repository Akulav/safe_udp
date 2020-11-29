using System;
using System.Net;
using System.Text;
using System.Threading;

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
            FTP ftp = new FTP();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;
            ftp.ip = ip;
            ftp.receive_port = receive_port;
            ftp.send_port = send_port;
            while (true)
            {
                ftp.getRequest();
                string input = Console.ReadLine();;
                byte[] sendbuf = Encoding.ASCII.GetBytes(input);
                ps.sendData(pkg.pack(sendbuf));
            }
        }

        public void syncWithServer()
        {
            ProtocolStack ps = new ProtocolStack();
            FTP ftp = new FTP();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;
            ftp.ip = ip;
            ftp.receive_port = receive_port;
            ftp.send_port = send_port;
            while (true)
            {
                string input = Console.ReadLine();
                byte[] sendbuf = Encoding.ASCII.GetBytes(input);
                ps.sendData(pkg.pack(sendbuf));
                Thread.Sleep(100);
                ftp.getRequest();
            }
        }

    }
}
