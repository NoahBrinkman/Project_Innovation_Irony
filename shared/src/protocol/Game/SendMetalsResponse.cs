using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    public class SendMetalsResponse : ASerializable
    {
        public MinigameRoom from;
        public MinigameRoom to;

        public int size;
        public List<Metal> metals;
        public int grade;

        public override void Deserialize(Packet pPacket)
        {
            from = (MinigameRoom)pPacket.ReadInt();
            to = (MinigameRoom)pPacket.ReadInt();
             size = pPacket.ReadInt(); 
            metals = new List<Metal>();
            for (int i = 0; i < size; i++)
            {
                metals.Add((Metal)pPacket.ReadInt());

            }
            grade = pPacket.ReadInt(); 
        }

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write((int)from);
            pPacket.Write((int)to);
            size = metals.Count;
            pPacket.Write(metals.Count);
            for (int i = 0; i < size; i++)
            {
                pPacket.Write((int)metals[i]);
            }
            pPacket.Write(grade);
        }
    }
}
