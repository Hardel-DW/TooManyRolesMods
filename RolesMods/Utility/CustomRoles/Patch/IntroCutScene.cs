using HarmonyLib;
using UnityEngine;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CoBegin))]
    public static class IntroCutScenePatch {
        public static void Postfix(IntroCutscene __instance) {
            byte localPlayerId = PlayerControl.LocalPlayer.PlayerId;
            bool isImpostor = PlayerControl.LocalPlayer.Data.IsImpostor;

            foreach (var Role in RoleManager.AllRoles) {
                if (Role.HasRole(localPlayerId) && Role.ShowIntroCutScene) {
                    __instance.Title.text = Role.Name;
                    __instance.Title.m_fontScale /= 1 + (Mathf.Max(0f, Role.Name.Length - 7) * (1 / 3));
                    __instance.Title.color = Role.Color;
                    __instance.ImpostorText.text = Role.IntroDescription;
                    __instance.BackgroundBar.material.color = Role.Color;
                }
            }
        }
    }
}
