using UnityEngine;
using HardelAPI.Cooldown;
using SecurityGuardRoles = RolesMods.Roles.SecurityGuard;
using HardelAPI.Utility.Helper;

namespace RolesMods.Systems.SecurityGuard {

    public enum SecurityGuardState {
        PlaceCamera,
        SealVent
    }

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {
        private static Sprite SealVent;
        private static Sprite PlaceCamera;
        private static SecurityGuardState SecurityGuardType;
        public static int totalScrews = 7;
        public static int ventPrice = 1;
        public static int camPrice = 2;

        public override void OnCreateButton() {
            SecurityGuardType = SecurityGuardState.SealVent;
            Closest = HardelAPI.Cooldown.ClosestElement.Vent;

            Timer = SecurityGuardRoles.CooldownSecurityGuard.GetValue();
            Roles = SecurityGuardRoles.Instance;
            SealVent = SpriteHelper.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Foresight.png", 1000f);
            PlaceCamera = SpriteHelper.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Rewind.png", 250f);
            SetSprite("RolesMods.Resources.Rewind.png", 250);
        }

        public override void OnClick() {
            int cost = SecurityGuardType switch {
                SecurityGuardState.PlaceCamera => camPrice,
                SecurityGuardState.SealVent => ventPrice,
                _ => 0
            };

            if (totalScrews > cost) {
                totalScrews -= cost;

                Vent closestVent = GetVentTarget();
                if (SecurityGuardType == SecurityGuardState.PlaceCamera)
                    SecurityGuardRoles.camerasToAdd.Add(PlayerControl.LocalPlayer.transform.position);
                else if (SecurityGuardType == SecurityGuardState.SealVent)
                    SecurityGuardRoles.ventsToSeal.Add(closestVent);
            }
        }

        public override void OnUpdate() {
            if (SecurityGuardRoles.Instance.AllPlayers != null && PlayerControl.LocalPlayer != null) {
                if (SecurityGuardRoles.Instance.HasRole(PlayerControl.LocalPlayer)) {

                    Vent closestVent = GetVentTarget();
                    if (closestVent != null || ShipStatus.Instance.AllCameras == null || ShipStatus.Instance.AllCameras.Count == 0) {
                        IsDisable = (closestVent == null);

                        if (SecurityGuardType != SecurityGuardState.SealVent) {
                            SecurityGuardType = SecurityGuardState.SealVent;
                            SetSprite(SealVent);
                        }
                    } else {
                        if (SecurityGuardType != SecurityGuardState.PlaceCamera) {
                            SecurityGuardType = SecurityGuardState.PlaceCamera;
                            SetSprite(PlaceCamera);
                        }
                    }
                }
            }
        }
    }
}