﻿using System.Reflection;
using HarmonyLib;
using NitroxClient.GameLogic.FMOD;
using NitroxModel.Core;
using NitroxModel.Helper;
using NitroxModel_Subnautica.DataStructures;

namespace NitroxPatcher.Patches.Dynamic;

public class Utils_PlayFMODAsset_Patch : NitroxPatch, IDynamicPatch
{
    private static FMODSystem fmodSystem;

    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => Utils.PlayFMODAsset(default));

    public static bool Prefix()
    {
        return !FMODSuppressor.SuppressFMODEvents;
    }

    public static void Postfix(FMODAsset asset)
    {
        if (fmodSystem.IsWhitelisted(asset.path))
        {
            fmodSystem.PlayAsset(asset.path, Player.main.transform.position.ToDto(), 1f);
        }
    }

    public override void Patch(Harmony harmony)
    {
        fmodSystem = Resolve<FMODSystem>();
        PatchMultiple(harmony, TARGET_METHOD, prefix:true, postfix:true);
    }
}
