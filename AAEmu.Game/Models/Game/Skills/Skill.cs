﻿using System;
using System.Collections.Generic;
using System.Linq;

using AAEmu.Commons.Utils;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Managers.Id;
using AAEmu.Game.Core.Managers.World;
using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Faction;
using AAEmu.Game.Models.Game.Items;
using AAEmu.Game.Models.Game.Items.Actions;
using AAEmu.Game.Models.Game.Skills.Effects;
using AAEmu.Game.Models.Game.Skills.Plots;
using AAEmu.Game.Models.Game.Skills.Static;
using AAEmu.Game.Models.Game.Skills.Templates;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Models.Game.World;
using AAEmu.Game.Models.Tasks.Skills;
using AAEmu.Game.Utils;

using NLog;

namespace AAEmu.Game.Models.Game.Skills
{
    public class Skill
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public uint TemplateId { get; set; }
        public SkillTemplate Template { get; set; }
        public byte Level { get; set; }
        public ushort TlId { get; set; }

        public Skill()
        {
        }

        public Skill(SkillTemplate template)
        {
            TemplateId = template.Id;
            Template = template;
            Level = 1;
        }

        public void Use(Unit caster, SkillCaster casterType, SkillCastTarget targetType, SkillObject skillObject = null)
        {
            //if (caster is Character chr)
            //{
            //    var dist = MathUtil.CalculateDistance(chr.Position, chr.CurrentTarget.Position, true);
            //    if (dist > SkillManager.Instance.GetSkillTemplate(Id).MaxRange)
            //    {
            //        chr.SendMessage("Target is too far ...");
            //        return;
            //    }
            //}

            // TODO : Add check for range
            var skillRange = caster.ApplySkillModifiers(this, SkillAttribute.Range, Template.MaxRange);

            if (skillObject == null)
            {
                skillObject = new SkillObject();
            }
            var effects = caster.Effects.GetEffectsByType(typeof(BuffTemplate));
            foreach (var effect in effects)
            {
                if (effect != null && (((BuffTemplate)effect.Template).RemoveOnStartSkill || ((BuffTemplate)effect.Template).RemoveOnUseSkill))
                {
                    effect.Exit();
                }
            }
            effects = caster.Effects.GetEffectsByType(typeof(BuffEffect));
            foreach (var effect in effects)
            {
                if (effect != null && (((BuffEffect)effect.Template).Buff.RemoveOnStartSkill || ((BuffEffect)effect.Template).Buff.RemoveOnUseSkill))
                {
                    effect.Exit();
                }
            }

            var target = (BaseUnit)caster;

            if (Template.TargetType == SkillTargetType.Self)
            {
                if (targetType != null && (targetType.Type == SkillCastTargetType.Unit || targetType.Type == SkillCastTargetType.Doodad))
                {
                    targetType.ObjId = target.ObjId;
                }
            }
            else if (Template.TargetType == SkillTargetType.Friendly)
            {
                if (targetType != null && (targetType.Type == SkillCastTargetType.Unit || targetType.Type == SkillCastTargetType.Doodad))
                {
                    target = targetType.ObjId > 0 ? WorldManager.Instance.GetBaseUnit(targetType.ObjId) : caster;
                    if (target != null)
                    {
                        targetType.ObjId = target.ObjId;
                    }
                }
                else
                {
                    // TODO ...
                }

                if (target != null && caster.Faction.GetRelationState(target.Faction.Id) != RelationState.Friendly)
                {
                    return; //TODO отправлять ошибку?
                }
            }
            else if (Template.TargetType == SkillTargetType.Hostile)
            {
                if (targetType != null && (targetType.Type == SkillCastTargetType.Unit || targetType.Type == SkillCastTargetType.Doodad))
                {
                    target = targetType.ObjId > 0 ? WorldManager.Instance.GetBaseUnit(targetType.ObjId) : caster;
                    if (target != null)
                    {
                        targetType.ObjId = target.ObjId;
                    }
                }
                else
                {
                    // TODO ...
                }

                if (target != null && caster.Faction.GetRelationState(target.Faction.Id) != RelationState.Hostile)
                {
                    return; //TODO отправлять ошибку?
                }
            }
            else if (Template.TargetType == SkillTargetType.AnyUnit)
            {
                if (targetType != null && (targetType.Type == SkillCastTargetType.Unit || targetType.Type == SkillCastTargetType.Doodad))
                {
                    target = targetType.ObjId > 0 ? WorldManager.Instance.GetBaseUnit(targetType.ObjId) : caster;
                    if (target != null)
                    {
                        targetType.ObjId = target.ObjId;
                    }
                }
                else
                {
                    // TODO ...
                }
            }
            else if (Template.TargetType == SkillTargetType.Doodad)
            {
                if (targetType != null && (targetType.Type == SkillCastTargetType.Unit || targetType.Type == SkillCastTargetType.Doodad))
                {
                    target = targetType.ObjId > 0 ? WorldManager.Instance.GetBaseUnit(targetType.ObjId) : caster;
                    if (target != null)
                    {
                        targetType.ObjId = target.ObjId;
                    }
                }
                else
                {
                    // TODO ...
                }
            }
            else if (Template.TargetType == SkillTargetType.Item)
            {
                // TODO ...
            }
            else if (Template.TargetType == SkillTargetType.Others)
            {
                if (targetType != null && (targetType.Type == SkillCastTargetType.Unit || targetType.Type == SkillCastTargetType.Doodad))
                {
                    target = targetType.ObjId > 0 ? WorldManager.Instance.GetBaseUnit(targetType.ObjId) : caster;
                    if (target != null)
                    {
                        targetType.ObjId = target.ObjId;
                    }
                }
                else
                {
                    // TODO ...
                }

                if (target != null && caster.ObjId == target.ObjId)
                {
                    return; //TODO отправлять ошибку?
                }
            }
            else if (Template.TargetType == SkillTargetType.FriendlyOthers)
            {
                if (targetType != null && (targetType.Type == SkillCastTargetType.Unit || targetType.Type == SkillCastTargetType.Doodad))
                {
                    target = targetType.ObjId > 0 ? WorldManager.Instance.GetBaseUnit(targetType.ObjId) : caster;
                    if (target != null)
                    {
                        targetType.ObjId = target.ObjId;
                    }
                }
                else
                {
                    // TODO ...
                }

                if (target != null && caster.ObjId == target.ObjId)
                {
                    return; //TODO отправлять ошибку?
                }
                if (target != null && caster.Faction.GetRelationState(target.Faction.Id) != RelationState.Friendly)
                {
                    return; //TODO отправлять ошибку?
                }
            }
            else if (Template.TargetType == SkillTargetType.GeneralUnit)
            {
                if (targetType != null && (targetType.Type == SkillCastTargetType.Unit || targetType.Type == SkillCastTargetType.Doodad))
                {
                    target = targetType.ObjId > 0 ? WorldManager.Instance.GetBaseUnit(targetType.ObjId) : caster;
                    if (target != null)
                    {
                        targetType.ObjId = target.ObjId;
                    }
                }
                else
                {
                    // TODO ...
                }

                if (target != null && caster.ObjId == target.ObjId)
                {
                    return; //TODO отправлять ошибку?
                }
            }
            else if (Template.TargetType == SkillTargetType.Pos)
            {
                var positionTarget = (SkillCastPositionTarget)targetType;
                var positionUnit = new BaseUnit();
                positionUnit.Position = new Point(positionTarget.PosX, positionTarget.PosY, positionTarget.PosZ);
                positionUnit.Position.ZoneId = caster.Position.ZoneId;
                positionUnit.Position.WorldId = caster.Position.WorldId;
                positionUnit.Region = caster.Region;
                target = positionUnit;
            }
            else
            {
                // TODO ...
            }

            TlId = (ushort)TlIdManager.Instance.GetNextId();

            if (Template.Plot != null)
            {
                var eventTemplate = Template.Plot.EventTemplate;
                var step = new PlotStep();
                step.Event = eventTemplate;
                step.Flag = 2;

                if (!eventTemplate.СheckСonditions(caster, casterType, target, targetType, skillObject))
                {
                    step.Flag = 0;
                }

                var res = true;
                if (step.Flag != 0)
                {
                    var callCounter = new Dictionary<uint, int>();
                    callCounter.Add(step.Event.Id, 1);
                    foreach (var evnt in eventTemplate.NextEvents)
                    {
                        res = res && BuildPlot(caster, casterType, target, targetType, skillObject, evnt, step, callCounter);
                    }
                }
                ParsePlot(caster, casterType, target, targetType, skillObject, step);
            }

            if (Template.CastingTime > 0)
            {
                caster.BroadcastPacket(new SCSkillStartedPacket(TemplateId, TlId, casterType, targetType, this, skillObject), true);
                caster.SkillTask = new CastTask(this, caster, casterType, target, targetType, skillObject);
                TaskManager.Instance.Schedule(caster.SkillTask, TimeSpan.FromMilliseconds(Template.CastingTime));
            }
            else if (caster is Character && (TemplateId == 2 || TemplateId == 3 || TemplateId == 4) && !caster.IsAutoAttack)
            {
                caster.IsAutoAttack = true; // enable auto attack
                caster.SkillId = TemplateId;
                caster.TlId = TlId;
                caster.BroadcastPacket(new SCSkillStartedPacket(TemplateId, TlId, casterType, targetType, this, skillObject), true);

                caster.AutoAttackTask = new MeleeCastTask(this, caster, casterType, target, targetType, skillObject);
                TaskManager.Instance.Schedule(caster.AutoAttackTask, TimeSpan.FromMilliseconds(300), TimeSpan.FromMilliseconds(1300));
            }
            else
            {
                Cast(caster, casterType, target, targetType, skillObject);
            }
        }

