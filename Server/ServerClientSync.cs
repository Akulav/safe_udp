using System;
using System.Net;
using System.Text;
using System.Threading;

namespace Server
{
    class ServerClientSync
    {
        public int connected = 0;
        public IPAddress ip;
        public int receive_port;
        public int send_port;
        public int key;

        Packager pkg = new Packager();

        public void syncWithClient()
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;
            ps.key = key;
            while (true)
            {
                receiveDataRetrans();
                string input = Console.ReadLine();
                byte[] sendbuf = Encoding.ASCII.GetBytes(input);
                sendDataRetrans(sendbuf);
            }
        }

        public void syncWithServer()
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;
            ps.key = key;
            while (true)
            {
                string input = Console.ReadLine();
                Thread.Sleep(100);
                byte[] sendbuf = Encoding.ASCII.GetBytes(input);
                sendDataRetrans(sendbuf);
                receiveDataRetrans();
            }
        }

        public void sendDataRetrans(byte[] information)
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            string[] response;

            while (true)
            {
                ps.sendData(pkg.pack(information));
                response = ps.receiveData();
                if (response[0].Equals("1")) { break; }
            }

        }

        public String[] receiveDataRetrans()
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            string[] response;

            while (true)
            {
                response = ps.receiveData();
                if (pkg.verifyPackage(response[0], response[1]) == 1) { sendCode("1"); Console.WriteLine(response[0]); break; }
                else { sendCode("0"); }
            }

            return response;
        }

        public void sendCode(String code)
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;
            ps.sendData(pkg.pack(Encoding.ASCII.GetBytes(code)));
        }

    }
}