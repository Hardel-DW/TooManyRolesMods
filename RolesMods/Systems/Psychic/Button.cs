using HardelAPI.Utility;
using HarmonyLib;
using System;
using UnityEngine;

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
            PsychicMap.isPsychicActivated = false;
            PsychicMap.ClearAllPlayers();

            if (MapBehaviour.Instance != null) {
                HudManager.Instance.OpenMap();
                HudManager.Instance.OpenMap();
            }
        }

        private static void OnClick() {
            PsychicMap.isPsychicActivated = true;
            DestroyableSingleton<HudManager>.Instance.ShowMap((Action<MapBehaviour>) (map => {
                map.gameObject.SetActive(true);
                map.gameObject.AddComponent<PsychicMap>();
                DestroyableSingleton<HudManager>.Instance.SetHudActive(false);
            }));
        }

        private static void OnUpdate(CooldownButton button) {
            if (Roles.Psychic.Instance.AllPlayers != null && PlayerControl.LocalPlayer != null) {
                if (Roles.Psychic.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId)) {
                    if (PlayerControl.LocalPlayer.Data.IsDead) button.SetCanUse(false);
                    else button.SetCanUse(!MeetingHud.Instance);
                }
            }
        }
    }
}