        public bool BuildPlot(Unit caster, SkillCaster casterCaster, BaseUnit target, SkillCastTarget targetCaster, SkillObject skillObject, PlotNextEvent nextEvent, PlotStep baseStep, Dictionary<uint, int> counter)
        {
            if (counter.ContainsKey(nextEvent.Event.Id))
            {
                var nextCount = counter[nextEvent.Event.Id] + 1;
                if (nextCount > nextEvent.Event.Tickets)
                {
                    return true;
                }
                counter[nextEvent.Event.Id] = nextCount;
            }
            else
            {
                counter.Add(nextEvent.Event.Id, 1);
            }

            if (nextEvent.Delay > 0)
            {
                baseStep.Delay = nextEvent.Delay;
                caster.SkillTask = new PlotTask(this, caster, casterCaster, target, targetCaster, skillObject, nextEvent, counter);
                TaskManager.Instance.Schedule(caster.SkillTask, TimeSpan.FromMilliseconds(nextEvent.Delay));
                return false;
            }

            if (nextEvent.Speed > 0)
            {
                baseStep.Speed = nextEvent.Speed;
                caster.SkillTask = new PlotTask(this, caster, casterCaster, target, targetCaster, skillObject, nextEvent, counter);
                var dist = MathUtil.CalculateDistance(caster.Position, target.Position, true);
                TaskManager.Instance.Schedule(caster.SkillTask, TimeSpan.FromSeconds(dist / nextEvent.Speed));
                return false;
            }

            var step = new PlotStep();
            step.Event = nextEvent.Event;
            step.Flag = 2;
            step.Casting = nextEvent.Casting;
            step.Channeling = nextEvent.Channeling;
            foreach (var condition in nextEvent.Event.Conditions)
            {
                if (condition.Condition.Check(caster, casterCaster, target, targetCaster, skillObject))
                {
                    continue;
                }
                step.Flag = 0;
                break;
            }

            baseStep.Steps.AddLast(step);
            if (step.Flag == 0)
            {
                return true;
            }
            var res = true;
            foreach (var e in nextEvent.Event.NextEvents)
            {
                res = res && BuildPlot(caster, casterCaster, target, targetCaster, skillObject, e, step, counter);
            }
            return res;
        }

