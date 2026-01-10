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
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text.Json;

// Make sure your namespace is the same everywhere
namespace DefiniteSeeds
{

    [HarmonyPatch] //DO NOT REMOVE/CHANGE

    public class DefiniteSeedsFunctions
    {


        public static bool IsHost()
        {
            // Check if both GameManager.Instance and NetworkManager.Instance are not null to avoid possible NullReferenceException
            if (GameManager.Instance == null || NetworkManager.Instance == null)
                return false;

            return GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster();
        }

        public static bool IsMP()
        {
            // Check if GameManager.Instance is not null to avoid possible NullReferenceException
            if (GameManager.Instance == null)
                return false;

            return GameManager.Instance.IsMultiplayer();
        }


        public static void HandleResetSeed4()
        {
            AtOManager.Instance?.SetGameId();
            AtOManager.Instance?.SaveGame();
            AtOManager.Instance?.LoadGame(AtOManager.Instance.GetSaveSlot());
        }

        public static void HandleResetSeedChosen()
        {

            AlertManager.buttonClickDelegate = new AlertManager.OnButtonClickDelegate(SetSeedChosen);
            AlertManager.Instance.AlertInput("Input Seed", Texts.Instance.GetText("accept").ToUpper());
        }

        public static void SetSeedChosen()
        {
            AlertManager.buttonClickDelegate -= new AlertManager.OnButtonClickDelegate(SetSeedChosen);
            if (AlertManager.Instance.GetInputValue() == null)
                return;
            string seed = AlertManager.Instance.GetInputValue().ToLower();
            if (seed.Trim() == "")
            {
                HandleResetSeed4();
                return;
            }

            AtOManager.Instance?.SetGameId(seed);
            AtOManager.Instance?.SaveGame();
            AtOManager.Instance?.LoadGame(AtOManager.Instance.GetSaveSlot());
        }

        public static Transform CreateIcon(Transform original, string name = "", List<GameObject> buttonList = null)
        {
            GameObject iconClone = UnityEngine.Object.Instantiate(original.gameObject, Vector3.zero, Quaternion.identity, null);
            if (iconClone == null)
            {
                LogDebug("Failed to instantiate game object clone");
                return null;
            }
            Transform newIcon = iconClone.transform;
            if (newIcon == null)
            {
                LogDebug("Cloned object has no transform component");
                return null;
            }
            UnityEngine.Object.DontDestroyOnLoad(newIcon);
            newIcon.gameObject.SetActive(false);

            newIcon.gameObject.name = name;
            buttonList?.Add(newIcon.gameObject);
            return newIcon;
        }

        public static void SetButtons(List<GameObject> gameObjects, bool active)
        {
            foreach (GameObject go in gameObjects)
            {
                go.SetActive(active);
            }
        }

    }
}

