using AAEmu.Game.Core.Managers.UnitManagers;
using AAEmu.Game.Models.Game.DoodadObj;
using AAEmu.Game.Models.Game.DoodadObj.Templates;
using AAEmu.Game.Models.Game.Skills;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.World.Interactions
{
    public class Looting : IWorldInteraction
    {
        public void Execute(Unit caster, SkillCaster casterType, BaseUnit target, SkillCastTarget targetType,
            uint skillId, uint itemId, DoodadFuncTemplate objectFunc)
        {
            if (target is Doodad doodad)
            {
                DoodadManager.Instance.TriggerPhases(GetType().Name, caster, doodad, skillId);
            }
        }
    }
}