        public void ParsePlot(Unit caster, SkillCaster casterCaster, BaseUnit target, SkillCastTarget targetCaster, SkillObject skillObject, PlotStep step)
        {
            _log.Warn("Plot: StepId {0}, Flag {1}, Delay {2}", step.Event.Id, step.Flag, step.Delay);

            if (step.Flag != 0)
            {
                foreach (var eff in step.Event.Effects)
                {
                    var template = SkillManager.Instance.GetEffectTemplate(eff.ActualId, eff.ActualType);
                    if (template is BuffEffect)
                    {
                        step.Flag = 6;
                    }
                    template.Apply(caster, casterCaster, target, targetCaster, new CastPlot(step.Event.PlotId, TlId, step.Event.Id, Template.Id), this, skillObject, DateTime.Now);
                }
            }

            var time = (ushort)(step.Flag != 0 ? step.Delay / 10 + 1 : 0); // TODO fixed the CSStopCastingPacket spam when using the "Chain Lightning" skill
            var unkId = step.Casting || step.Channeling ? caster.ObjId : 0;
            var casterPlotObj = new PlotObject(caster);
            var targetPlotObj = new PlotObject(target);
            caster.BroadcastPacket(new SCPlotEventPacket(TlId, step.Event.Id, Template.Id, casterPlotObj, targetPlotObj, unkId, time, step.Flag), true);

            foreach (var st in step.Steps)
            {
                ParsePlot(caster, casterCaster, target, targetCaster, skillObject, st);
            }
        }

