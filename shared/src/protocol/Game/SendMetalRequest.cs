using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    public class SendMetalRequest : ASerializable
    {
        public MinigameRoom from;
        public MinigameRoom to;
        public Metal metal;
        public int grade;

        public override void Deserialize(Packet pPacket)
        {
            from = (MinigameRoom)pPacket.ReadInt();
            to = (MinigameRoom)pPacket.ReadInt();
            metal = (Metal)pPacket.ReadInt();
            grade = pPacket.ReadInt(); 
        }

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write((int)from);
            pPacket.Write((int)to);
            pPacket.Write((int)metal);
            pPacket.Write(grade);
            pPacket.Write(grade);
        }
    }
}
