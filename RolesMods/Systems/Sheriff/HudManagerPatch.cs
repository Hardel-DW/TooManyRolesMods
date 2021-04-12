/*using HarmonyLib;
using InnerNet;
using RolesMods.Utility;
using UnityEngine;

namespace RolesMods.Systems.Sheriff {

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerPatch {
        private static KillButtonManager KillButton;

        public static void Postfix(HudManager __instance) {
            KillButton = __instance.KillButton;
            if (PlayerControl.LocalPlayer == null || PlayerControl.AllPlayerControls == null)
                return;

            if (PlayerControl.LocalPlayer.Data == null || !(PlayerControl.AllPlayerControls.Count > 0))
                return;

            if (Roles.Sheriff.Instance.HasRole(PlayerControl.LocalPlayer)) {
                if (PlayerControl.LocalPlayer.Data.IsDead) {
                    KillButton.gameObject.SetActive(false);
                    KillButton.isActive = false;
                } else {
                    KillButton.gameObject.SetActive(!MeetingHud.Instance);
                    KillButton.isActive = !MeetingHud.Instance;
                    KillButton.SetCoolDown(Roles.Sheriff.SheriffKillTimer(), Roles.Sheriff.SheriffKillCooldown.GetValue() + 15f);

                    if (Input.GetKeyDown(KeyCode.Q))
                        KillButton.PerformKill();

                    PlayerControl ClosestPlayer = PlayerControlUtils.GetClosestPlayer(PlayerControl.LocalPlayer);
                    float distBetweenPlayers = Vector3.Distance(PlayerControl.LocalPlayer.transform.position, ClosestPlayer.transform.position);

                    if (distBetweenPlayers < GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance] && KillButton.enabled)
                        KillButton.SetTarget(ClosestPlayer);
                }
            } else {
                if ((PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.Data.IsImpostor) || !(AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)) {
                    KillButton.gameObject.SetActive(false);
                    KillButton.isActive = false;
                    Plugin.Logger.LogInfo("Test");
                } else {
                    Plugin.Logger.LogInfo(!MeetingHud.Instance);
                    __instance.KillButton.gameObject.SetActive(!MeetingHud.Instance);
                    __instance.KillButton.isActive = !MeetingHud.Instance;
                }
            }   

        }
    }
}
*/