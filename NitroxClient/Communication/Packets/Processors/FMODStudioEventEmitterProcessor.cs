using NitroxClient.Communication.Abstract;
using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.MonoBehaviours;
using NitroxClient.Unity.Helper;
using NitroxModel.Packets;
using UnityEngine;

namespace NitroxClient.Communication.Packets.Processors;

public class FMODStudioEventEmitterProcessor : ClientPacketProcessor<FMODStudioEmitterPacket>
{
    private readonly IPacketSender packetSender;

    public FMODStudioEventEmitterProcessor(IPacketSender packetSender)
    {
        this.packetSender = packetSender;
    }


    public override void Process(FMODStudioEmitterPacket packet)
    {
        GameObject soundSource = NitroxEntity.RequireObjectFrom(packet.Id);
        FMODEmitterController fmodEmitterController = soundSource.RequireComponent<FMODEmitterController>();

        using (packetSender.Suppress<FMODStudioEmitterPacket>())
        {
            if (packet.Play)
            {
                fmodEmitterController.PlayStudioEmitter(packet.AssetPath);
            }
            else
            {
                fmodEmitterController.StopStudioEmitter(packet.AssetPath, packet.AllowFadeout);
            }
        }
    }
}
