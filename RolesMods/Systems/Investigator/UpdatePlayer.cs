﻿using HarmonyLib;
using RolesMods.Utility;
using System.Linq;
using UnityEngine;

namespace RolesMods.Systems.Investigator {
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class UpdatePlayerPatch {
        public static float time = 0.0f;

        public static void Postfix(PlayerControl __instance) {
            float interpolationPeriod = RolesMods.fontPrintInterval.GetValue();

            if (GlobalVariable.isGameStarted && GlobalVariable.InvestigatorsList != null && HelperRoles.IsInvestigator(PlayerControl.LocalPlayer.PlayerId)) {

                time += Time.deltaTime;
                if (time >= interpolationPeriod) {
                    time -= interpolationPeriod;

                    if (HelperRoles.IsInvestigator(PlayerControl.LocalPlayer.PlayerId)) {
                        foreach (var player in PlayerControl.AllPlayerControls) {
                            if (player != null && !player.Data.IsDead && player.PlayerId != PlayerControl.LocalPlayer.PlayerId) {
                                bool canPlace = true;

                                foreach (var footprint in FootPrint.allFootprint)
                                    if (player.PlayerId == footprint.player.PlayerId && Vector3.Distance(footprint.position, PlayerControlUtils.Position(player)) < 0.05f)
                                        canPlace = false;

                                if (!RolesMods.VentFootprintVisible.GetValue() && ShipStatus.Instance != null)
                                    foreach (var vent in ShipStatus.Instance.AllVents.ToList())
                                        if (Vector2.Distance(vent.gameObject.transform.position, PlayerControlUtils.Position(player)) < 1f)
                                            canPlace = false;

                                if (canPlace) 
                                    new FootPrint(RolesMods.footPrintSize.GetValue(), RolesMods.fontPrintDuration.GetValue(), player);
                            }
                        }
                    }
                }
            }
        }
    }
}
