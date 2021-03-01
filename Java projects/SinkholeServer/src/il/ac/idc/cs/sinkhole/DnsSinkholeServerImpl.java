package il.ac.idc.cs.sinkhole;

import java.io.IOException;
import java.net.*;
import java.util.HashSet;

/**
 * Implements DnsSinkholeServer
 */
public class DnsSinkholeServerImpl implements DnsSinkholeServer {

    private final HashSet<String> blockedDomains;
    private final int serverPort;
    private DatagramSocket sinkholeServer;
    private DnsPacket originalDnsRequestReceived;
    private int originalDnsRequestLength;
    private int currentDnsPacketLength;

    /**
     * Creates a new DnsSinkholdServerImpl object
     *
     * @param blockedDomains list of blocked domains
     * @param serverPort     port in which to run the server on
     */
    DnsSinkholeServerImpl(HashSet<String> blockedDomains, int serverPort) {

        this.blockedDomains = blockedDomains;
        this.serverPort = serverPort;
    }

    /**
     * Start accepting dns requests
     */
    @Override
    public void start() {

        System.out.println("Starting sinkhole server on port " + serverPort);

        while (true) {

            DatagramPacket originalDnsRequestUdpPacket;
            try {
                // create new socket and listen on server port
                sinkholeServer = new DatagramSocket(serverPort);

                // allocate a byte buffer for the next dns request
                byte[] originalDnsRequestPacketBuffer = DnsUtils.getUdpByteBuffer();

                // originalDnsRequestUdpPacket is the packet containing DNS query we received from the client
                originalDnsRequestUdpPacket = new DatagramPacket(originalDnsRequestPacketBuffer, originalDnsRequestPacketBuffer.length);

                // wait until we receive packet from client
                sinkholeServer.receive(originalDnsRequestUdpPacket);

                // save address and port we received from the request
                var clientAddress = originalDnsRequestUdpPacket.getAddress();
                var clientPort = originalDnsRequestUdpPacket.getPort();


                // update the original dns request packet length
                originalDnsRequestLength = originalDnsRequestUdpPacket.getLength();

                // we maintain this class member in order to determine the current dns packet length, as to not send oversized/undersized dns packets
                currentDnsPacketLength = originalDnsRequestLength;

                // get queried domain to check if in block list
                originalDnsRequestReceived = DnsPacket.CreateFromUdpPacket(originalDnsRequestUdpPacket);

                System.out.println("A request was received from: " + clientAddress + ":" + clientPort + ". Domain requested: " + originalDnsRequestReceived.getDomainQueried());

                // check if the domain or root server in the request is in the block list
                if (doesPacketContainBlockedDomain(originalDnsRequestReceived)) {

                    var blockedDomainRawUdpDnsPacket = getBlockedDomainRawUdpDnsPacket();
                    sendFinalResponseAndCloseConnection(blockedDomainRawUdpDnsPacket);

                    continue;
                }

                // send first query to root server
                var currentDnsPacket = sendQueryToRootServer();

                // flag that indicates if we should send a DNS response to client or whether we already sent it
                var shouldSendResponseToClient = true;

                var numberOfIterations = 0;
                // iteratively query servers until we receive an answer, an error, or max iterations passed
                while (currentDnsPacket.isValidDnsQuery()
                        && currentDnsPacket.getNumberOfAnswers() == 0
                        && currentDnsPacket.getNumberOfAuthorities() > 0
                        && numberOfIterations < DnsUtils.MAX_QUERY_ITERATIONS) {

                    // check if the domain or authority server in the request is in the block list
                    if (doesPacketContainBlockedDomain(currentDnsPacket)) {

                        shouldSendResponseToClient = false;
                        var blockedDomainRawUdpDnsPacket = getBlockedDomainRawUdpDnsPacket();
                        sendFinalResponseAndCloseConnection(blockedDomainRawUdpDnsPacket);

                        break;
                    }

                    // get new authority server address to query
                    var authorityServerAddress = currentDnsPacket.getAuthorityServer();

                    // create DNS query udp packet to send to the newly discovered authority server
                    var clientDnsQueryData = originalDnsRequestUdpPacket.getData();

                    var authorityServer = InetAddress.getByName(authorityServerAddress);

                    var outboundDnsUdpPacket = new DatagramPacket(clientDnsQueryData, originalDnsRequestUdpPacket.getLength(), authorityServer, DnsUtils.OUTBOUND_PORT);

                    System.out.println("Querying next authority server: " + authorityServerAddress);

                    // send to next authority server
                    var incomingDnsUdpPacket = sendPacketAndReceiveResponse(outboundDnsUdpPacket);

                    // parse the response
                    currentDnsPacket = DnsPacket.CreateFromUdpPacket(incomingDnsUdpPacket);

                    // update packet length - required for trimming the packet later
                    currentDnsPacketLength = incomingDnsUdpPacket.getLength();

                    numberOfIterations++;
                }

                // check final time if domain is in block list (and check if we have yet to send a response)
                if (shouldSendResponseToClient && doesPacketContainBlockedDomain(currentDnsPacket)) {
                    var blockedDomainRawUdpDnsPacket = getBlockedDomainRawUdpDnsPacket();
                    sendFinalResponseAndCloseConnection(blockedDomainRawUdpDnsPacket);

                    continue;
                }

                if (shouldSendResponseToClient) {
                    // if we got a valid answer - fix response flags
                    currentDnsPacket.setValidResponseFlags();


                    // get the raw packet from the DnsPacket object
                    var responsePacket = currentDnsPacket.rawUdpPacket;

                    // set the client address and port
                    responsePacket.setAddress(clientAddress);
                    responsePacket.setPort(clientPort);

                    // trim the packet so we won't send redundant data
                    responsePacket.setLength(currentDnsPacketLength);

                    sendFinalResponseAndCloseConnection(responsePacket);
                }
            } catch (IOException | IllegalArgumentException e) {
                System.err.println("A Socket or DNS Parsing error was caught. Message: " + e.getMessage());
            } finally {
                if (sinkholeServer != null && !sinkholeServer.isClosed()) {
                    // dispose of resources gracefully
                    originalDnsRequestUdpPacket = null;
                    sinkholeServer.close();
                    sinkholeServer = null;
                }
            }
        }
    }

