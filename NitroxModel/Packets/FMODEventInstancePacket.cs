using System;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.Unity;

namespace NitroxModel.Packets;

[Serializable]
public class FMODEventInstancePacket : FMODAssetPacket
{
    public NitroxId Id { get; }
    public bool Play { get; }

    public FMODEventInstancePacket(NitroxId id, string assetPath, bool play, NitroxVector3 position, float volume, float radius, bool isGlobal) : base(assetPath, position, volume, radius, isGlobal)
    {
        Id = id;
        Play = play;
    }
}
