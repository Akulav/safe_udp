using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace VODKA_MOSCOW_PROTOCOL
{
    class ProtocolStack
    {
        public IPAddress ip;
        public int receive_port;
        public int send_port;
        Packager pkg = new Packager();

        public void sendData(byte[] buffer)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ep = new IPEndPoint(ip, send_port);
            s.SendTo(buffer, ep);
            s.Close();
        }

        public string receiveData()
        {
            UdpClient listener = new UdpClient(receive_port);
            IPEndPoint groupEP = new IPEndPoint(ip, receive_port);
            byte[] bytes = listener.Receive(ref groupEP);
            listener.Close();
            string data = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            Console.WriteLine($"Received broadcast from {groupEP} :");
            var base64EncodedBytes = System.Convert.FromBase64String(pkg.unpack(data)[0]);
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(base64EncodedBytes));
            return data;
        }

    }

}
