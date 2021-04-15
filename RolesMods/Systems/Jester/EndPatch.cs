using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace RolesMods.Systems.Jester {


    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    public static class EndGameManagerPatch {
        public static void Postfix(EndGameManager __instance) {
            if (ExiledPatch.JesterForceEndGame) {
                __instance.WinText.color = new Color(0.921f, 0.239f, 0.862f, 1f);
                __instance.BackgroundBar.material.color = new Color(0.921f, 0.239f, 0.862f, 1f);
                ExiledPatch.JesterForceEndGame = false;
            }
        }
    }
}
