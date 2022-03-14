using System.Reflection;
using HarmonyLib;
using NitroxClient.GameLogic.FMOD;
using NitroxModel.Core;
using NitroxModel.Helper;
using NitroxModel_Subnautica.DataStructures;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic;

public class FMODUWE_PlayOneShotImpl_Patch : NitroxPatch, IDynamicPatch
{
    private static FMODSystem fmodSystem;

    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => FMODUWE.PlayOneShotImpl(default, default, default));

    public static bool Prefix()
    {
        return !FMODSuppressor.SuppressFMODEvents;
    }

    public static void Postfix(string eventPath, Vector3 position, float volume)
    {
        if (fmodSystem.IsWhitelisted(eventPath))
        {
            fmodSystem.PlayAsset(eventPath, position.ToDto(), volume);
        }
    }

    public override void Patch(Harmony harmony)
    {
        fmodSystem = NitroxServiceLocator.LocateService<FMODSystem>();
        PatchMultiple(harmony, TARGET_METHOD, prefix:true, postfix:true);
    }
}
