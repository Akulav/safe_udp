using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ProtocolStack
    {
        public IPAddress ip;
        public int receive_port;
        public int send_port;

        public void sendData(byte[] buffer)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            IPEndPoint ep = new IPEndPoint(ip, send_port);
            s.SendTo(buffer, ep);
            s.Close();
        }

        public void receiveData()
        {
            UdpClient listener = new UdpClient(receive_port);
            listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            IPEndPoint groupEP = new IPEndPoint(ip, receive_port);
         
                Console.WriteLine("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);

           
                    Console.WriteLine($"Received broadcast from {groupEP} :");
                    Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
            listener.Close();
        }

    }
           
}
