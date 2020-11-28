using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODKA_MOSCOW_PROTOCOL
{
    class Packager
    {

        public byte[] pack(byte[] data)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] checksum = sha1.ComputeHash(data, 0, data.Length);
            dataTree dataRaw = new dataTree();
            
            dataRaw.data = data;
            dataRaw.checksum = checksum;
            string jsonStr = JsonConvert.SerializeObject(dataRaw);
            
            return Encoding.ASCII.GetBytes(jsonStr);
        }

        public string[] unpack(string json)
        {
            dynamic unpacked = JsonConvert.DeserializeObject(json);
            string[] result = { unpacked.data, unpacked.checksum };
            return result;
        }
    }

    class dataTree
    {
        public byte[] data;
        public byte[] checksum;
    }
}
