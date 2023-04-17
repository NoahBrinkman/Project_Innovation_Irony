using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    public class GameFinishedResult : ASerializable
    {
        public PlayerInfo winner;


        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(winner);
        }
        public override void Deserialize(Packet pPacket)
        {
            winner = pPacket.Read<PlayerInfo>();
        }
    }
}
