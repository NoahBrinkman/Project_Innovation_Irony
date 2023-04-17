using System;

namespace shared
{
    /**
     * Send from CLIENT to SERVER to request joining the server.
     */
    public class HostJoinResponse : ASerializable
    {
        Random random = new Random();
        public string passcode = "";

        public HostJoinResponse()
        {
            for (int i = 0; i < 4; i++)
            {
                char letter = (char)random.Next('A', 'Z' + 1);
                passcode += letter;
            }
        }

     public override void Serialize(Packet pPacket)
        {
            pPacket.Write(passcode);
        }

        public override void Deserialize(Packet pPacket)
        {
            passcode = pPacket.ReadString();
        }
    }
}
