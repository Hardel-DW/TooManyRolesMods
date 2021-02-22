﻿using HarmonyLib;
using UnityEngine;
using IntroCutScene = PENEIDJGGAF.CKACLKCOJFO;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(IntroCutScene), nameof(IntroCutScene.MoveNext))]
    public static class IntroCutScenePatch {
        public static void Postfix(IntroCutScene __instance) {
            byte localPlayerId = PlayerControl.LocalPlayer.PlayerId;
            bool isImpostor = PlayerControl.LocalPlayer.Data.IsImpostor;

            // Investigator
            if (HelperRoles.IsInvestigator(localPlayerId)) {
                __instance.__this.Title.Text = "Investigator";
                __instance.__this.Title.scale /= 1.4f;
                __instance.__this.Title.Color = new Color(0.180f, 0.678f, 1f, 1f);
                __instance.__this.ImpostorText.Text = "Find all imposters by examining footprints";
                __instance.__this.BackgroundBar.material.color = new Color(0.180f, 0.678f, 1f, 1f);
            } 
            
            // Time Master
            else if (HelperRoles.IsTimeMaster(localPlayerId) && !isImpostor) {
                __instance.__this.Title.Text = "Time Master";
                __instance.__this.Title.scale /= 1.3f;   
                __instance.__this.Title.Color = new Color(0.490f, 0.490f, 0.490f, 1f);
                __instance.__this.ImpostorText.Text = "Bend time as you will";
                __instance.__this.BackgroundBar.material.color = new Color(0.490f, 0.490f, 0.490f, 1f);
            }  

            else if (HelperRoles.IsTimeMaster(localPlayerId) && isImpostor) {
                __instance.__this.Title.Text = "Impostor Time Master";
                __instance.__this.Title.scale /= 2.3f;
                __instance.__this.Title.Color = new Color(0.686f, 0.415f, 0.435f, 1f);
                __instance.__this.ImpostorText.Text = "Bend time as you will and kill everyone.";
                __instance.__this.BackgroundBar.material.color = new Color(0.490f, 0.490f, 0.490f, 1f);
            }

            // Lighter
            else if (HelperRoles.IsLighter(localPlayerId)) {
                __instance.__this.Title.Text = "Lighter";
                __instance.__this.Title.Color = new Color(0.729f, 0.356f, 0.074f, 1f);
                __instance.__this.ImpostorText.Text = "Your vision is better than crewmate";
                __instance.__this.BackgroundBar.material.color = new Color(0.729f, 0.356f, 0.074f, 1f);
            }
        }
    }
}
