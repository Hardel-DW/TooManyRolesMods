using HarmonyLib;
using Hazel;
using RolesMods.Utility.Enumerations;
using UnityEngine;
using UnityEngine.UI;

namespace RolesMods.Systems.Psychic {
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class Button {
        public static CooldownButton buttonPsychic;

        public static void Postfix(HudManager __instance) {
            buttonPsychic = new CooldownButton
                (() => OnClick(),
                Roles.Psychic.PsychicCooldown.GetValue(),
                "RolesMods.Resources.Foresight.png",
                1000,
                new Vector2(0f, 0f),
                __instance,
                Roles.Psychic.PsychicDuration.GetValue(),
                () => OnEffectEnd(),
                () => OnUpdate(buttonPsychic)
            );
        }

        private static void OnEffectEnd() {
            PsychicSystems.isPsychicActivated = false;
            PsychicSystems.ClearAllPlayers();
            PsychicSystems.psychicOverlay.SetActive(false);
            PsychicSystems.SyncOverlay(false);
                
            if (MapBehaviour.Instance != null) 
                MapBehaviour.Instance.ColorControl.SetColor(new Color(0.05f, 0.2f, 1f, 1f));
        }

        private static void OnClick() {
            PsychicSystems.isPsychicActivated = true;
            PsychicSystems.SyncOverlay(true);
            if (MapBehaviour.Instance != null)
                MapBehaviour.Instance.ShowNormalMap();
        }

        private static void OnUpdate(CooldownButton button) {
            if (Roles.Psychic.Instance.AllPlayers != null && PlayerControl.LocalPlayer != null) {
                if (Roles.Psychic.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId)) {
                    if (PlayerControl.LocalPlayer.Data.IsDead) button.SetCanUse(false);
                    else button.SetCanUse(true);
                }
            }
        }
    }
}
