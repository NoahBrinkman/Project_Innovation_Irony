namespace shared
{
    /**
     * Send from CLIENT to SERVER to request enabling/disabling the ready status.
     */
    public class MinigameChosenResponse : ASerializable
    {
        public bool accepted;
        public MinigameRoom chosenRoom; 

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write((int)chosenRoom);
            pPacket.Write(accepted);
        }

        public override void Deserialize(Packet pPacket)
        {
            chosenRoom = (MinigameRoom)pPacket.ReadInt();  
            accepted = pPacket.ReadBool();
        }
    }
}
