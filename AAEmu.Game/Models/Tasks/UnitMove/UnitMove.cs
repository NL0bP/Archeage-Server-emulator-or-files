using AAEmu.Game.Models.Game.Gimmicks;
using AAEmu.Game.Models.Game.NPChar;
using AAEmu.Game.Models.Game.Units;

using SQLitePCL;

namespace AAEmu.Game.Models.Tasks.UnitMove
{
    public class UnitMove : Task
    {
        private readonly Patrol _patrol;
        private readonly BaseUnit _unit;

        /// <summary>
        /// 初始化任务
        /// Initialization task
        /// </summary>
        /// <param name="patrol"></param>
        /// <param name="npc"></param>
        public UnitMove(Patrol patrol, BaseUnit unit)
        {
            _patrol = patrol;
            _unit = unit;
        }

        /// <summary>
        /// 执行任务
        /// Perform tasks
        /// </summary>
        public override void Execute()
        {
            switch (_unit)
            {
                case Npc _npc:
                    _patrol?.Apply(_npc);
                    break;
                case Gimmick _gimmick:
                    _patrol?.Apply(_gimmick);
                    break;
                case Transfer _transfer:
                    _patrol?.Apply(_transfer);
                    break;
            }
        }
    }
}
