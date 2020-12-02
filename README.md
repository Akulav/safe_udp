# A better UDP, rebranded as VODKA PROTOCOL
Features: error checking, retransmission, encryption, data compression.

# Protocol Stack
Has only two functions. It can send and receive data.

# Compressor
Compresses and decompresses data (lossless)

# Encryptor
Reliably makes a handshake between the server and the client to agree on an encryption key. Diffie-Hellman style.

# Packager
Calculates a checksum for the data and then packs them into a json. It also calls the compressor and encryptor after the json is created. It has also the function of checking the checksum for data integrity.

# ServerClientSync
This does most of the work. It is basically the controller for the entire data exchange. It switches both the client and server between two states: write / read. It also makes sure that all data arrives reliably. If connection is lost, a easy to modify timeout variable can change the maximum number of data retransmission.

# FTP 
It works but, very slow. It separates a large file in however large chunks you want, and send them. Retransmission ensures packet safety. (for some reason there are a lot of packet loss, therefore it is very slow because of a large number of retransmissions. (It is there, but let's say it is only half baked, ignore it).

# Program Class

Default main class. 
```
using System.Net;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //If encryptor is not used, the data will be encrypted with a hardcoded value
            Encryptor ec = new Encryptor();
            ec.ip = IPAddress.Parse("YOUR IP");
            ec.send_port = ABCD;
            ec.receive_port = ABCE;
            int key = ec.keyServer();

            //The server and client should use reverse ports since the roles of listening/writing are also reversed.
  
            ServerClientSync sc = new ServerClientSync();
            sc.ip = IPAddress.Parse("YOUR IP");
            sc.send_port = ABCD;
            sc.receive_port = ABCE;
            sc.key = key;
            //If you are the server use - sc.syncWithClient();
            //If you are the client use - sc.syncWithServer();
        }
    }
}
```




