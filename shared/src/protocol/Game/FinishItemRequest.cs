using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    public class FinishItemRequest : ASerializable
    {
        public Recipe recipe;
        public int grade;
        public override void Deserialize(Packet pPacket)
        {
            recipe = pPacket.Read<Recipe>();
            grade = pPacket.ReadInt();
        }

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(recipe);   
            pPacket.Write(grade);
        }
    }
}
