using HardelAPI.CustomRoles;
using HardelAPI.Utility;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Systems.Seer {

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class Button {
        public static CooldownButton button;
        public static PlayerControl closestPlayer;
        public static List<PlayerControl> allPlayersTargetable = new List<PlayerControl>();
        public static int UseNumber = (int) Roles.Seer.SeerUseNumber.GetValue();

        public static void Postfix(HudManager __instance) {
            button = new CooldownButton
                (() => OnClick(),
                Roles.Seer.SeerCooldown.GetValue(),
                Plugin.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Rewind.png", 250f),
                250,
                new Vector2(0f, 0f),
                __instance,
                () => OnUpdate(button)
            );
        }


        private static void OnClick() {
            if (allPlayersTargetable == null)
                return;

            if (UseNumber > 0) {
                UseNumber--;

                if (closestPlayer.Data.IsImpostor) {
                    RevealRole(closestPlayer, true);
                } else {
                    int PercentReaveal = new System.Random().Next(0, 100);
                    if (PercentReaveal > Roles.Seer.SeerPercentSeeRole.GetValue())
                        RevealRole(closestPlayer, false);
                    else
                        RevealRole(closestPlayer, true);
                }
            }
        }

        private static void OnUpdate(CooldownButton button) {
            if (Roles.Seer.Instance.AllPlayers != null && PlayerControl.LocalPlayer != null) {
                if (Roles.Seer.Instance.HasRole(PlayerControl.LocalPlayer)) {
                    if (PlayerControl.LocalPlayer.Data.IsDead || UseNumber <= 0)
                        button.SetCanUse(false);
                    else button.SetCanUse(!MeetingHud.Instance);

                    if (allPlayersTargetable != null) {
                        PlayerControl target = PlayerControlUtils.GetClosestPlayer(PlayerControl.LocalPlayer, allPlayersTargetable);
                        if (closestPlayer != null) {
                            button.isDisable = false;
                            closestPlayer.myRend.material.SetFloat("_Outline", 0f);
                        } else {
                            button.isDisable = true;
                        }

                        if (target != null) {
                            target.myRend.material.SetFloat("_Outline", 1f);
                            target.myRend.material.SetColor("_OutlineColor", Roles.Seer.Instance.Color);
                            closestPlayer = target;
                        } else {
                            closestPlayer = null;
                        }
                    }
                }
            }
        }

        private static void RevealRole(PlayerControl target, bool failed) {
            RoleManager mainRole = RoleManager.GetMainRole(target);
            Color colorDisplay = mainRole != null ? mainRole.Color : target.Data.IsImpostor ?  Palette.ImpostorRed : Palette.White;
            string nameDisplay = mainRole != null ? mainRole.Name : target.Data.IsImpostor ? "Impostor" : "Crewmate";

            if (failed) RoleManager.specificNameInformation.Add(target, (Palette.White, "???"));
            else RoleManager.specificNameInformation.Add(target, (colorDisplay, nameDisplay));

            allPlayersTargetable.RemovePlayer(target);
        }
    }
}