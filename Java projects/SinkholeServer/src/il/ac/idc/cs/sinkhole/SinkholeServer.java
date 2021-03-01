package il.ac.idc.cs.sinkhole;

import java.io.FileNotFoundException;
import java.util.HashSet;

/**
 * Main class
 */
public class SinkholeServer {

    /**
     * Main program endpoint
     *
     * @param args - can either be empty or contain 1 argument: a path to a file that contains a list of domains to block
     */
    public static void main(String[] args) {

        var blockedDomains = new HashSet<String>();

        // get blocked domains
        try {
            if (args.length > 0) {
                blockedDomains = DnsUtils.getBlockedDomainsFromFile(args[0]);
            }
        } catch (FileNotFoundException f) {
            System.err.println("Error: could not file in specified path: " + args[0]);
        }

        // initialize the server object
        var dnsSinkholeServer = new DnsSinkholeServerImpl(blockedDomains, DnsUtils.SERVER_PORT);

        // start the dns sinkhole server
        dnsSinkholeServer.start();
    }
}
