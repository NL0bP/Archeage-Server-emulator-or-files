﻿using System;

using AAEmu.Game.Models.Game.AI.Abstracts;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.NPChar;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Models.Game.Units.Movements;
using AAEmu.Game.Models.Game.World;

/*
   Author:Sagara, NLObP
*/
namespace AAEmu.Game.Models.Game.AI
{
    public sealed class PlayerAi : ACreatureAi
    {
        public PlayerAi(GameObject owner, float visibleRange) : base(owner, visibleRange)
        {
        }

        protected override void IamSeeSomeone(GameObject someone)
        {
            //switch (someone.UnitType)
            //{
            //    case BaseUnitType.Character:
            //        break;
            //    case BaseUnitType.Npc:
            //        //var npc = (Npc)someone;
            //        //var chr = (Character)Owner;
            //        //var target = (BaseUnit)Owner;
            //        //// если Npc агрессивный, то будет смотреть на нас и нападёт, иначе просто смотрит на нас
            //        //if (!npc.IsInBattle && npc.Template.Aggression)
            //        //{
            //        //    if (npc.Template.AggroLinkHelpDist > Math.Abs(MathUtil.CalculateDistance(npc.Position, chr.Position)))
            //        //    {
            //        //        //NPCs are moving squarely
            //        //        var square = new Square { Interrupt = true, Loop = true, Abandon = false, Degree = 360 };
            //        //        npc.Patrol = square;
            //        //        npc.Patrol.Pause(npc);
            //        //        npc.Patrol.LastPatrol = null;
            //        //        npc.Patrol.Recovery(npc);
            //        //        chr.BroadcastPacket(new SCAiAggroPacket(npc.ObjId, 1, chr.ObjId), true);
            //        //        chr.BroadcastPacket(new SCCombatEngagedPacket(npc.ObjId), true); // caster
            //        //        chr.BroadcastPacket(new SCCombatEngagedPacket(chr.ObjId), true); // target
            //        //        //chr.BroadcastPacket(new SCCombatFirstHitPacket(chr.ObjId, npc.ObjId, 0), true);
            //        //        chr.BroadcastPacket(new SCAggroTargetChangedPacket(npc.ObjId, chr.ObjId), true);
            //        //        chr.BroadcastPacket(new SCTargetChangedPacket(npc.ObjId, chr.ObjId), true);
            //        //        //TaskManager.Instance.Schedule(new UnitMove(new Track(), npc), TimeSpan.FromMilliseconds(1000));
            //        //        npc.CurrentTarget = target;
            //        //        npc.SetForceAttack(true);
            //        //        npc.IsAutoAttack = true;
            //        //        npc.IsInBattle = true;
            //        //        var combat = new Combat();
            //        //        combat.Execute(npc);
            //        //    }
            //        //    else
            //        //    {
            //        //        // here the NPCs you can hunt, check that they are not protected by Guards
            //        //        if (npc.Faction.GuardHelp == false)
            //        //        {
            //        //            if (npc.Patrol == null)
            //        //            {
            //        //                npc.IsInBattle = false;
            //        //                Patrol patrol = null;
            //        //                var rnd = Rand.Next(0, 1000);
            //        //                if (rnd > 700)
            //        //                {
            //        //                    // NPC stand still
            //        //                    // turned it off because the NPCs are leaving their seats.
            //        //                    npc.Patrol = null;
            //        //                    // NPC is moving slowly
            //        //                    //var stirring = new Stirring() { Interrupt = true, Loop = true, Abandon = false };
            //        //                    //stirring.Degree = (short)Rand.Next(180, 360);
            //        //                    //patrol = stirring;
            //        //                }
            //        //                else if (rnd > 600)
            //        //                {
            //        //                    // NPCs are moving squarely
            //        //                    var square = new Square { Interrupt = true, Loop = true, Abandon = false, Degree = 360 };
            //        //                    // (short)Rand.Next(180, 360); checking that they're not leaving their seats.
            //        //                    patrol = square;
            //        //                }
            //        //                else if (rnd > 500)
            //        //                {
            //        //                    // NPCs are moving around in a circle
            //        //                    patrol = new Circular { Interrupt = true, Loop = true, Abandon = false };
            //        //                }
            //        //                else if (rnd > 400)
            //        //                {
            //        //                    // NPC stand still
            //        //                    // turned it off because the NPCs are leaving their seats.
            //        //                    npc.Patrol = null;
            //        //                    // NPCs are jerking around
            //        //                    //var jerky = new Jerky { Interrupt = true, Loop = true, Abandon = false };
            //        //                    //jerky.Degree = (short)Rand.Next(180, 360);
            //        //                    //patrol = jerky;
            //        //                }
            //        //                else if (rnd > 300)
            //        //                {
            //        //                    // NPC move along the weaving shuttle in the Y-axis.
            //        //                    var quill = new QuillY { Interrupt = true, Loop = true, Abandon = false, Degree = (short)Rand.Next(180, 360) };
            //        //                    patrol = quill;
            //        //                }
            //        //                else if (rnd > 200)
            //        //                {
            //        //                    // NPC move along the weaving shuttle in the X-axis.
            //        //                    var quill = new QuillX { Interrupt = true, Loop = true, Abandon = false, Degree = (short)Rand.Next(180, 360) };
            //        //                    patrol = quill;
            //        //                }
            //        //                else
            //        //                {
            //        //                    // NPC stand still
            //        //                    npc.Patrol = null;
            //        //                }
            //        //                if (patrol != null)
            //        //                {
            //        //                    patrol.Pause(npc);
            //        //                    npc.Patrol = patrol;
            //        //                    npc.Patrol.LastPatrol = patrol;
            //        //                    patrol.Recovery(npc);
            //        //                }
            //        //            }
            //        //        }
            //        //    }
            //        //}
            //        //else
            //        //{
            //        //    // here the NPCs you can hunt, check that they are not protected by Guards
            //        //    if (npc.Faction.GuardHelp == false)
            //        //    {
            //        //        if (npc.Patrol == null)
            //        //        {
            //        //            npc.IsInBattle = false;
            //        //            Patrol patrol = null;
            //        //            var rnd = Rand.Next(0, 1000);
            //        //            if (rnd > 700)
            //        //            {
            //        //                // NPC stand still
            //        //                // turned it off because the NPCs are leaving their seats.
            //        //                npc.Patrol = null;
            //        //                // NPC is moving slowly
            //        //                //var stirring = new Stirring() { Interrupt = true, Loop = true, Abandon = false };
            //        //                //stirring.Degree = (short)Rand.Next(180, 360);
            //        //                //patrol = stirring;
            //        //            }
            //        //            else if (rnd > 600)
            //        //            {
            //        //                // NPCs are moving squarely
            //        //                var square = new Square { Interrupt = true, Loop = true, Abandon = false, Degree = 360 };
            //        //                // (short)Rand.Next(180, 360); checking that they're not leaving their seats.
            //        //                patrol = square;
            //        //            }
            //        //            else if (rnd > 500)
            //        //            {
            //        //                // NPCs are moving around in a circle
            //        //                patrol = new Circular { Interrupt = true, Loop = true, Abandon = false };
            //        //            }
            //        //            else if (rnd > 400)
            //        //            {
            //        //                // NPC stand still
            //        //                // turned it off because the NPCs are leaving their seats.
            //        //                npc.Patrol = null;
            //        //                // NPCs are jerking around
            //        //                //var jerky = new Jerky { Interrupt = true, Loop = true, Abandon = false };
            //        //                //jerky.Degree = (short)Rand.Next(180, 360);
            //        //                //patrol = jerky;
            //        //            }
            //        //            else if (rnd > 300)
            //        //            {
            //        //                // NPC move along the weaving shuttle in the Y-axis.
            //        //                var quill = new QuillY { Interrupt = true, Loop = true, Abandon = false, Degree = (short)Rand.Next(180, 360) };
            //        //                patrol = quill;
            //        //            }
            //        //            else if (rnd > 200)
            //        //            {
            //        //                // NPC move along the weaving shuttle in the X-axis.
            //        //                var quill = new QuillX { Interrupt = true, Loop = true, Abandon = false, Degree = (short)Rand.Next(180, 360) };
            //        //                patrol = quill;
            //        //            }
            //        //            else
            //        //            {
            //        //                // NPC stand still
            //        //                npc.Patrol = null;
            //        //            }

            //        //            if (patrol != null)
            //        //            {
            //        //                patrol.Pause(npc);
            //        //                npc.Patrol = patrol;
            //        //                npc.Patrol.LastPatrol = patrol;
            //        //                patrol.Recovery(npc);
            //        //            }
            //        //        }
            //        //    }

            //        //    //var seq = (uint)(DateTime.Now - GameService.StartTime).TotalMilliseconds;
            //        //    //var unitMovement = (ActorData)UnitMovement.GetType(UnitMovementType.Actor);
            //        //    //unitMovement.X = npc.Position.X;
            //        //    //unitMovement.Y = npc.Position.Y;
            //        //    //unitMovement.Z = npc.Position.Z;
            //        //    //var angle = MathUtil.CalculateAngleFrom(npc, chr);
            //        //    //var rotZ = MathUtil.ConvertDegreeToDirection(angle);
            //        //    //unitMovement.RotationX = 0;
            //        //    //unitMovement.RotationY = 0;
            //        //    //unitMovement.RotationZ = rotZ;
            //        //    //unitMovement.Flags = 5;
            //        //    //unitMovement.DeltaMovement = new sbyte[3];
            //        //    //unitMovement.DeltaMovement[0] = 0;
            //        //    //unitMovement.DeltaMovement[1] = 0;
            //        //    //unitMovement.DeltaMovement[2] = 0;
            //        //    //unitMovement.Stance = 1;    //combat=0, idle=1
            //        //    //unitMovement.Alertness = 0; //idle=0, combat=2
            //        //    //unitMovement.Time = seq;
            //        //    //chr.BroadcastPacket(new SCOneUnitMovementPacket(npc.ObjId, unitMovement), true);
            //        //}
            //        break;
            //    case BaseUnitType.Slave:
            //        break;
            //    case BaseUnitType.Housing:
            //        break;
            //    case BaseUnitType.Transfer:
            //        break;
            //    case BaseUnitType.Mate:
            //        break;
            //    case BaseUnitType.Shipyard:
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
        }

        protected override void IamUnseeSomeone(GameObject someone)
        {
            switch (someone.UnitType)
            {
                case BaseUnitType.Character:
                    break;
                case BaseUnitType.Npc:
                    var npc = (Npc)someone;
                    var character = (Character)Owner;
                    if (!npc.IsInBattle && npc.Hp > 0 && npc.Patrol != null)
                    {
                        // NPC stand still
                        //npc.Patrol = null;
                        npc.Patrol?.Stop(npc);
                        npc.RemoveVisibleObject(character);
                    }
                    break;
                case BaseUnitType.Slave:
                    break;
                case BaseUnitType.Housing:
                    break;
                case BaseUnitType.Transfer:
                    break;
                case BaseUnitType.Mate:
                    break;
                case BaseUnitType.Shipyard:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void SomeoneSeeMe(GameObject someone)
        {
        }

        protected override void SomeoneUnseeMee(GameObject someone)
        {
        }

        protected override void SomeoneThatIamSeeWasMoved(GameObject someone, MovementAction action)
        {
        }

        protected override void SomeoneThatSeeMeWasMoved(GameObject someone, MovementAction action)
        {
        }
    }
}
