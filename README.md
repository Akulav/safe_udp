# safe_udp
MOSCOW VODKA PROTOCOL

__Packager - packs the data into jsons. The json contains a checksum, and the data. It is also resposible of verifying integrity.
__ProtocolStack - basic I/O (send or receive data).
ServerClientSync - Takes care of data retransmission and switches roles between client and server (one either writes or reads, not both).
Compressor - compresses the data right before sending, and decomporess it after receiving.
