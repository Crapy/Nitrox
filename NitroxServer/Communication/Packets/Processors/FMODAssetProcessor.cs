using NitroxModel.DataStructures.Unity;
using NitroxModel.Helper;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;

namespace NitroxServer.Communication.Packets.Processors;

public class FMODAssetProcessor : AuthenticatedPacketProcessor<FMODAssetPacket>
{
    private readonly PlayerManager playerManager;

    public FMODAssetProcessor(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public override void Process(FMODAssetPacket packet, Player sendingPlayer)
    {
        foreach (Player player in playerManager.GetConnectedPlayers())
        {
            float distance = NitroxVector3.Distance(player.Position, packet.Position);
            if (player != sendingPlayer && (packet.IsGlobal || player.SubRootId.Equals(sendingPlayer.SubRootId)) && distance <= packet.Radius)
            {
                packet.Volume = SoundHelper.CalculateVolume(distance, packet.Radius, packet.Volume);
                player.SendPacket(packet);
            }
        }
    }
}
