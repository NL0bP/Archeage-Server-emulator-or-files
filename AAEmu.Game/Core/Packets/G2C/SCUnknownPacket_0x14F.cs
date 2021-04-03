using System;
using AAEmu.Commons.Network;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Models.Game.Skills;

namespace AAEmu.Game.Core.Packets.G2C
{
    public class SCUnknownPacket_0x14F : GamePacket
    {
        private readonly byte _AccountAttributeKind;
        private readonly uint _extraKind;
        private readonly byte _worldId;
        private readonly uint _count;
        private readonly DateTime _startDate;
        private readonly DateTime _endData;

        public SCUnknownPacket_0x14F() : base(SCOffsets.SCUnknownPacket_0x14F, 5)
        {
            _AccountAttributeKind = (byte) 1;
            _extraKind = 0;
            _worldId = 0x1;
            _count = 0;
            _startDate = DateTime.Now;
            _endData = DateTime.MinValue;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_count);
            for (var i = 0; i < _count; i++)
            {
                stream.Write(_AccountAttributeKind); // chatTypeGroup
                stream.Write(_extraKind);
                stream.Write(_worldId);
                stream.Write(_count);
                stream.Write(_startDate);
                stream.Write(_endData);
            }
            return stream;
        }
    }
}
