using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Security.Cryptography;
using System.IO;

namespace Server
{
    class Encryptor
    {
        public IPAddress ip;
        public int receive_port;
        public int send_port;

        Random rand = new Random();
        Packager pkg = new Packager();

        public int public_key;
        public int private_key;
        public int iteration_zero;
        public int swapped_key;
        public int final_key;

        public byte[] Encrypt(byte[] input, int key)
        {
            PasswordDeriveBytes pdb =
              new PasswordDeriveBytes(key.ToString(), // Change this
              new byte[] { 0x43, 0x87, 0x23, 0x72 }); // Change this
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms,
              aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();
            return ms.ToArray();
        }
        public byte[] Decrypt(byte[] input, int key)
        {
            PasswordDeriveBytes pdb =
              new PasswordDeriveBytes(key.ToString(), // Change this
              new byte[] { 0x43, 0x87, 0x23, 0x72 }); // Change this
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms,
              aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();
            return ms.ToArray();
        }

        public int keyClient()
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            private_key = rand.Next(1000);

            string[] data = receiveDataRetrans();
            public_key = int.Parse(data[0]);
            iteration_zero = public_key + private_key;

            Thread.Sleep(200);
            byte[] sendbuf = Encoding.ASCII.GetBytes(iteration_zero.ToString());
            sendDataRetrans(sendbuf);

            data = receiveDataRetrans();
            swapped_key = int.Parse(data[0]);
            final_key = swapped_key + private_key;
            return final_key;
        }

        public int keyServer()
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            private_key = rand.Next(1000);
            public_key = rand.Next(1000);

            Thread.Sleep(200);
            byte[] sendbuf = Encoding.ASCII.GetBytes(public_key.ToString());
            sendDataRetrans(sendbuf);

            iteration_zero = public_key + private_key;

            string[] data = receiveDataRetrans();
            swapped_key = int.Parse(data[0]);
            final_key = private_key + swapped_key;

            Thread.Sleep(200);
            sendbuf = Encoding.ASCII.GetBytes(iteration_zero.ToString());
            sendDataRetrans(sendbuf);
            return final_key;
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