        public void Cast(Unit caster, SkillCaster casterCaster, BaseUnit target, SkillCastTarget targetCaster, SkillObject skillObject)
        {
            caster.SkillTask = null;

            if (TemplateId == 2 || TemplateId == 3 || TemplateId == 4)
            {
                if (caster is Character && caster.CurrentTarget == null)
                {
                    StopSkill(caster);
                    return;
                }

                // Get a random number (from 0 to n)
                var value = Rand.Next(0, 1);
                // для skillId = 2
                // 87 (35) - удар наотмаш, chr
                //  2 (00) - удар сбоку, NPC
                //  3 (46) - удар сбоку, chr
                //  1 (00) - удар похож на 2 удар сбоку, NPC
                // 91 - удар сверху (немного справа)
                // 92 - удар наотмашь слева вниз направо
                //  0 - удар не наносится (расстояние большое и надо подойти поближе), f=1, c=15
                var effectDelay = new Dictionary<int, short> { { 0, 46 }, { 1, 35 } };
                var fireAnimId = new Dictionary<int, int> { { 0, 3 }, { 1, 87 } };
                var effectDelay2 = new Dictionary<int, short> { { 0, 0 }, { 1, 0 } };
                var fireAnimId2 = new Dictionary<int, int> { { 0, 1 }, { 1, 2 } };

                var trg = (Unit)target;
                var dist = 0f;
                if (trg != null)
                {
                    dist = MathUtil.CalculateDistance(caster.Position, trg.Position, true);
                }

                if (dist >= SkillManager.Instance.GetSkillTemplate(TemplateId).MinRange && dist <= SkillManager.Instance.GetSkillTemplate(TemplateId).MaxRange)
                {
                    caster.BroadcastPacket(caster is Character
                            ? new SCSkillFiredPacket(TemplateId, TlId, casterCaster, targetCaster, this, skillObject, effectDelay[value], fireAnimId[value])
                            : new SCSkillFiredPacket(TemplateId, TlId, casterCaster, targetCaster, this, skillObject, effectDelay2[value], fireAnimId2[value]),
                        true);
                }
                else
                {
                    caster.BroadcastPacket(caster is Character
                            ? new SCSkillFiredPacket(TemplateId, TlId, casterCaster, targetCaster, this, skillObject, effectDelay[value], fireAnimId[value], false)
                            : new SCSkillFiredPacket(TemplateId, TlId, casterCaster, targetCaster, this, skillObject, effectDelay2[value], fireAnimId2[value], false),
                        true);

                    if (caster is Character chr)
                    {
                        chr.SendMessage("Target is too far ...");
                    }
                    return;
                }
            }
            else
            {
                caster.BroadcastPacket(new SCSkillFiredPacket(TemplateId, TlId, casterCaster, targetCaster, this, skillObject), true);
            }

            if (Template.ChannelingTime > 0)
            {
                if (Template.ChannelingBuffId != 0)
                {
                    var buff = SkillManager.Instance.GetBuffTemplate(Template.ChannelingBuffId);
                    buff.Apply(caster, casterCaster, target, targetCaster, new CastSkill(Template.Id, TlId), this, skillObject, DateTime.Now);
                }

                caster.SkillTask = new ChannelingTask(this, caster, casterCaster, target, targetCaster, skillObject);
                TaskManager.Instance.Schedule(caster.SkillTask, TimeSpan.FromMilliseconds(Template.ChannelingTime));
            }
            else
            {
                Channeling(caster, casterCaster, target, targetCaster, skillObject);
            }
        }

