namespace shared
{
    /**
     * Send from CLIENT to SERVER to request joining the server.
     */
    public class PlayerJoinRequest : ASerializable
    {
        public string passCode;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(passCode);
        }

        public override void Deserialize(Packet pPacket)
        {
            passCode = pPacket.ReadString();
        }
    }
}
