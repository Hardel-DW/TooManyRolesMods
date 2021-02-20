using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace RolesMods.Systems.Investigator {
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class UpdatePlayerPatch {
        public static float time = 0.0f;
        public static float interpolationPeriod = 2f;

        public static void Postfix(PlayerControl __instance) {
            if (GlobalVariable.isGameStarted && GlobalVariable.Investigator != null) {
                time += Time.deltaTime;

                if (time >= interpolationPeriod) {
                    time -= interpolationPeriod;

                    if (HelperRoles.IsInvestigator(PlayerControl.LocalPlayer.PlayerId))
                        foreach (var player in PlayerControl.AllPlayerControls)
                            if (player != null && !player.Data.IsDead && player.PlayerId != PlayerControl.LocalPlayer.PlayerId)
                                new FootPrint(1024, 10f, player);

                    foreach (var footprint in FootPrint.allFootprint.ToList()) {
                        footprint.Update();
                    }
                }
            }
        }
    }
}
