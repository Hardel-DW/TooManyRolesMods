using HarmonyLib;
using UnityEngine;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__11), nameof(IntroCutscene._CoBegin_d__11.MoveNext))]
    public static class IntroCutScenePatch {
        public static void Postfix(IntroCutscene._CoBegin_d__11 __instance) {
            byte localPlayerId = PlayerControl.LocalPlayer.PlayerId;
            bool isImpostor = PlayerControl.LocalPlayer.Data.IsImpostor;

            foreach (var Role in RoleManager.AllRoles) {
                if (Role.HasRole(localPlayerId) && Role.ShowIntroCutScene) {
                    __instance.__this.Title.Text = Role.Name;
                    __instance.__this.Title.scale /= 1 + (Mathf.Max(0f, Role.Name.Length - 7) * (1 / 3));
                    __instance.__this.Title.Color = Role.Color;
                    __instance.__this.ImpostorText.Text = Role.IntroDescription;
                    __instance.__this.BackgroundBar.material.color = Role.Color;
                }
            }
        }
    }
}
