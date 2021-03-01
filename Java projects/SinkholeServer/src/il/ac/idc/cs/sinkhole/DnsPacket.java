package il.ac.idc.cs.sinkhole;

import java.net.DatagramPacket;
import java.nio.ByteBuffer;

/**
 * This class represents a DNS packet and exposes several DNS methods such as response flag manipulation, getting parameters from the packet, etc.
 */
class DnsPacket {

    // private constructor because we want to allow only via our static Parser/builder helper method
    private DnsPacket() {
    }

    // underlying UDP packet of this DnsPacket
    DatagramPacket rawUdpPacket;

    // was an error received in this packet?
    private boolean errorReceived;

    // number of answers RR records received
    private short numberOfAnswers;

    // number of authority RR records received
    private short numberOfAuthorities;

    // the domain name in the query
    private String addressQueried;

    // The first (if any) authority server received in the packet
    private String authorityServer;

    // The first (if any) answer received in the packet
    private String answer;

    /**
     * Create a new DnsPacket object from the udp packet
     *
     * @param packet raw udp packet
     * @return DnsPacket object
     */
    static DnsPacket CreateFromUdpPacket(DatagramPacket packet) {

        var dnsPacket = new DnsPacket();

        // save the original packet
        dnsPacket.rawUdpPacket = packet;

        // wrap with ByteBuffer class for ease of use
        ByteBuffer byteBuffer = ByteBuffer.wrap(packet.getData());

        // skip over the transaction ID
        skipNBytes(byteBuffer, 2);

        // response flags - total of 2 bytes
        var flagFirstByte = byteBuffer.get();
        var flagSecondByte = byteBuffer.get();

        // determine whether an error was received
        dnsPacket.errorReceived = (flagSecondByte & DnsUtils.ERROR_CODE_FLAG_CHECKER) != 0;

        // discard number of questions, always = 1
        skipNBytes(byteBuffer, 2);

        // get number of answers and authority RRs we received
        dnsPacket.numberOfAnswers = byteBuffer.getShort();
        dnsPacket.numberOfAuthorities = byteBuffer.getShort();

        // discard number of additional RRs
        skipNBytes(byteBuffer, 2);

        // parse queried domain
        dnsPacket.addressQueried = parseDomainName(byteBuffer);

        // skip redundant (for our case) bytes
        skipNBytes(byteBuffer, 4);

        // if we got an answer:
        if (dnsPacket.getNumberOfAnswers() > 0) {

            dnsPacket.answer = resolveAddressInRR(byteBuffer);

        } else if (dnsPacket.getNumberOfAuthorities() > 0) {

            // authority has 12 bytes of "headers"
            skipNBytes(byteBuffer, 12);
            dnsPacket.authorityServer = parseDomainName(byteBuffer);
        }

        return dnsPacket;
    }

    /**
     * Set valid response flags on the raw packet
     */
    void setValidResponseFlags() {

        var rawUdpPacketData = rawUdpPacket.getData();

        var firstByteOfResponseFlag = rawUdpPacketData[DnsUtils.FIRST_BYTE_RESPONSE_FLAG_POS];

        firstByteOfResponseFlag &= (byte) 0b11111011;

        // make sure we set as response
        firstByteOfResponseFlag |= (byte) 0b10000000;

        var secondByteOfResponseFlag = rawUdpPacketData[DnsUtils.SECOND_BYTE_RESPONSE_FLAG_POS];

        secondByteOfResponseFlag |= (byte) 0b10000000;

        rawUdpPacketData[2] = firstByteOfResponseFlag;
        rawUdpPacketData[3] = secondByteOfResponseFlag;

        rawUdpPacket.setData(rawUdpPacketData);
    }

    /**
     * Set Error "NXDOMAIN" - response code 3 on the raw udp packet
     */
    void setBlockedDomainError() {

        var rawUdpPacketData = rawUdpPacket.getData();
        var secondByteOfResponseFlag = rawUdpPacketData[DnsUtils.SECOND_BYTE_RESPONSE_FLAG_POS];

        secondByteOfResponseFlag &= (byte) 0b11110000;
        secondByteOfResponseFlag |= (byte) 0b00000011;

        rawUdpPacketData[3] = secondByteOfResponseFlag;
        rawUdpPacket.setData(rawUdpPacketData);
    }

