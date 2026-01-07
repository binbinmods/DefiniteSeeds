using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
// using static Obeliskial_Essentials.Essentials;
using System;
// using static DefiniteSeeds.CustomFunctions;
using static DefiniteSeeds.Plugin;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Analytics;
using System.Text;
using static DefiniteSeeds.DefiniteSeedsFunctions;

// Make sure your namespace is the same everywhere
namespace DefiniteSeeds
{

    [HarmonyPatch] //DO NOT REMOVE/CHANGE

    public class DefiniteSeedsPatches
    {
        // To create a patch, you need to declare either a prefix or a postfix. 
        // Prefixes are executed before the original code, postfixes are executed after
        // Then you need to tell Harmony which method to patch.

        // public static 

        // #pragma warning disable Harmony003 // Harmony non-ref patch parameters modified

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), nameof(AtOManager.BeginAdventure))]
        public static void BeginAdventurePrefix(ref AtOManager __instance, out string __state)
        {
            // LogInfo("BeginAdventurePrefix - Start");

            __state = __instance.GetGameId();

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), nameof(AtOManager.BeginAdventure))]
        public static void BeginAdventurePostfix(ref AtOManager __instance, string __state)
        {
            // LogInfo("BeginAdventurePostfix - Start");
            if (EnableMod.Value)
            {
                __instance.SetGameId(__state);
            }
        }



    }
}