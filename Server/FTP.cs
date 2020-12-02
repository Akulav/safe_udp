using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class FTP
    {
        public IPAddress ip;
        public int receive_port;
        public int send_port;

        public string[] buffer;

        public String path = @"YOUR FILE PATH";
        FileStream file = new FileStream("YOUR FILE PATH", FileMode.Open, FileAccess.Read, FileShare.Read);
        Packager pkg = new Packager();
        byte[] information = File.ReadAllBytes("YOUR FILE PATH");

        public byte[] ReadFully(Stream stream)
        {
            Packager pkg = new Packager();
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            byte[] buffer = new byte[1024]; //set the size of your buffer (chunk)
            using (MemoryStream ms = new MemoryStream()) //You need a db connection instead
            {
                while (true) //loop to the end of the file
                {
                    int read = stream.Read(buffer, 0, buffer.Length); //read each chunk
                    if (read <= 0)
                    {
                        return ms.ToArray();
                    }
                    ms.Write(buffer, 0, read); //write chunk to [wherever]
                    ms.Flush();
                    while (true)
                    {
                        Thread.Sleep(500);
                        ps.sendData(pkg.pack(buffer));
                        string buff = ps.receiveData()[0];
                        if (buff.Equals("1")) { break; }
                      
                    }
                }

                
            }

            ps.sendData(pkg.pack(Encoding.ASCII.GetBytes("EOF")));

        }

        public void getFile()
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;
            while (true)
            {
                while (true)
                {
                    buffer = ps.receiveData();
                    sendAKG(pkg.verifyPackage(buffer[0], buffer[1]));
                    Console.WriteLine(pkg.verifyPackage(buffer[0], buffer[1]));
                    if (pkg.verifyPackage(buffer[0], buffer[1]) == 1) { break; }
                }


                byte[] bytes = Encoding.ASCII.GetBytes(buffer[0]);
                using (var stream = new FileStream("C:\\A\\PBL_presentations.txt", FileMode.Append))
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                if (Encoding.ASCII.GetString(bytes).Equals("EOF"))
                { break; }

            }
        }

        public void sendFile()
        {

        }

        public void getRequest()
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            buffer = ps.receiveData();

            if (buffer[0].Equals("FILE"))
            {
                //MemoryStream stream = new MemoryStream(information);
                ReadFully(file);
            }

            Console.WriteLine(buffer[0]);
        }

        public void sendAKG(int code)
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            if (code == 0)
            {
                ps.sendData(pkg.pack(Encoding.ASCII.GetBytes("0")));
            }

            if (code == 1)
            {
                ps.sendData(pkg.pack(Encoding.ASCII.GetBytes("1")));
            }
        }
    }
}