    /**
     * Get number of answers RRs
     */
    short getNumberOfAnswers() {
        return numberOfAnswers;
    }

    /**
     * Get number of authority RRs
     */
    short getNumberOfAuthorities() {
        return numberOfAuthorities;
    }

    /**
     * Determine whether the dns packet is valid or not
     */
    boolean isValidDnsQuery() {
        return !errorReceived;
    }

    /**
     * Get the first authority server (if any) from the packet
     */
    String getAuthorityServer() {
        return authorityServer;
    }


    /**
     * Get domain that was queried
     */
    String getDomainQueried() {
        return addressQueried;
    }

    /**
     * Get the first answer(if any) from the packet
     */
    String getAnswer() {
        return answer;
    }

    /**
     * Parse address as per the RFC instruction. Handles compression
     *
     * @param byteBuffer the byteBuffer in which the address to be parsed is in
     * @return the address to extract
     */
    private static String parseDomainName(ByteBuffer byteBuffer) {
        // get label
        var label = byteBuffer.get();

        var stringBuilder = new StringBuilder();

        // address parsing stops when we get an '0' label
        while (label != 0) {

            // handle compression (for the compression pointer, the MSB must be 1 which means it is negative, normal labels can't be bigger than 64)
            if (label < 0) {

                // save only the 6 LSBs (offset is 14 bit long, the 6 bits from the first octet which signaled compression + 8 bits of the 2nd octet)
                var offsetMsbs = label & DnsUtils.COMPRESSION_POINTER_INDICATOR;

                // move the MSBs 8 bits to the left
                offsetMsbs <<= 8;

                // add the 8 LSBs
                var offset = offsetMsbs | (int) byteBuffer.get();

                // move the byte buffer to the offset position
                byteBuffer.position(offset);
            }
            // no compression: read the <label> next octets and add to string builder
            else {
                for (byte i = 0; i < label; i++) {
                    stringBuilder.append((char) byteBuffer.get());
                }
            }
            // get the next label
            label = byteBuffer.get();

            // if we still have more to parse, add a dot
            if (label > 0)
                stringBuilder.append('.');
        }
        var address = stringBuilder.toString();

        // check edge case where the queried domain is also a FQDN of one of the authority servers, and compression is used:
        // e.g. query for a.ns.facebook.com, the compression offset pointer will point exactly at the original query domain
        // which will then add an erroneous dot at the start of the address
        if (address.startsWith(".")) {
            address = address.substring(1);
        }

        return address;
    }

    /**
     * Skips the byte buffer's current position by n bytes
     *
     * @param byteBuffer the byte buffer to skip
     * @param n          number of bytes to skip
     */
    private static void skipNBytes(ByteBuffer byteBuffer, int n) {
        byteBuffer.position(byteBuffer.position() + n);
    }

    /**
     * Resolve answer RR address: Either an ARPA 4 byte address or a domain name,
     * According to the TYPE received in the response
     *
     * @param byteBuffer the byteBuffer in which the address to be parsed is in
     * @return the address to extract
     */
    private static String resolveAddressInRR(ByteBuffer byteBuffer) {

        parseDomainName(byteBuffer);

        // get answer type
        var answerType = byteBuffer.getShort();

        // skip redundant (for our case) bytes
        skipNBytes(byteBuffer, 8);

        // if A - type, parse as 4 numbers that indicate IP address
        if (answerType == DnsUtils.A_TYPE_RESOURCE_RECORD_VALUE) {
            return parseARPAAddress(byteBuffer);
        }

        // otherwise, (e.g. NS / CNAME) - parse as domain
        return parseDomainName(byteBuffer);
    }

    /**
     * Parse an ARPA address (4 bytes: X.X.X.X)
     *
     * @param byteBuffer the byteBuffer in which the address to be parsed is in
     * @return the address to extract
     */
    private static String parseARPAAddress(ByteBuffer byteBuffer) {

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < 4; i++) {

            var nextValue = byteBuffer.get();

            var unSignedValue = nextValue & 0xff;

            stringBuilder.append(unSignedValue);

            if (i != 3) {
                stringBuilder.append(".");
            }
        }

        return stringBuilder.toString();
    }

}
