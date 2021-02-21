using HarmonyLib;
using RolesMods.Utility;
using System.Linq;
using UnityEngine;

namespace RolesMods.Systems.Investigator {
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class UpdatePlayerPatch {
        public static float time = 0.0f;
        public static float interpolationPeriodNew = RolesMods.fontPrintInterval.GetValue() * 2;

        public static float timeUpdate = 0.0f;
        public static float interpolationPeriodUpdate = 2f;

        public static void Postfix(PlayerControl __instance) {
            if (GlobalVariable.isGameStarted && GlobalVariable.InvestigatorsList != null) {

                // New Footprint
                time += Time.deltaTime;
                if (time >= interpolationPeriodNew) {
                    time -= interpolationPeriodNew;

                    if (HelperRoles.IsInvestigator(PlayerControl.LocalPlayer.PlayerId))
                        foreach (var player in PlayerControl.AllPlayerControls)
                            if (player != null && !player.Data.IsDead && player.PlayerId != PlayerControl.LocalPlayer.PlayerId && !PlayerControl.LocalPlayer.inVent)
                                new FootPrint(GlobalVariable.footprintSizeValues[(int) RolesMods.footPrintSize.GetValue() - 1], RolesMods.fontPrintDuration.GetValue(), player);
                }

                // Update
                timeUpdate += Time.deltaTime;
                if (timeUpdate >= interpolationPeriodUpdate) {
                    timeUpdate -= interpolationPeriodUpdate;

                    foreach (var footprint in FootPrint.allFootprint.ToList()) {
                        footprint.Update();
                    }
                }
            }
        }
    }
}
