﻿using HarmonyLib;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.ExitGame))]
    public static class EndGamePatch {

        public static void Prefix(AmongUsClient __instance) {
            EndGameCommons.ResetGlobalVariable();
        }
    }

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    public static class EndGameManagerPatch {
        public static bool Prefix(EndGameManager __instance) {
            EndGameCommons.ResetGlobalVariable();

            return true;
        }
    }

    public static class EndGameCommons {
        public static void ResetGlobalVariable() {
            GlobalVariable.isGameStarted = false;
            Systems.Investigator.FootPrint.allFootprint.Clear();
            HelperRoles.ClearRoles();
            Systems.TimeMaster.Time.ClearGameHistory();
        }
    }
}