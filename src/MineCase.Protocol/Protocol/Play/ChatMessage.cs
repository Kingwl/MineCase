﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MineCase.Formats;
using MineCase.Serialization;

namespace MineCase.Protocol.Play
{
    [Packet(0x03)]
    public sealed class ServerboundChatMessage
    {
        [SerializeAs(DataType.String)]
        public string Message;

        public static ServerboundChatMessage Deserialize(BinaryReader br)
        {
            return new ServerboundChatMessage
            {
                Message = br.ReadAsString()
            };
        }
    }

    // TODO
    [Packet(0x0F)]
    public sealed class ClientboundChatMessage : ISerializablePacket
    {
        [SerializeAs(DataType.Chat)]
        public Chat JSONData;

        [SerializeAs(DataType.Byte)]
        public byte Position; // 0: chat (chat box), 1: system message (chat box), 2: game info (above hotbar).

        public void Serialize(BinaryWriter bw)
        {
            bw.WriteAsChat(JSONData);
            bw.WriteAsByte(Position);
        }
    }
}
