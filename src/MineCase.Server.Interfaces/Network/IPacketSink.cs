﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MineCase.Protocol;

namespace MineCase.Server.Network
{
    public interface IPacketSink
    {
        Task<(uint packetId, byte[] data)> PreparePacket(ISerializablePacket packet);

        Task SendPacket(ISerializablePacket packet);

        Task SendPacket(uint packetId, byte[] data);
    }
}
