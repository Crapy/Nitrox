using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using HarmonyLib;
using NitroxClient.Communication.Abstract;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.DataStructures;
using NitroxModel.Helper;
using NitroxModel_Subnautica.DataStructures.GameLogic;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic;

public class ToggleLights_SetLightsActive_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo targetMethod = Reflect.Method((ToggleLights t) => t.SetLightsActive(default));

    private static readonly HashSet<Type> syncedParents = new()
    {
        typeof(SeaMoth),
        typeof(Seaglide),
        typeof(FlashLight),
        typeof(LEDLight) // LEDLight uses ToggleLights, but does not provide a method to toggle them.
    };

    public static void Prefix(ToggleLights __instance, out bool __state)
    {
        __state = __instance.lightsActive;
    }

    public static void Postfix(ToggleLights __instance, bool __state)
    {
        if (__state != __instance.lightsActive)
        {
            // Find the right gameobject in the hierarchy to sync on:
            GameObject gameObject = null;
            Type type = null;
                
            foreach (Type t in syncedParents)
            {
                if (__instance.GetComponent(t))
                {
                    type = t;
                    gameObject = __instance.gameObject;
                    break;
                }
                if (__instance.transform.parent.GetComponent(t))
                {
                    type = t;
                    gameObject = __instance.transform.parent.gameObject;
                    break;
                }
            }

            if (!gameObject)
            {
                Log.Error("Couldn't find any valid component for sending a ToggleLights packet.");
            }

            NitroxId id = NitroxEntity.GetId(gameObject);
            // If the floodlight belongs to a seamoth, then set the lights for the model
            if (type == typeof(SeaMoth))
            {
                Resolve<Vehicles>().GetVehicles<SeamothModel>(id).LightOn = __instance.lightsActive;
            }
            Resolve<IPacketSender>().Send(new NitroxModel.Packets.ToggleLights(id, __instance.lightsActive));
        }
    }

    public override void Patch(Harmony harmony)
    {
        PatchMultiple(harmony, targetMethod, prefix:true, postfix:true);
    }
}