    /**
     * Sends a response to the client and closes the socket connection
     *
     * @param finalPacketToSend the packet to send to the client
     * @throws IOException from either IO or Socket failures
     */
    private void sendFinalResponseAndCloseConnection(DatagramPacket finalPacketToSend) throws IOException {

        System.out.println("Sending response to client");
        sinkholeServer.send(finalPacketToSend);

        if (!sinkholeServer.isClosed()) {
            sinkholeServer.close();
        }
        System.out.println("Response sent");
    }

    /**
     * Checks whether the dns packet contains either a domain, server to query or answer in the blocked list
     *
     * @param receivedDnsPacket the dns packet containing the domains
     * @return true if contains blocked, false otherwise
     */
    private boolean doesPacketContainBlockedDomain(DnsPacket receivedDnsPacket) {
        return blockedDomains.contains(receivedDnsPacket.getDomainQueried())
                || blockedDomains.contains(receivedDnsPacket.getAuthorityServer())
                || blockedDomains.contains(receivedDnsPacket.getAnswer());
    }

    /**
     * Get the udp packet representing a response with "NXDomain" error and the original queried domain
     */
    private DatagramPacket getBlockedDomainRawUdpDnsPacket() {

        System.out.println("A domain name or address was found in the block list. Preparing response to client");

        // create a new dns packet, not overriding the original one we are saving
        var blockedDomainDnsPacket = DnsPacket.CreateFromUdpPacket(originalDnsRequestReceived.rawUdpPacket);

        blockedDomainDnsPacket.setBlockedDomainError();
        blockedDomainDnsPacket.setValidResponseFlags();

        var blockedDomainRawUdpPacket = originalDnsRequestReceived.rawUdpPacket;
        blockedDomainRawUdpPacket.setLength(originalDnsRequestLength);

        return blockedDomainRawUdpPacket;
    }

    /**
     * Sends a udp packet and waits for a response from the server
     *
     * @param outboundDnsUdpPacket the udp packet to send
     * @return the received udp packet
     * @throws IOException from either IO or Socket failures
     */
    private DatagramPacket sendPacketAndReceiveResponse(DatagramPacket outboundDnsUdpPacket) throws IOException {
        byte[] incomingData = DnsUtils.getUdpByteBuffer();

        var dnsServerSocket = new DatagramSocket();

        dnsServerSocket.setSoTimeout(5000);

        DatagramPacket incomingDnsUdpPacket = null;
        int numOfRetries = 0;

        while (numOfRetries < DnsUtils.MAXIMUM_SEND_RETRY_COUNT) {

            try {

                incomingDnsUdpPacket = new DatagramPacket(incomingData, incomingData.length);

                dnsServerSocket.send(outboundDnsUdpPacket);

                dnsServerSocket.receive(incomingDnsUdpPacket);

                break;
            } catch (SocketTimeoutException exception) {
                System.out.println("Received timeout while waiting to receive response from server, retry count = " + numOfRetries);
            }

            numOfRetries++;

            if (numOfRetries == DnsUtils.MAXIMUM_SEND_RETRY_COUNT)
                throw new SocketTimeoutException("Maximum retry count reached");
        }

        // close the socket
        dnsServerSocket.close();

        // return the packet, but use the actual length
        return new DatagramPacket(incomingDnsUdpPacket.getData(), incomingDnsUdpPacket.getLength(), incomingDnsUdpPacket.getAddress(), incomingDnsUdpPacket.getPort());
    }

    /**
     * Sends the original dns query received from the client to a random root server
     *
     * @return DnsPacket wrapping the response received from the root server
     * @throws IOException from either IO or Socket failures
     */
    private DnsPacket sendQueryToRootServer() throws IOException {
        var randomRootServer = DnsUtils.getRandomRootDnsServer();

        System.out.println("Querying root server " + randomRootServer.getHostName());

        var clientRequestPacket = originalDnsRequestReceived.rawUdpPacket;

        var outboundDnsUdpPacket = new DatagramPacket(clientRequestPacket.getData(), clientRequestPacket.getLength(), randomRootServer, DnsUtils.OUTBOUND_PORT);

        var incomingDnsUdpPacket = sendPacketAndReceiveResponse(outboundDnsUdpPacket);

        currentDnsPacketLength = incomingDnsUdpPacket.getLength();

        return DnsPacket.CreateFromUdpPacket(incomingDnsUdpPacket);
    }
}
