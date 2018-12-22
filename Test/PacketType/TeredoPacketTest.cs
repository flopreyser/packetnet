﻿/*
This file is part of PacketDotNet
PacketDotNet is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
PacketDotNet is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.
You should have received a copy of the GNU Lesser General Public License
along with PacketDotNet.  If not, see <http://www.gnu.org/licenses/>.
*/
/*
 *  Copyright 2018 Steven Haufe<haufes@hotmail.com>
 */

using System;
using NUnit.Framework;
using SharpPcap.LibPcap;
using PacketDotNet;

namespace Test.PacketType
{
    [TestFixture]
    public class TeredoPacketTest
    {
        [Test]
        public void TeredoParsing()
        {
            var dev = new CaptureFileReaderDevice("../../CaptureFiles/teredo.pcap");
            dev.Open();
            var rawCapture = dev.GetNextPacket();
            dev.Close();

            Packet ethernetPacket = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);
            Assert.IsNotNull(ethernetPacket);
            Assert.AreEqual(typeof(EthernetPacket), ethernetPacket.GetType());

            Packet ip4Packet = ethernetPacket.PayloadPacket;
            Assert.IsNotNull(ip4Packet);
            Assert.AreEqual(typeof(IPv4Packet), ip4Packet.GetType());

            Packet udpPacket = ip4Packet.PayloadPacket;
            Assert.IsNotNull(udpPacket);
            Assert.AreEqual(typeof(UdpPacket), udpPacket.GetType());

            Packet tunneledIp6Packet = udpPacket.PayloadPacket;
            Assert.IsNotNull(tunneledIp6Packet);
            Assert.AreEqual(typeof(IPv6Packet), tunneledIp6Packet.GetType());

            Packet tunneledTcpPacket = tunneledIp6Packet.PayloadPacket;
            Assert.IsNotNull(tunneledTcpPacket);
            Assert.AreEqual(typeof(ICMPv6Packet), tunneledTcpPacket.GetType());

        }
    }
}