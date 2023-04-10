using System.Collections.Generic;

namespace shared
{
	/**
	 * Send from SERVER to CLIENT to notify that the client has joined a specific room (i.e. that it should change state).
	 */
	public class RoomJoinedEvent : ASerializable
	{
		public enum Room { LOGIN_ROOM, LOBBY_ROOM, GAME_ROOM };
		public Room room;
		
		public bool miningGameChosen = false;
		public bool cleaningGameChosen = false;
		public bool smeltingGameChosen = false;
		public bool castingGameChosen = false;

		public override void Serialize(Packet pPacket)
		{
			pPacket.Write((int)room);
            pPacket.Write(miningGameChosen);
            pPacket.Write(cleaningGameChosen);
            pPacket.Write(smeltingGameChosen);
            pPacket.Write(castingGameChosen);
        }

		public override void Deserialize(Packet pPacket)
		{
			room = (Room)pPacket.ReadInt();
            miningGameChosen = pPacket.ReadBool();
			cleaningGameChosen = pPacket.ReadBool();
			smeltingGameChosen = pPacket.ReadBool();
			castingGameChosen = pPacket.ReadBool();
		}
	}
}
