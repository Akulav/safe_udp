using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VODKA_MOSCOW_PROTOCOL
{
    class FTP
    {
        public IPAddress ip;
        public int receive_port;
        public int send_port;

        public string[] buffer;

        public String path = @"C:\PBL_presentation.pdf";
        FileStream file = new FileStream("C:\\A\\PBL_presentation.pdf", FileMode.Open, FileAccess.Read, FileShare.Read);
        Packager pkg = new Packager();
        public byte[] ReadFully(Stream stream)
        {
            Packager pkg = new Packager();
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            byte[] buffer = new byte[8192]; //set the size of your buffer (chunk)
            using (MemoryStream ms = new MemoryStream()) //You need a db connection instead
            {
                while (true) //loop to the end of the file
                {
                    int read = stream.Read(buffer, 0, buffer.Length); //read each chunk
                    if (read <= 0) //check for end of file
                        return ms.ToArray();
                    ms.Write(buffer, 0, read); //write chunk to [wherever]
                    Thread.Sleep(10);
                    ps.sendData(pkg.pack(buffer));
                }
            }
        }

        public void ConnectToServer()
        {

        }
        public void ConnectToClient()
        {

        }

        public void getFile()
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            buffer = ps.receiveData();
            Console.WriteLine(pkg.verifyPackage(buffer[0], buffer[1]));
            byte[] bytes = Encoding.ASCII.GetBytes(buffer[0]);
            using (var stream = new FileStream("C:\\A\\PBL_presentations.pdf", FileMode.Append))
            {
                stream.Write(bytes, 0, bytes.Length);
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
                ReadFully(file);
            }

            Console.WriteLine(buffer[0]);
        }
    }
}
