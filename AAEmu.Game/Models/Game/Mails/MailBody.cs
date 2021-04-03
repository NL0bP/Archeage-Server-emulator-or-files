using System;
using System.Collections.Generic;

using AAEmu.Commons.Network;
using AAEmu.Game.Models.Game.Items;

namespace AAEmu.Game.Models.Game.Mails
{
    public class MailBody : PacketMarshaler
    {
        public static byte MaxMailAttachments = 10;
        public long mailId { get; set; }
        public byte Type { get; set; }
        public string ReceiverName { get; set; } // TODO max length 128
        public string Title { get; set; } // TODO max length 1200
        public string Text { get; set; } // TODO max length 1600
        public int MoneyAmount1 { get; set; }
        public int MoneyAmount2 { get; set; }
        public int MoneyAmount3 { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime RecvDate { get; set; }
        public DateTime OpenDate { get; set; }
        public List<Item> Attachments { get; set; } // TODO max length 10

        public MailBody()
        {
            Attachments = new List<Item>();
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(mailId);
            stream.Write(Type);
            stream.Write(ReceiverName);
            stream.Write(Title);
            stream.Write(Text);
            stream.Write(MoneyAmount1);
            stream.Write(MoneyAmount2);
            stream.Write(MoneyAmount3);
            stream.Write(SendDate);
            stream.Write(RecvDate);
            stream.Write(OpenDate);
            for (var i = 0; i < MaxMailAttachments; i++)
            {
                if (i >= Attachments.Count || Attachments[i] == null)
                {
                    stream.Write(0);
                }
                else
                {
                    stream.Write(Attachments[i]);
                }
            }

            return stream;
        }
    }
}