        public async void StopSkill(Unit caster)
        {
            await caster.AutoAttackTask.Cancel();
            caster.BroadcastPacket(new SCSkillEndedPacket(TlId), true);
            caster.BroadcastPacket(new SCSkillStoppedPacket(caster.ObjId, TemplateId), true);
            caster.AutoAttackTask = null;
            caster.IsAutoAttack = false; // turned off auto attack
            TlIdManager.Instance.ReleaseId(TlId);
        }

        public void Channeling(Unit caster, SkillCaster casterCaster, BaseUnit target, SkillCastTarget targetCaster, SkillObject skillObject)
        {
            caster.SkillTask = null;
            if (Template.ChannelingBuffId != 0)
            {
                caster.Effects.RemoveEffect(Template.ChannelingBuffId, Template.Id);
            }
            if (Template.ToggleBuffId != 0)
            {
                var buff = SkillManager.Instance.GetBuffTemplate(Template.ToggleBuffId);
                buff.Apply(caster, casterCaster, target, targetCaster, new CastSkill(Template.Id, TlId), this, skillObject, DateTime.Now);
            }

            if (Template.EffectDelay > 0)
            {
                var totalDelay = Template.EffectDelay;
                if (Template.MatchAnimation)
                {
                    totalDelay += Template.FireAnim.Duration;
                }
                else if (Template.UseAnimTime)
                {
                    totalDelay += Template.FireAnim.CombatSyncTime;
                }

                TaskManager.Instance.Schedule(new ApplySkillTask(this, caster, casterCaster, target, targetCaster, skillObject), TimeSpan.FromMilliseconds(totalDelay));
            }
            else
            {
                var totalDelay = 0;
                if ((Template.MatchAnimation || Template.UseAnimTime) && Template.FireAnim != null)
                {
                    if (Template.MatchAnimation)
                    {
                        totalDelay += Template.FireAnim.CombatSyncTime;
                    }

                    if (Template.UseAnimTime)
                    {
                        totalDelay += Template.FireAnim.Duration;
                    }

                    TaskManager.Instance.Schedule(new ApplySkillTask(this, caster, casterCaster, target, targetCaster, skillObject), TimeSpan.FromMilliseconds(totalDelay));
                }
                else
                {
                    Apply(caster, casterCaster, target, targetCaster, skillObject);
                }
            }
        }

