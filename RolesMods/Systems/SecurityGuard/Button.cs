using HardelAPI.Utility;
using HarmonyLib;
using UnityEngine;

namespace RolesMods.Systems.SecurityGuard {

    public enum SecurityGuardState {
        PlaceCamera,
        SealVent
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class Button {
        private static Sprite SealVent;
        private static Sprite PlaceCamera;
        private static SecurityGuardState SecurityGuardType;

        public static CooldownButton button;
        public static Vent closestVent;
        public static int totalScrews = 7;
        public static int ventPrice = 1;
        public static int camPrice = 2;

        public static void Postfix(HudManager __instance) {
            SealVent = Plugin.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Foresight.png", 1000f);
            PlaceCamera = Plugin.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Rewind.png", 250f);
            SecurityGuardType = SecurityGuardState.PlaceCamera;

            button = new CooldownButton
                (() => OnClick(),
                Roles.SecurityGuard.CooldownSecurityGuard.GetValue(),
                PlaceCamera,
                250,
                new Vector2(0f, 0f),
                __instance,
                () => OnUpdate(button)
            );
        }

        private static void OnClick() {
            int cost = SecurityGuardType switch {
                SecurityGuardState.PlaceCamera => camPrice,
                SecurityGuardState.SealVent => ventPrice,
                _ => 0
            };

            if (totalScrews > cost) {
                totalScrews -= cost;

                if (SecurityGuardType == SecurityGuardState.PlaceCamera) {
                    CameraUtils.AddNewCamera(PlayerControl.LocalPlayer.transform.position);
                    Roles.SecurityGuard.camerasToAdd.Add(PlayerControl.LocalPlayer.transform.position);
                }
                else if (SecurityGuardType == SecurityGuardState.SealVent)
                    Roles.SecurityGuard.ventsToSeal.Add(closestVent);
            }
        }

        private static void OnUpdate(CooldownButton button) {
            if (Roles.SecurityGuard.Instance.AllPlayers != null && PlayerControl.LocalPlayer != null) {
                if (Roles.SecurityGuard.Instance.HasRole(PlayerControl.LocalPlayer)) {
                    if (PlayerControl.LocalPlayer.Data.IsDead)
                        button.SetCanUse(false);
                    else button.SetCanUse(!MeetingHud.Instance);

                    if (closestVent != null) {
                        closestVent.GetComponent<SpriteRenderer>().material.SetFloat("_Outline", 0f);

                        if (SecurityGuardType != SecurityGuardState.SealVent) {
                            SecurityGuardType = SecurityGuardState.SealVent;
                            button.Sprite = SealVent;
                            Plugin.Logger.LogInfo("Test");
                        }
                    } else {
                        if (SecurityGuardType != SecurityGuardState.PlaceCamera) {
                            SecurityGuardType = SecurityGuardState.PlaceCamera;
                            button.Sprite = PlaceCamera;
                        }
                    }

                    Vent target = VentUtils.GetClosestVent(PlayerControl.LocalPlayer);
                    if (target != null) {
                        SpriteRenderer component = target.GetComponent<SpriteRenderer>();
                        component.material.SetFloat("_Outline", 1f);
                        component.material.SetColor("_OutlineColor", Roles.SecurityGuard.Instance.Color);
                        closestVent = target;
                    } else {
                        closestVent = null;
                    }
                }
            }
        }
    }
}