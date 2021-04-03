using AAEmu.Game.Models.Game.Gimmicks;
using AAEmu.Game.Models.Game.NPChar;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Models.Game.Units.Route;

namespace AAEmu.Game.Models.Tasks.UnitMove
{
    public class Move : Task
    {
        private readonly Simulation _patrol;
        private readonly Unit _unit;
        private readonly float _targetX;
        private readonly float _targetY;
        private readonly float _targetZ;

        /// <summary>
        /// Move task
        /// </summary>
        /// <param name="patrol"></param>
        /// <param name="unit"></param>
        /// <param name="TargetX"></param>
        /// <param name="TargetY"></param>
        /// <param name="TargetZ"></param>
        public Move(Simulation patrol, Unit unit, float TargetX, float TargetY, float TargetZ)
        {
            _patrol = patrol;
            _targetX = TargetX;
            _targetY = TargetY;
            _targetZ = TargetZ;
            _unit = unit;
        }

        /// <summary>
        /// Perform tasks
        /// </summary>
        public override void Execute()
        {
            switch (_unit)
            {
                case Npc _npc:
                    _patrol?.MoveTo(_patrol, _npc, _targetX, _targetY, _targetZ);
                    break;
                case Gimmick _gimmick:
                    _patrol?.MoveTo(_patrol, _gimmick, _targetX, _targetY, _targetZ);
                    break;
                case Transfer _transfer:
                    _patrol?.MoveTo(_patrol, _transfer, _targetX, _targetY, _targetZ);
                    break;
            }
        }
    }
}
