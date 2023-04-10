using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    [Serializable]
    public class Recipe : ASerializable
    {
        public Item item = Item.Axe;
        public int recipeSize = 3;
        public List<Metal> metalRecipe = new List<Metal>();
        public override void Deserialize(Packet pPacket)
        {
            item = (Item)pPacket.ReadInt();
            recipeSize = pPacket.ReadInt();
            for (int i = 0; i < recipeSize; i++)
            {
                metalRecipe.Add((Metal)pPacket.ReadInt());
            }
        }

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write((int)item);
            pPacket.Write(recipeSize);
            for (int i = 0; i < recipeSize; i++)
            {
                pPacket.Write((int)metalRecipe[i]);
            }
        }
    }
}
