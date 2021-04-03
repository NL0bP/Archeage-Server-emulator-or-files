using AAEmu.Commons.Network;
using AAEmu.Commons.Utils;
using AAEmu.Game.Core.Network.Stream;
using AAEmu.Game.Models.Game.DoodadObj;

namespace AAEmu.Game.Core.Packets.S2C
{
    public class TCDoodadStreamPacket : StreamPacket
    {
        private readonly int _id;
        private readonly int _next;
        private readonly Doodad[] _doodads;

        public TCDoodadStreamPacket(int id, int next, Doodad[] doodads) : base(0x02)
        {
            _id = id;
            _next = next;
            _doodads = doodads;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_id);             // id
            stream.Write(_next);           // next
            stream.Write(_doodads.Length); // count
            foreach (var doodad in _doodads)
            {
                stream.WriteBc(doodad.ObjId);    // bc
                stream.Write(doodad.TemplateId); // type
                stream.WritePositionBc(doodad.Position.X, doodad.Position.Y, doodad.Position.Z); // pos_bc_xyz
                stream.Write(Helpers.ConvertRotation(doodad.Position.RotationX)); // rotx
                stream.Write(Helpers.ConvertRotation(doodad.Position.RotationY)); // roty
                stream.Write(Helpers.ConvertRotation(doodad.Position.RotationZ)); // rotz
                stream.Write(doodad.Scale);       //scale
                stream.Write(doodad.FuncGroupId); // doodad_func_groups Id type
            }

            return stream;
        }
    }
}
