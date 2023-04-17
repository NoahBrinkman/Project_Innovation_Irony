using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    public class RoomGrade : ASerializable
    {
        public int grade;
        public MinigameRoom room;

        public override void Deserialize(Packet pPacket)
        {
            grade = pPacket.ReadInt();
            room = (MinigameRoom)pPacket.ReadInt();
        }

        public override void Serialize(Packet pPacket)
        {
           pPacket.Write(grade);
           pPacket.Write((int)room);
        }
    }
}
