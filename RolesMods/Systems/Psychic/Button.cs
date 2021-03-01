using HarmonyLib;
using RolesMods.Utility.Enumerations;
using UnityEngine;

namespace RolesMods.Systems.Psychic {
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class Button {
        public static void Postfix(HudManager __instance) {
            GlobalVariable.buttonPsychic = new CooldownButton
                (() => OnClick(),
                RolesMods.PsychicCooldown.GetValue(),
                "RolesMods.Resources.Psychic.png",
                128f,
                new Vector2(0f, 0f),
                Visibility.Everyone,
                __instance,
                RolesMods.PsychicDuration.GetValue(),
                () => OnEffectEnd(),
                () => OnUpdate(GlobalVariable.buttonPsychic)
            );
        }

        private static void OnEffectEnd() {
            GlobalVariable.ispsychicActivated = false;
            MiniMapPlayers.ClearAllPlayers();

            if (MapBehaviour.Instance != null) 
                MapBehaviour.Instance.ColorControl.SetColor(new Color(0.05f, 0.2f, 1f, 1f));
        }

        private static void OnClick() {
            GlobalVariable.ispsychicActivated = true;
            MiniMapPlayers.OverlaySystem(true);
            if (MapBehaviour.Instance != null)
                MapBehaviour.Instance.ShowNormalMap();
        }

        private static void OnUpdate(CooldownButton button) {
            if (GlobalVariable.PsychicList != null && PlayerControl.LocalPlayer != null) {
                if (HelperRoles.IsPsychic(PlayerControl.LocalPlayer.PlayerId)) {
                    if (PlayerControl.LocalPlayer.Data.IsDead) button.SetCanUse(false);
                    else button.SetCanUse(true);
                }
            }
        }
    }
}
