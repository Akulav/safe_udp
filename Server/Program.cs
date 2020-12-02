using System.Net;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Encryptor ec = new Encryptor();
            ec.ip = IPAddress.Parse("192.168.10.100");
            ec.send_port = 11001;
            ec.receive_port = 11000;
            int key = ec.keyClient();

            ServerClientSync sc = new ServerClientSync();
            sc.ip = IPAddress.Parse("192.168.10.100");
            sc.send_port = 11001;
            sc.receive_port = 11000;
            sc.syncWithServer();
        }
    }
}
