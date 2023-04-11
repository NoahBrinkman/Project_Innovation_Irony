using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    public class FinishItemResponse : ASerializable
    {
        public Recipe recipe;
        public override void Deserialize(Packet pPacket)
        {
            recipe = pPacket.Read<Recipe>();
        }

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(recipe);
        }
    }
}
