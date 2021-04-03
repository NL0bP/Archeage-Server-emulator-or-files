using System;
using AAEmu.Commons.Network;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Network.Connections;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Observers;

namespace AAEmu.Game.Core.Packets.C2G
{
    public class CSBroadcastVisualOption_0_Packet : GamePacket
    {
        public CSBroadcastVisualOption_0_Packet() : base(CSOffsets.CSBroadcastVisualOption_0_Packet, 5)
        {
        }

        public override void Read(PacketStream stream)
        {
            Connection.State = GameState.World;

            Connection.ActiveChar.VisualOptions = new CharacterVisualOptions();
            Connection.ActiveChar.VisualOptions.Read(stream);

            Connection.SendPacket(new SCUnitStatePacket(Connection.ActiveChar));

            //Connection.SendPacket(new SCDailyResetPacket(3));
            //Connection.SendPacket(new SCDailyResetPacket(9));
            //Connection.SendPacket(new SCCurServerTimePacket(DateTime.Now));

            Connection.ActiveChar.PushSubscriber(TimeManager.Instance.Subscribe(Connection, new TimeOfDayObserver(Connection.ActiveChar)));

            _log.Info("CSBroadcastVisualOption_0_Packet");
        }
    }
}
