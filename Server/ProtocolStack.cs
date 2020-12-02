using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class ProtocolStack
    {
        public IPAddress ip;
        public int receive_port;
        public int send_port;
        public int key = 1234567890; //default key, used only if another key was not generated.
        Packager pkg = new Packager();
        Encryptor ec = new Encryptor();

        public String sendData(byte[] buffer)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ep = new IPEndPoint(ip, send_port);
            s.SendTo(ec.Encrypt(buffer, key), ep);
            s.Close();
            return Encoding.ASCII.GetString(buffer);
        }

        public string[] receiveData()
        {
            UdpClient listener = new UdpClient(receive_port);
            IPEndPoint groupEP = new IPEndPoint(ip, receive_port);
            byte[] bytes = listener.Receive(ref groupEP);
            bytes = ec.Decrypt(bytes, key);
            listener.Close();
            string data = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            string[] output = pkg.unpack(data);
            var base64EncodedBytes = Convert.FromBase64String(pkg.unpack(data)[0]);
            output[1] = pkg.unpack(data)[1];
            output[0] = Encoding.UTF8.GetString(base64EncodedBytes);
            Console.WriteLine($"Received broadcast from {groupEP} :");
            listener.Close();
            return output;
        }

    }

}
