using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace RolesMods.Utility {

    public class HatsCreator {
        private static bool modded = false;

        public struct HatData {
            public bool bounce;
            public string name;
            public bool highUp;
            public Vector2 offset;
            public string author;
        }

        private static List<uint> TallIds = new List<uint>();
        private static List<HatData> allHatsData = new List<HatData>();
        protected internal static Dictionary<uint, HatData> IdToData = new Dictionary<uint, HatData>();

        public static void CreateHats(HatData hat) {
            allHatsData.Add(hat);
        }

        public static void CreateMultipleHats(List<HatData> hats) {
            allHatsData.AddRange(hats);
        }

        private static HatBehaviour CreateHat(HatData hat, int id) {
            System.Console.WriteLine($"Creating Hat: {hat.name}");
            Sprite sprite = HelperSprite.LoadHatSprite($"IronMan.Resources.Hats.{hat.name}.png");
            HatBehaviour newHat = ScriptableObject.CreateInstance<HatBehaviour>();
            newHat.MainImage = sprite;
            newHat.ProductId = hat.name;
            newHat.Order = 99 + id;
            newHat.InFront = true;
            newHat.NoBounce = !hat.bounce;
            newHat.ChipOffset = hat.offset;

            return newHat;
        }

        private static IEnumerable<HatBehaviour> CreateAllHats() {
            var i = 0;
            foreach (var hat in allHatsData)
                yield return CreateHat(hat, ++i);
        }

        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetHatById))]
        public static class HatManagerPatch {
            static bool Prefix(HatManager __instance) {
                try {
                    if (!modded) {
                        System.Console.WriteLine("Adding hats");
                        modded = true;
                        var id = 0;
                        foreach (var hatData in allHatsData) {
                            HatBehaviour hat = CreateHat(hatData, id++);
                            __instance.AllHats.Add(hat);
                            if (hatData.highUp)
                                TallIds.Add((uint) (__instance.AllHats.Count - 1));
                            IdToData.Add((uint) __instance.AllHats.Count - 1, hatData);
                        }
                    }
                    return true;
                } catch (Exception e) {
                    System.Console.WriteLine("During Prefix, an exception occured" + e);
                    throw;
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetHat))]
        public static class PlayerControl_SetHat {
            public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] uint hatId, [HarmonyArgument(1)] int colorId) {
                __instance.nameText.transform.localPosition = new Vector3(0f, hatId == 0U ? 0.7f : TallIds.Contains(hatId) ? 1.2f : 1.05f, -0.5f);
            }
        }
    }
}
