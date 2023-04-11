namespace shared
{
    /**
     * Send from CLIENT to SERVER to request enabling/disabling the ready status.
     */
    public class MinigameChosenRequest : ASerializable
    {
        public MinigameRoom chosenRoom; 

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write((int)chosenRoom);
        }

        public override void Deserialize(Packet pPacket)
        {
            chosenRoom = (MinigameRoom)pPacket.ReadInt();   
        }
    }
}
