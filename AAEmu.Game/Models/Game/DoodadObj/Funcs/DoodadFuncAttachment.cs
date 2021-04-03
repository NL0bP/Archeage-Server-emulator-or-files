using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.DoodadObj.Static;
using AAEmu.Game.Models.Game.DoodadObj.Templates;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.DoodadObj.Funcs
{
    public class DoodadFuncAttachment : DoodadFuncTemplate
    {
        public AttachPoint AttachPointId { get; set; }
        public int Space { get; set; }
        public uint AnimActionId { get; set; }
        public byte BondKindId { get; set; }

        public override void Use(Unit caster, Doodad owner, uint skillId)
        {
            _log.Debug("DoodadFuncAttachment");
            if (caster is Character character)
            {
                if (BondKindId > 1)
                {
                    // Chairs, beds etc.
                    AnimActionId = 0; // TODO пока нет серверной базы для 3+
                    var Spot = 0;// spot = 0 sit left, = 1 sit right on the bench
                    character.Bonding = new BondDoodad(owner, AttachPointId, Space, Spot, AnimActionId);
                    character.BroadcastPacket(new SCBondDoodadPacket(caster.ObjId, character.Bonding), true);
                }
                else
                {
                    // Ships
                    SlaveManager.Instance.BindSlave(character, owner.ParentObjId);
                }
            }
        }
    }
}
