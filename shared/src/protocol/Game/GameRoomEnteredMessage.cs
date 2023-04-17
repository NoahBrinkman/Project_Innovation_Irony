using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    public class GameRoomEnteredMessage : ASerializable
    {
        public PlayerInfo player1;
        public PlayerInfo player2;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(player1);
            pPacket.Write(player2);
        }
        public override void Deserialize(Packet pPacket)
        {
            player1 = pPacket.Read<PlayerInfo>();
            player2 = pPacket.Read<PlayerInfo>();
        }
    }
}
