using HarmonyLib;
using Hazel;
using RolesMods.Utility.Enumerations;
using System;
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
            PsychicMap.isPsychicActivated = false;
            PsychicMap.ClearAllPlayers();
            //PsychicMap.psychicOverlay.SetActive(false);
            //PsychicMap.SyncOverlay(false);

            if (MapBehaviour.Instance != null) {
                    DestroyableSingleton<HudManager>.Instance.ShowMap((Action<MapBehaviour>) (map => {
                    map.ColorControl.SetColor(new Color(0.05f, 0.2f, 1f, 1f));
                    map.countOverlay.gameObject.SetActive(false);
                    map.infectedOverlay.gameObject.SetActive(false);
                    map.taskOverlay.Show();
                    map.HerePoint.enabled = true;
                }));
            }
        }

        private static void OnClick() {
            PsychicMap.isPsychicActivated = true;
            //PsychicSystems.SyncOverlay(true);
            DestroyableSingleton<HudManager>.Instance.ShowMap((Action<MapBehaviour>) (map => {
                map.transform.localScale = Vector3.one;
                map.transform.localPosition = new Vector3(0f, 0f, -25f);
                map.gameObject.SetActive(true);
                map.countOverlay.gameObject.SetActive(false);
                map.infectedOverlay.gameObject.SetActive(false);
                map.taskOverlay.Show();
                map.HerePoint.enabled = true;
                map.gameObject.AddComponent<PsychicMap>();
                DestroyableSingleton<HudManager>.Instance.SetHudActive(false);
            }));
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
