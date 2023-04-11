namespace shared
{
	/**
	 * Send from SERVER to CLIENT to notify that the client has joined a specific room (i.e. that it should change state).
	 */
	public class MinigameChosenEvent : ASerializable
	{

		public MinigameRoom room;

		public override void Serialize(Packet pPacket)
		{
			pPacket.Write((int)room);
		}

		public override void Deserialize(Packet pPacket)
		{
			room = (MinigameRoom)pPacket.ReadInt();
		}
	}
}
