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

        public string Encrypt(byte[] data, int key)
        {
            try
            {
                string textToEncrypt = Encoding.ASCII.GetString(data);
                string ToReturn = "";
                string publickey = "control_string";
                string secretkey = key.ToString();
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public string Decrypt(string data,int key)
        {
            try
            {
                string textToDecrypt = data;
                string ToReturn = "";
                string publickey = "control_string";
                string privatekey = key.ToString();
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(privatekey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
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

            Console.WriteLine(final_key);
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

            Console.WriteLine(final_key);
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
