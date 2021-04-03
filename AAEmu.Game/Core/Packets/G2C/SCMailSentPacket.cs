using AAEmu.Commons.Network;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Models.Game.Items;
using AAEmu.Game.Models.Game.Mails;

namespace AAEmu.Game.Core.Packets.G2C
{
    public class SCMailSentPacket : GamePacket
    {
        private readonly MailHeader _mail;
        private readonly CountUnreadMail _countUnread;
        private readonly (SlotType slotType, byte slot)[] _items;

        public SCMailSentPacket(MailHeader mail, CountUnreadMail countUnread, (SlotType slotType, byte slot)[] items)
            : base(SCOffsets.SCMailSentPacket, 5)
        {
            _mail = mail;
            _countUnread = countUnread;
            _items = items;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_mail);
            stream.Write(_countUnread);
            foreach (var (slotType, slot) in _items) // TODO should be 10 items
            {
                stream.Write((byte)slotType); // type
                stream.Write(slot);           // index
            }

            return stream;
        }
    }
}
