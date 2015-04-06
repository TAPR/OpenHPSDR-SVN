package org.g0orx.openhpsdr.discovery;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.NetworkInterface;
import java.net.SocketException;
import java.net.SocketTimeoutException;
import java.net.UnknownHostException;
import java.util.Enumeration;

import org.g0orx.openhpsdr.Log;

public class Discovery extends Thread {

    public Discovery(Discover handler) {
        this.handler = handler;
    }

    public void run() {
        String result = "";
        try {
            for (Enumeration<NetworkInterface> en = NetworkInterface.getNetworkInterfaces(); en.hasMoreElements();) {
                NetworkInterface intf = en.nextElement();
                for (Enumeration<InetAddress> enumIpAddr = intf.getInetAddresses(); enumIpAddr.hasMoreElements();) {
                    InetAddress inetAddress = enumIpAddr.nextElement();
                    if (!inetAddress.isLoopbackAddress()) {
                        result = inetAddress.getHostAddress().toString();
                        if (result.contains(".")) {
                            Log.i("Discovery", "Using interface " + intf.toString() + " " + result);
                            openSocket(inetAddress, intf.getName());
                            sendDiscoveryPacket();
                            receiveDiscoveryPackets(result, intf.getName());
                        }
                    }
                }
            }
        } catch (Exception ex) {
            Log.i("Discovery", "getLocalIpAddress:" + ex.toString());
        }

        handler.endDiscovery();
    }

    public void openSocket(InetAddress myaddress, String name) {
        Log.i("Discovery", "openSocket: "+myaddress.getHostAddress()+" "+name);
        try {
            //InetSocketAddress socketaddress = new InetSocketAddress(myaddress, myport);
            socket = new DatagramSocket(myport,myaddress);
            socket.setReuseAddress(true);
            socket.setBroadcast(true);
            socket.setSoTimeout(0);
            running=true;
        } catch (Exception e) {
            Log.i("Discovery", "openSocket: exception:" + e.toString());
        }
    }
    public void sendDiscoveryPacket() {
        // send a discovery packet
        byte buffer[] = new byte[63];
        buffer[0] = (byte) 0xEF;
        buffer[1] = (byte) 0xFE;
        buffer[2] = (byte) 0x02;
        for (int i = 3; i < 63; i++) {
            buffer[i] = (byte) 0x00;
        }
        try {
            InetAddress address = InetAddress.getByName(toaddress);
            DatagramPacket datagram = new DatagramPacket(buffer, buffer.length, address, toport);
            if (socket != null) {
                socket.send(datagram);
            }
        } catch (SocketException se) {
            Log.i("DiscoverActivity", "DiscoveryAsyncTask.doInBackground: " + se.toString());
            running = false;
        } catch (UnknownHostException uhe) {
            Log.i("DiscoverActivity", "DiscoveryAsyncTask.doInBackground: " + uhe.toString());
            running = false;
        } catch (IOException ioe) {
            Log.i("DiscoverActivity", "DiscoveryAsyncTask.doInBackground: " + ioe.toString());
            running = false;
        }
    }
    
    public void receiveDiscoveryPackets(String myaddress, String name) {
        while (running) {
            try {
                rxdatagram = new DatagramPacket(rxbuffer, rxbuffer.length);
                socket.setSoTimeout(2000); // timeout after 2 seconds
                socket.receive(rxdatagram);

                String deviceaddress = rxdatagram.getAddress().toString();
                Log.i("Discovery", "received: "+deviceaddress);
                if (deviceaddress.startsWith("/")) {
                    deviceaddress = deviceaddress.substring(1);
                }
                if ((rxbuffer[0] & 0xFF) == 0xEF && (rxbuffer[1] & 0xFF) == 0xFE) {
                    int status = rxbuffer[2] & 0xFF;
                    if (status == 2 || status == 3) {
                        String macaddress;
                        int softwareversion;
                        int device;
                        macaddress = String.format("%02X", rxbuffer[3] & 0xFF)
                                + ":"
                                + String.format("%02X", rxbuffer[4] & 0xFF)
                                + ":"
                                + String.format("%02X", rxbuffer[5] & 0xFF)
                                + ":"
                                + String.format("%02X", rxbuffer[6] & 0xFF)
                                + ":"
                                + String.format("%02X", rxbuffer[7] & 0xFF)
                                + ":"
                                + String.format("%02X", rxbuffer[8] & 0xFF);

                        softwareversion = rxbuffer[9] & 0xFF;
                        device = rxbuffer[10] & 0xff;

                        if (!deviceaddress.equals(myaddress)) {
                            Discovered d = new Discovered();
                            d.setInterface(myaddress);
                            d.setInterfaceName(name);
                            d.setDevice(device);
                            d.setAddress(deviceaddress);
                            d.setMac(macaddress);
                            d.setSoftwareversion(softwareversion);
                            d.setState(status);

                            handler.discovered(d);
                        }
                    } else {
                        Log.i("Discovery", "run: expected status 2 or 3 got " + status);
                    }
                } else {
                    Log.i("Discover", "run: received unknown packet: length:" + rxdatagram.getLength());
                    for (int i = 0; i < rxdatagram.getLength(); i++) {
                        Log.i("Discovery", Integer.toString(i) + ":" + Integer.toHexString(rxbuffer[i] & 0xFF));
                    }
                }
            } catch (SocketTimeoutException e) {
                Log.i("Discovery", e.toString());
                running = false;
            } catch (Exception e) {
                Log.i("Discovery", "run: Exception: " + e.toString());
                running = false;
            }
        }

        if (socket != null) {
            Log.i("Discovery", "close socket");
            socket.close();
        }

    }

    public void terminate() {
        running = false;
    }
    
    private String getLocalIpAddress() {
        String result = "";
        try {
            for (Enumeration<NetworkInterface> en = NetworkInterface.getNetworkInterfaces(); en.hasMoreElements();) {
                NetworkInterface intf = en.nextElement();
                for (Enumeration<InetAddress> enumIpAddr = intf.getInetAddresses(); enumIpAddr.hasMoreElements();) {
                    InetAddress inetAddress = enumIpAddr.nextElement();
                    if (!inetAddress.isLoopbackAddress()) {
                        result = inetAddress.getHostAddress().toString();
                        if (result.contains(".")) {
                            Log.i("Discovery", "Using interface " + intf.toString() + " " + result);
                            return result;
                        }
                    }
                }
            }
        } catch (Exception ex) {
            Log.i("Discovery", "getLocalIpAddress:" + ex.toString());
        }
        return result;
    }
    
    private boolean running;
    private DatagramSocket socket;
    private int myport = 1024;
    private int toport = 1024;
    private String toaddress = "255.255.255.255";

    private DatagramPacket rxdatagram;
    private byte rxbuffer[] = new byte[102];

    private Discover handler;
}
