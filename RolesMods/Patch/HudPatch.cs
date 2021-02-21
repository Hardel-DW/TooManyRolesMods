using HarmonyLib;
using UnityEngine;

namespace RolesMods.Patch {

    public static class HudPatch {
        public static void UpdateMeetingHUD(MeetingHud __instance) {
            foreach (PlayerVoteArea player in __instance.playerStates) {
                if (player.NameText.Text == GlobalVariable.TimeMaster.name && HelperRoles.IsTimeMaster(PlayerControl.LocalPlayer.PlayerId))
                    player.NameText.Color = new Color(0.49f, 0.49f, 0.49f, 1f);

                if (PlayerControl.AllPlayerControls != null && PlayerControl.AllPlayerControls.Count > 1 && PlayerControl.LocalPlayer != null) {
                    foreach (var playerControl in PlayerControl.AllPlayerControls) {

                        if (HelperRoles.IsInvestigator(playerControl.PlayerId) && HelperRoles.IsInvestigator(PlayerControl.LocalPlayer.PlayerId))
                            if (playerControl.Data.PlayerName == player.NameText.Text)
                                player.NameText.Color = new Color(0.180f, 0.678f, 1f, 1f);

                        if (HelperRoles.IsLighter(playerControl.PlayerId) && HelperRoles.IsLighter(PlayerControl.LocalPlayer.PlayerId))
                            if (playerControl.Data.PlayerName == player.NameText.Text)
                                player.NameText.Color = new Color(0.729f, 0.356f, 0.074f, 1f);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudUpdatePatch {

        public static void Postfix(HudManager __instance) {
            if (MeetingHud.Instance != null)
                HudPatch.UpdateMeetingHUD(MeetingHud.Instance);

            if (PlayerControl.AllPlayerControls.Count > 1 && GlobalVariable.InvestigatorsList != null && HelperRoles.IsInvestigator(PlayerControl.LocalPlayer.PlayerId))
                PlayerControl.LocalPlayer.nameText.Color = new Color(0.180f, 0.678f, 1f, 1f);

            if (PlayerControl.AllPlayerControls.Count > 1 && GlobalVariable.TimeMaster != null && HelperRoles.IsTimeMaster(PlayerControl.LocalPlayer.PlayerId))
                PlayerControl.LocalPlayer.nameText.Color = new Color(0.49f, 0.49f, 0.49f, 1f);

            if (PlayerControl.AllPlayerControls.Count > 1 && GlobalVariable.LightersList != null && HelperRoles.IsLighter(PlayerControl.LocalPlayer.PlayerId))
                PlayerControl.LocalPlayer.nameText.Color = new Color(0.729f, 0.356f, 0.074f, 1f);

            CooldownButton.HudUpdate();
        }
    }
}