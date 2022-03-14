using NitroxClient.Communication.Abstract;
using NitroxModel;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.Unity;
using NitroxModel.GameLogic.FMOD;
using NitroxModel.Packets;

namespace NitroxClient.GameLogic.FMOD;

public class FMODSystem : FMODWhitelist
{
    private readonly IPacketSender packetSender;

    public FMODSystem(IPacketSender packetSender) : base(GameInfo.Subnautica.Name)
    {
        this.packetSender = packetSender;
    }

    public static FMODSuppressor SuppressSounds()
    {
        return new FMODSuppressor();
    }

    public void PlayAsset(string path, NitroxVector3 position, float volume)
    {
        packetSender.Send(new FMODAssetPacket(path, position, volume));
    }

    public void PlayCustomEmitter(NitroxId id, string assetPath, bool play)
    {
        packetSender.Send(new FMODCustomEmitterPacket(id, assetPath, play));
    }

    public void PlayCustomLoopingEmitter(NitroxId id, string assetPath)
    {
        packetSender.Send(new FMODCustomLoopingEmitterPacket(id, assetPath));
    }

    public void PlayStudioEmitter(NitroxId id, string assetPath, bool play, bool allowFadeout)
    {
        packetSender.Send(new FMODStudioEmitterPacket(id, assetPath, play, allowFadeout));
    }

    public void PlayEventInstance(NitroxId id, string assetPath, bool play, NitroxVector3 position, float volume)
    {
        packetSender.Send(new FMODEventInstancePacket(id, play, assetPath, position, volume));
    }
}
