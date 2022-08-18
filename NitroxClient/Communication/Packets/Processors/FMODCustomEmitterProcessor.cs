using NitroxClient.Communication.Abstract;
using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.MonoBehaviours;
using NitroxClient.Unity.Helper;
using NitroxModel.Packets;
using UnityEngine;

namespace NitroxClient.Communication.Packets.Processors;

public class FMODCustomEmitterProcessor : ClientPacketProcessor<FMODCustomEmitterPacket>
{
    private readonly IPacketSender packetSender;

    public FMODCustomEmitterProcessor(IPacketSender packetSender)
    {
        this.packetSender = packetSender;
    }


    public override void Process(FMODCustomEmitterPacket packet)
    {
        GameObject soundSource = NitroxEntity.RequireObjectFrom(packet.Id);
        FMODEmitterController fmodEmitterController = soundSource.RequireComponent<FMODEmitterController>();

        using (packetSender.Suppress<FMODCustomEmitterPacket>())
        using (packetSender.Suppress<FMODCustomLoopingEmitterPacket>())
        {
            if (packet.Play)
            {
                fmodEmitterController.PlayCustomEmitter(packet.AssetPath);
            }
            else
            {
                fmodEmitterController.StopCustomEmitter(packet.AssetPath);
            }
        }
    }
}
