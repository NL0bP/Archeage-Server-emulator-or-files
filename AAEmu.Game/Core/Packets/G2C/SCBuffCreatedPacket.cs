using AAEmu.Commons.Network;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Skills;

namespace AAEmu.Game.Core.Packets.G2C
{
    public class SCBuffCreatedPacket : GamePacket
    {
        private readonly Effect _effect;

        public SCBuffCreatedPacket(Effect effect) : base(SCOffsets.SCBuffCreatedPacket, 5)
        {
            _effect = effect;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_effect.SkillCaster);             // skillCaster
            stream.Write(_effect.Caster is Character character ? character.Id : 0); // casterId
            stream.WriteBc(_effect.Owner.ObjId);           // targetId
            stream.Write(_effect.Index);                   // buffId

            stream.Write(_effect.Template.BuffId);         // t template buffId
            stream.Write(_effect.Caster.Level);            // l sourceLevel
            stream.Write(_effect.AbLevel);                 // a sourceAbLevel
            stream.Write(_effect.Skill?.Template.Id ?? 0); // s skillId
            stream.Write(0);                               // stack add in 3.0.3.0

            _effect.WriteData(stream);

            return stream;
        }
    }
}
