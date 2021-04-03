﻿using System;

using AAEmu.Commons.Network;
using AAEmu.Commons.Utils;
using AAEmu.Game.Core.Managers.World;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.DoodadObj.Templates;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Models.Game.World;
using AAEmu.Game.Models.Tasks.Doodads;

using NLog;

namespace AAEmu.Game.Models.Game.DoodadObj
{
    public class Doodad : BaseUnit
    {
        private float _scale;

        private static Logger _log = LogManager.GetCurrentClassLogger();
        
        public byte Flag { get; set; }
        public uint TemplateId { get; set; }
        public DoodadTemplate Template { get; set; }
        public override float Scale => _scale;
        public uint FuncGroupId { get; set; }
        public string FuncType { get; set; }
        public ulong ItemId { get; set; }
        public DateTime GrowthTime { get; set; }
        public DateTime PlantTime { get; set; }
        public uint OwnerId { get; set; }
        public uint OwnerObjId { get; set; }
        public uint ParentObjId { get; set; }
        public DoodadOwnerType OwnerType { get; set; }
        public byte AttachPoint { get; set; }
        public uint DbHouseId { get; set; }
        public int Data { get; set; }
        public uint QuestGlow { get; set; } //0 off // 1 on
        public DoodadSpawner Spawner { get; set; }
        public DoodadFuncTask FuncTask { get; set; }
        public DateTime FreshnessTime { get; set; }
        public bool IsRebuy { get; set; }

        public uint TimeLeft => GrowthTime > DateTime.Now ? (uint)(GrowthTime - DateTime.Now).TotalMilliseconds : 0; // TODO formula time of phase
        public bool cancelPhasing { get; set; }

        public Doodad()
        {
            _scale = 1f;
            Position = new Point();
            WorldPos = new WorldPos();
            PlantTime = DateTime.MinValue;
            AttachPoint = 255;
        }

        public void SetScale(float scale)
        {
            _scale = scale;
        }

        public uint GetFuncGroupId()
        {
            foreach (var funcGroup in Template.FuncGroups)
            {
                if (funcGroup.GroupKindId == DoodadFuncGroups.DoodadFuncGroupKind.Start)
                    return funcGroup.Id;
            }
            return 0;
        }

        public override void BroadcastPacket(GamePacket packet, bool self)
        {
            foreach (var character in WorldManager.Instance.GetAround<Character>(this))
                character.SendPacket(packet);
        }

        public override void AddVisibleObject(Character character)
        {
            character.SendPacket(new SCDoodadCreatedPacket(this));
        }

        public override void RemoveVisibleObject(Character character)
        {
            if (character.CurrentTarget != null && character.CurrentTarget == this)
            {
                character.CurrentTarget = null;
                character.SendPacket(new SCTargetChangedPacket(character.ObjId, 0));
            }
            character.SendPacket(new SCDoodadRemovedPacket(ObjId));
        }

        public PacketStream Write(PacketStream stream)
        {
            stream.WriteBc(ObjId);

            stream.WritePisc(TemplateId, FuncGroupId, 0, 0);

            stream.Write(Flag);
            stream.WriteBc(OwnerObjId);
            stream.WriteBc(ParentObjId);
            stream.Write(AttachPoint); // attachPoint

            stream.WritePositionBc(Position.X, Position.Y, Position.Z);

            stream.Write(Helpers.ConvertRotation(Position.RotationX));
            stream.Write(Helpers.ConvertRotation(Position.RotationY));
            stream.Write(Helpers.ConvertRotation(Position.RotationZ));

            stream.Write(Scale);
            stream.Write(OwnerId);         // characterId
            stream.Write(ItemId);          // type(id)
            stream.Write(0u);              // type(id)
            stream.Write(TimeLeft);        // growing
            stream.Write(PlantTime);       // plantTime
            stream.Write(0);               // family
            stream.Write(-1);              // puzzleGroup
            stream.Write((byte)OwnerType); // ownerType
            stream.Write(DbHouseId);       // dbHouseId
            stream.Write(Data);            // data

            return stream;
        }
    }
}
