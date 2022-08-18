using System;
using System.Collections.Generic;

namespace NitroxClient.Communication;

public class PacketSuppressor<T> : IDisposable
{
    private readonly HashSet<Type> suppressedPacketTypes;

    public PacketSuppressor(HashSet<Type> suppressedPacketTypes)
    {
        this.suppressedPacketTypes = suppressedPacketTypes;
        suppressedPacketTypes.Add(typeof(T));
    }

    public void Dispose()
    {
        suppressedPacketTypes.Remove(typeof(T));
    }
}

public class PacketSuppressor : IDisposable
{
    private readonly HashSet<Type> suppressedPacketTypes;
    private readonly Type[] packetsToSuppress;

    public PacketSuppressor(HashSet<Type> suppressedPacketTypes, Type[] packetsToSuppress)
    {
        this.suppressedPacketTypes = suppressedPacketTypes;
        this.packetsToSuppress = packetsToSuppress;
        suppressedPacketTypes.AddRange(packetsToSuppress);
    }

    public void Dispose()
    {
        suppressedPacketTypes.RemoveRange(packetsToSuppress);
    }
}
