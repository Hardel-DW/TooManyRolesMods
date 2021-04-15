using HarmonyLib;
using InnerNet;
using RolesMods.Utility;
using UnityEngine;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerPatch {
        private static KillButtonManager KillButton;

        public static void Postfix(HudManager __instance) {
            KillButton = __instance.KillButton;
            
            foreach (var Role in RoleManager.AllRoles) {
                if (PlayerControl.LocalPlayer == null || PlayerControl.AllPlayerControls == null)
                    return;

                if (PlayerControl.LocalPlayer.Data == null || !(PlayerControl.AllPlayerControls.Count > 1))
                    return;

                if (Role.WhiteListKill == null) {
                    if (Role.CanKill != Enumerations.PlayerSide.Nobody || PlayerControl.LocalPlayer.Data.IsImpostor) {
                        Role.DefineKillWhiteList();
                    }

                    continue;
                }

                PlayerControl ClosestPlayer = Role.GetClosestTarget(PlayerControl.LocalPlayer);
                if ((AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started) || (AmongUsClient.Instance.GameMode == GameModes.FreePlay)) {
                    if (Role.HasRole(PlayerControl.LocalPlayer)) {
                        if (PlayerControl.LocalPlayer.Data.IsDead) {
                            KillButton.gameObject.SetActive(false);
                            KillButton.isActive = false;
                        } else {
                            KillButton.gameObject.SetActive(!MeetingHud.Instance);
                            KillButton.isActive = !MeetingHud.Instance;
                            KillButton.SetCoolDown(Role.KillTimer(), Role.KillCooldown + 15f);

                            if (Input.GetKeyDown(KeyCode.Q))
                                KillButton.PerformKill();
                            
                            float distBetweenPlayers = Vector3.Distance(PlayerControl.LocalPlayer.transform.position, ClosestPlayer.transform.position);

                            if (distBetweenPlayers < GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance] && KillButton.enabled)
                                KillButton.SetTarget(ClosestPlayer);
                        }

                        break;
                    } else if (PlayerControl.LocalPlayer.Data.IsImpostor && !PlayerControl.LocalPlayer.Data.IsDead) {
                        __instance.KillButton.gameObject.SetActive(!MeetingHud.Instance);
                        __instance.KillButton.isActive = !MeetingHud.Instance;
                    } else {
                        KillButton.gameObject.SetActive(false);
                        KillButton.isActive = false;
                    }
                }
            }
        }
    }
}