        public void Apply(Unit caster, SkillCaster casterCaster, BaseUnit targetSelf, SkillCastTarget targetCaster, SkillObject skillObject)
        {
            var effectsToApply = new List<EffectToApply>();
            foreach (var effect in Template.Effects)
            {
                var targets = GetTargets(caster, targetCaster, Template.TargetType, (SkillEffectApplicationMethod)effect.ApplicationMethodId);

                _log.Warn(effect.Template?.ToString());

                foreach (var target in targets)
                {
                    if (effect.StartLevel > caster.Level || effect.EndLevel < caster.Level)
                    {
                        continue;
                    }

                    if (effect.Friendly && !effect.NonFriendly && caster.Faction.GetRelationState(target.Faction.Id) != RelationState.Friendly)
                    {
                        continue;
                    }

                    if (!effect.Friendly && effect.NonFriendly && caster.Faction.GetRelationState(target.Faction.Id) != RelationState.Hostile)
                    {
                        continue;
                    }

                    if (effect.Front && !effect.Back && !MathUtil.IsFront(caster, target))
                    {
                        continue;
                    }

                    if (!effect.Front && effect.Back && MathUtil.IsFront(caster, target))
                    {
                        continue;
                    }

                    if (effect.SourceBuffTagId > 0 && !caster.Effects.CheckBuffs(SkillManager.Instance.GetBuffsByTagId(effect.SourceBuffTagId)))
                    {
                        continue;
                    }

                    if (effect.SourceNoBuffTagId > 0 && caster.Effects.CheckBuffs(SkillManager.Instance.GetBuffsByTagId(effect.SourceNoBuffTagId)))
                    {
                        continue;
                    }

                    if (effect.TargetBuffTagId > 0 && !target.Effects.CheckBuffs(SkillManager.Instance.GetBuffsByTagId(effect.TargetBuffTagId)))
                    {
                        continue;
                    }

                    if (effect.TargetNoBuffTagId > 0 && target.Effects.CheckBuffs(SkillManager.Instance.GetBuffsByTagId(effect.TargetNoBuffTagId)))
                    {
                        continue;
                    }

                    if (effect.Chance < 100 && Rand.Next(100) > effect.Chance)
                    {
                        continue;
                    }
                    if (casterCaster is CasterEffectBuff castItem) // TODO Clean up. 
                    {
                        var castItemTemplate = ItemManager.Instance.GetTemplate(castItem.ItemTemplateId);
                        if ((castItemTemplate.UseSkillAsReagent) && (caster is Character player))
                        {
                            player.Inventory.Bag.ConsumeItem(ItemTaskType.SkillReagents, castItemTemplate.Id, effect.ConsumeItemCount, null);
                        }
                        /*
                        var itemUsed = ItemManager.Instance.Create(castItem.ItemTemplateId, 1, 1, true);
                        var isRaegent = itemUsed.Template.UseSkillAsReagent;
                        if (isRaegent) //if item is a raegent
                        {
                           if (caster is Character player)
                           {
                               var items = player.Inventory.RemoveItem(castItem.ItemTemplateId, effect.ConsumeItemCount);
                               var tasks = new List<ItemTask>();
                               foreach (var (item, count) in items)
                               {
                                   InventoryHelper.RemoveItemAndUpdateClient(player, item, count, ItemTaskType.SkillReagents);
                               }
                           }
                        }
                        ItemManager.Instance.ReleaseId(itemUsed.Id);
                        */
                    }
                    if (caster is Character character && effect.ConsumeItemId != 0 && effect.ConsumeItemCount > 0)
                    {
                        if (effect.ConsumeSourceItem)
                        {
                            if (!character.Inventory.Bag.AcquireDefaultItem(ItemTaskType.SkillEffectConsumption,
                                effect.ConsumeItemId, effect.ConsumeItemCount))
                            {
                                continue;
                            }
                        }
                        else
                        {
                            var inventory = character.Inventory.CheckItems(SlotType.Inventory, effect.ConsumeItemId, effect.ConsumeItemCount);
                            var equipment = character.Inventory.CheckItems(SlotType.Equipment, effect.ConsumeItemId, effect.ConsumeItemCount);
                            if (!(inventory || equipment))
                            {
                                continue;
                            }

                            if (inventory)
                            {
                                character.Inventory.Bag.ConsumeItem(ItemTaskType.SkillEffectConsumption, effect.ConsumeItemId, effect.ConsumeItemCount, null);
                            }
                            else
                            if (equipment)
                            {
                                character.Inventory.Equipment.ConsumeItem(ItemTaskType.SkillEffectConsumption, effect.ConsumeItemId, effect.ConsumeItemCount, null);
                            }
                        }
                    }

                    //effect.Template?.Apply(caster, casterType, target, targetType, new CastSkill(Template.Id, TlId), this, skillObject, DateTime.Now);
                    if (effect.Template != null)
                    {
                        var effectToApply = new EffectToApply(effect.Template, caster, casterCaster, target, targetCaster, new CastSkill(Template.Id, TlId), this, skillObject);
                        effectsToApply.Add(effectToApply);
                    }
                }
            }
            foreach (var effect in effectsToApply)
            {
                effect.Apply();
            }

            if (Template.ConsumeLaborPower > 0 && caster is Character chr)
            {
                chr.ChangeLabor((short)-Template.ConsumeLaborPower, Template.ActabilityGroupId);
            }

            if (Template.ConsumeLaborPower > 0 && caster is Character chart)
            {
                chart.ChangeLabor((short)-Template.ConsumeLaborPower, Template.ActabilityGroupId);
            }

            caster.BroadcastPacket(new SCSkillEndedPacket(TlId), true);
            TlIdManager.Instance.ReleaseId(TlId);
            //TlId = 0;

            if (Template.CastingTime > 0)
            {
                caster.BroadcastPacket(new SCSkillStoppedPacket(caster.ObjId, Template.Id), true);
            }
        }

