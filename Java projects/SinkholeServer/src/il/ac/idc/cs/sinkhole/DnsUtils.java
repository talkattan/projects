package il.ac.idc.cs.sinkhole;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Random;

/**
 * This class contains several static variables and utility methods for convenience of our sinkhole server logic
 */
class DnsUtils {

    // the position of the first response flag byte in a dns response packet
    static final int FIRST_BYTE_RESPONSE_FLAG_POS = 2;

    // the position of the second response flag byte in a dns response packet
    static final int SECOND_BYTE_RESPONSE_FLAG_POS = 3;

    // The port our server is listening on
    static final int SERVER_PORT = 5300;

    // Outbound dns port
    static final int OUTBOUND_PORT = 53;

    // flag checker values
    // value to check against error code flag
    static final byte ERROR_CODE_FLAG_CHECKER = (byte) 0b00001111;

    // maximum iterative queries we should make
    static final int MAX_QUERY_ITERATIONS = 16;

    // maximum retry count for sending
    static final int MAXIMUM_SEND_RETRY_COUNT = 10;

    // A-type record indicator
    static final int A_TYPE_RESOURCE_RECORD_VALUE = 1;

    // compression pointer indicator
    static final byte COMPRESSION_POINTER_INDICATOR = (byte) 0b00111111;

    // maximum size of a dns packet, in bytes
    private static final int MAX_INCOMING_DNS_PACKET_SIZE = 512;

    /**
     * Gets a random root dns server
     *
     * @return random root dns server
     * @throws UnknownHostException if the host is unknown
     */
    static InetAddress getRandomRootDnsServer() throws UnknownHostException {
        var random = new Random();
        var rootServers = getRootDnsServers();

        return rootServers.get(random.nextInt(rootServers.size()));
    }

    /**
     * Gets an empty byte buffer with the maximum allowed size of a udp packet
     */
    static byte[] getUdpByteBuffer() {
        return new byte[MAX_INCOMING_DNS_PACKET_SIZE];
    }

    /**
     * Extracts a list of domains from a newline-separated text file
     *
     * @param path path to the file
     * @return A set of domains
     * @throws FileNotFoundException if the file was not found
     */
    static HashSet<String> getBlockedDomainsFromFile(String path) throws FileNotFoundException {

        var fileReader = new BufferedReader(new FileReader(path));

        var blockedDomains = new HashSet<String>();

        fileReader.lines()
                .filter(line -> !line.trim().isBlank() && !line.trim().isEmpty())
                .forEach(blockedDomains::add);

        return blockedDomains;
    }

    /**
     * Returns a list of all Root DNS Server
     *
     * @return list of root dns servers
     * @throws UnknownHostException if the host is unknown
     */
    private static List<InetAddress> getRootDnsServers() throws UnknownHostException {
        var rootServers = new ArrayList<InetAddress>() {
        };

        rootServers.add(InetAddress.getByName("a.root-servers.net"));
        rootServers.add(InetAddress.getByName("b.root-servers.net"));
        rootServers.add(InetAddress.getByName("c.root-servers.net"));
        rootServers.add(InetAddress.getByName("d.root-servers.net"));
        rootServers.add(InetAddress.getByName("e.root-servers.net"));
        rootServers.add(InetAddress.getByName("f.root-servers.net"));
        rootServers.add(InetAddress.getByName("g.root-servers.net"));
        rootServers.add(InetAddress.getByName("h.root-servers.net"));
        rootServers.add(InetAddress.getByName("j.root-servers.net"));
        rootServers.add(InetAddress.getByName("k.root-servers.net"));
        rootServers.add(InetAddress.getByName("l.root-servers.net"));

        return rootServers;
    }
}
