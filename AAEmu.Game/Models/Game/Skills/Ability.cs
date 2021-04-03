using AAEmu.Game.Models.Game.Skills.Static;

namespace AAEmu.Game.Models.Game.Skills
{

    public class Ability
    {
        public AbilityType Id { get; set; }
        public byte Order { get; set; }
        public int Exp { get; set; }

        public Ability()
        {
            Order = 255;
        }

        public Ability(AbilityType id)
        {
            Id = id;
            Order = 255;
        }
    }
}