        public List<BaseUnit> GetTargets(Unit caster, SkillCastTarget castTarget, SkillTargetType targetType, SkillEffectApplicationMethod effectApplicationMethod)
        {
            var targets = new List<BaseUnit>();
            var origin = (BaseUnit)caster;

            // TODO : Check SkillTargetType

            /*
                SkillTargetSelection : Seems to pick the "Origin" of the effect.
                This is general to the entire skill.
            */

            switch (Template.TargetSelection)
            {
                case SkillTargetSelection.Source:
                    origin = caster;
                    break;
                case SkillTargetSelection.Target:
                    //origin = castTarget.GetBaseUnit();
                    if (castTarget is SkillCastUnitTarget unitTarget)
                    {
                        origin = WorldManager.Instance.GetBaseUnit(unitTarget.ObjId);
                    }
                    else if (castTarget is SkillCastPositionTarget posTarget)
                    {
                        origin = new BaseUnit
                        {
                            Position = new Point(posTarget.PosX, posTarget.PosY, posTarget.PosZ)
                            {
                                ZoneId = caster.Position.ZoneId,
                                WorldId = caster.Position.WorldId
                            },
                            Region = caster.Region,
                            Faction = caster.Faction
                        };
                    }
                    else if (castTarget is SkillCastDoodadTarget doodadTarget)
                    {
                        origin = WorldManager.Instance.GetDoodad(doodadTarget.ObjId);
                    }
                    else
                    {
                        // TODO : There has to be a better way... god save my soul
                        origin = caster.CurrentTarget;
                    }

                    break;
                case SkillTargetSelection.Location:
                    var positionTarget = (SkillCastPositionTarget)castTarget;
                    origin = new BaseUnit
                    {
                        Position = new Point(positionTarget.PosX, positionTarget.PosY, positionTarget.PosZ)
                        {
                            ZoneId = caster.Position.ZoneId,
                            WorldId = caster.Position.WorldId
                        },
                        Region = caster.Region,
                        Faction = caster.Faction
                    };
                    break;
                case SkillTargetSelection.Line:
                    // TODO : Impl
                    break;
            }

            switch (effectApplicationMethod)
            {
                case SkillEffectApplicationMethod.SourceToSource:
                    // Get self but more than once ? Idk man
                    targets.Add(caster);
                    break;
                case SkillEffectApplicationMethod.SourceToPos:
                // GetAOETargets around the caster's target area
                case SkillEffectApplicationMethod.SourceToTarget:
                    // GetAOETargets around the caster's target
                    if (Template.TargetAreaRadius > 0)
                    {
                        // AREA AROUND TARGET (will hit caster because target objId is excluded, not Caster's)
                        var obj = WorldManager.Instance.GetAround<BaseUnit>(origin, Template.TargetAreaRadius, Template.TargetAreaCount);
                        obj = obj.Where(other => caster.GetRelationTo(other) == Template.TargetRelation).ToList();
                        targets.AddRange(obj);
                    }
                    else
                    {
                        targets.Add(origin);
                    }

                    break;
                case SkillEffectApplicationMethod.SourceToSourceOnce:
                    // Seems like that's the idea lol
                    targets.Add(caster);
                    break;
            }

            return targets;
        }

        public void Stop(Unit caster)
        {
            if (Template.ChannelingBuffId != 0)
            {
                caster.Effects.RemoveEffect(Template.ChannelingBuffId, Template.Id);
            }

            if (Template.ToggleBuffId != 0)
            {
                caster.Effects.RemoveEffect(Template.ToggleBuffId, Template.Id);
            }
            caster.BroadcastPacket(new SCCastingStoppedPacket(TlId, 0), true);
            caster.BroadcastPacket(new SCSkillEndedPacket(TlId), true);
            caster.SkillTask = null;
            TlIdManager.Instance.ReleaseId(TlId);
            //TlId = 0;
        }
    }
}
