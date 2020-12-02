using Newtonsoft.Json;
using System;
using System.Text;

namespace Server
{
    class Packager
    {
        Compressor zip = new Compressor();

        public byte[] pack(byte[] data)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] checksum = sha1.ComputeHash(data, 0, data.Length);
            dataTree dataRaw = new dataTree();

            dataRaw.data = data;
            dataRaw.checksum = BitConverter.ToString(checksum).Replace("-", "");
            string jsonStr = zip.CompressString(JsonConvert.SerializeObject(dataRaw));



            return Encoding.ASCII.GetBytes(jsonStr);
        }

        public string[] unpack(string json)
        {
            string unzip = zip.DecompressString(json);
            dynamic unpacked = JsonConvert.DeserializeObject(unzip);
            string[] result = { unpacked.data, unpacked.checksum };
            return result;
        }

        public int verifyPackage(string data, string checksum)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] buf = Encoding.UTF8.GetBytes(data);
            byte[] hash = sha1.ComputeHash(buf, 0, buf.Length);
            string newChecksum = System.BitConverter.ToString(hash).Replace("-", "");

            if (newChecksum.Equals(checksum))
            {
                return 1;
            }
            else
                return 0;
        }
    }

    class dataTree
    {
        public byte[] data;
        public string checksum;
    }
}
