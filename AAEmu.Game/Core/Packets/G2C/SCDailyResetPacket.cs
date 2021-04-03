using System;
using AAEmu.Commons.Network;
using AAEmu.Game.Core.Network.Game;

namespace AAEmu.Game.Core.Packets.G2C
{
    public class SCDailyResetPacket : GamePacket
    {
        private readonly byte _kind;

        public SCDailyResetPacket(byte kind) : base(SCOffsets.SCDailyResetPacket, 5)
        {
            _kind = kind;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_kind);

            return stream;
        }
    }
}
