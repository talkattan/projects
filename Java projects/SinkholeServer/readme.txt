Files:

1. DnsPacket - This class represents a DNS packet and exposes several DNS methods such as response flag manipulation, getting parameters from the packet, etc.

2. DnsSinkholeServer - Interface for a DNS Sinkhole Server, exposes 1 method "void start()" for starting the server

3. DnsSinkholeServerImpl - Implementation of DnsSinkholeServer interface, this is the code that is responsible for managing DNS requests between the client and the authority/root servers.

4. DnsUtils - Contains several static variables and utility methods for convenience of our sinkhole server logic

5. SinkholeServer - Main program endpoint