using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    public class EndGameEvent : ASerializable
    {
        public int size;
        public List<RoomGrade> grades = new List<RoomGrade>();

        public override void Deserialize(Packet pPacket)
        {
           size = pPacket.ReadInt();
            grades = new List<RoomGrade>();
            for (int i = 0; i < size; i++)
            {
                grades.Add(pPacket.Read<RoomGrade>());
            }
        }

        public override void Serialize(Packet pPacket)
        {
           pPacket.Write(size);
           
            for (int i = 0; i < size; i++)
            {
                pPacket.Write(grades[i]);
            }
        }
    }
}
