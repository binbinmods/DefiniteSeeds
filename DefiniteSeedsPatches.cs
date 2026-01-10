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
using TMPro;

// Make sure your namespace is the same everywhere
namespace DefiniteSeeds
{

    [HarmonyPatch] //DO NOT REMOVE/CHANGE

    public class DefiniteSeedsPatches
    {
        public static Transform iconResetSeed;
        public static List<GameObject> myButtonsMap;
        // public static bool isMP = false;
        // public static bool isHost = false;




        [HarmonyPostfix]
        [HarmonyPatch(typeof(OptionsManager), "Awake")]
        public static void AwakePostfix(OptionsManager __instance, List<GameObject> ___buttonOrder)
        {
            // BeginAdventure
            LogDebug("AwakePostfix");
            myButtonsMap = [];

            iconResetSeed = CreateIcon(__instance.iconRetry, "resetseed", myButtonsMap);

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(OptionsManager), nameof(OptionsManager.Show))]
        public static void ShowPostfix(OptionsManager __instance, List<GameObject> ___buttonOrder)
        {
            // BeginAdventure

            SetButtons(myButtonsMap, false);
            if ((bool)(UnityEngine.Object)MapManager.Instance && (IsHost() || !IsMP()))
            {
                SetButtons(myButtonsMap, true);


                iconResetSeed.gameObject.SetActive(EnableSeedResets.Value);


            }

            float positionRightButton = 0.95f;
            float distanceBetweenButton = 0.65f;

            for (int index = 0; index < myButtonsMap.Count; ++index)
            {
                if (myButtonsMap[index].activeSelf)
                {
                    myButtonsMap[index].transform.position = new Vector3(__instance.iconTome.transform.position.x - positionRightButton, __instance.iconTome.transform.position.y, __instance.iconTome.transform.position.z);
                    myButtonsMap[index].transform.localPosition = new Vector3(positionRightButton - distanceBetweenButton * 6.35f + IconHorizontalShift.Value * 0.01f, myButtonsMap[index].transform.localPosition.y, myButtonsMap[index].transform.localPosition.z);
                    positionRightButton -= distanceBetweenButton;
                }
            }


        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotonRollover), "ShowText")]
        public static void BotonRolloverShowText(BotonRollover __instance)
        {
            // LogDebug($"BotonRolloverShowText - {__instance.gameObject.name}");
            // LogDebug($"BotonRolloverShowText - text {__instance.rollOverText?.GetComponent<TMP_Text>()?.text ?? "null TMP_Text"}");
            if (__instance.gameObject.name == "resetseed")
            {
                __instance.rollOverText.GetComponent<TMP_Text>().text = "Reroll Seed";
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotonRollover), "OnMouseUp")]
        public static void BotonRolloverOnMouseUp(BotonRollover __instance)
        {
            // isMP = GameManager.Instance.IsMultiplayer();
            // isHost = GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster();

            if (!Functions.ClickedThisTransform(__instance.transform) || AlertManager.Instance.IsActive() || GameManager.Instance.IsTutorialActive() || SettingsManager.Instance.IsActive() || DamageMeterManager.Instance.IsActive() || (bool)(UnityEngine.Object)MapManager.Instance && MapManager.Instance.IsCharacterUnlock() || (bool)(UnityEngine.Object)MatchManager.Instance && MatchManager.Instance.console.IsActive())
                return;
            string name = __instance.gameObject.name;
            CloseWindows(__instance, name);
            switch (name)
            {
                case "resetseed":
                    HandleResetSeed4();
                    // HandleResetSeed2();
                    break;
            }
            fRollOut(__instance);

        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(BotonRollover), "fRollOut")]
        public static void fRollOut(BotonRollover __instance)
        {
            return;
        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(BotonRollover), "CloseWindows")]
        public static void CloseWindows(BotonRollover __instance, string botName)
        {
            return;
        }

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