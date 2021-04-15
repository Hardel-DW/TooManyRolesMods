using HarmonyLib;
using UnityEngine;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(IntroCutscene.Nested_0), nameof(IntroCutscene.Nested_0.MoveNext))]
    public static class IntroCutScenePatch {
        public static void Postfix(IntroCutscene.Nested_0 __instance) {
            byte localPlayerId = PlayerControl.LocalPlayer.PlayerId;
            bool isImpostor = PlayerControl.LocalPlayer.Data.IsImpostor;

            foreach (var Role in RoleManager.AllRoles) {
                if (Role.HasRole(localPlayerId) && Role.ShowIntroCutScene) {
                    __instance.__this.Title.text = Role.Name;
                    __instance.__this.Title.m_fontScale /= 1 + (Mathf.Max(0f, Role.Name.Length - 7) * (1 / 3));
                    __instance.__this.Title.color = Role.Color;
                    __instance.__this.ImpostorText.text = Role.IntroDescription;
                    __instance.__this.BackgroundBar.material.color = Role.Color;
                }
            }
        }
    }
}
