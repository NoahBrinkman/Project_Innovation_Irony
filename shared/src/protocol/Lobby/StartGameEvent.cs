namespace shared
{
    /**
     * Send from CLIENT to SERVER to request enabling/disabling the ready status.
     */
    public class StartGameEvent : ASerializable
    {
        public Recipe startRecipe;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(startRecipe);
        }

        public override void Deserialize(Packet pPacket)
        {
              startRecipe = pPacket.Read<Recipe>();
        }
    }
}
