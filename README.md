# safe_udp
MOSCOW VODKA PROTOCOL\
\
Packager - packs the data into jsons. The json contains a checksum, and the data. It is also resposible of verifying integrity.\
ProtocolStack - basic I/O (send or receive data).\
ServerClientSync - Takes care of data retransmission and switches roles between client and server (one either writes or reads, not both).\
Compressor - compresses the data right before sending, and decomporess it after receiving.\
FTP - works, but not reliable, maybe I will make it work...\
Encryptor - Ready, but not deployed as of 12/2/2020 16:56:00
