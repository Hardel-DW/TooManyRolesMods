using HarmonyLib;
using RolesMods.Utility;
using RolesMods;
using UnityEngine;

namespace RolesMods.Patch {

    public static class HudPatch {
        public static void UpdateMeetingHUD(MeetingHud __instance) {
            foreach (PlayerVoteArea player in __instance.playerStates) {
                if (player.NameText.Text == GlobalVariable.Investigator.name && HelperRoles.IsInvestigator(PlayerControl.LocalPlayer.PlayerId)) 
                    player.NameText.Color = new Color(0.180f, 0.678f, 1f, 1f);

                if (player.NameText.Text == GlobalVariable.TimeMaster.name && HelperRoles.IsTimeMaster(PlayerControl.LocalPlayer.PlayerId))
                    player.NameText.Color = new Color(0.49f, 0.49f, 0.49f, 1f);

                if (player.NameText.Text == GlobalVariable.Lighter.name && HelperRoles.IsLighter(PlayerControl.LocalPlayer.PlayerId))
                    player.NameText.Color = new Color(186, 91, 19, 255);
            }
        }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudUpdatePatch {

        public static void Postfix(HudManager __instance) {
            if (MeetingHud.Instance != null)
                HudPatch.UpdateMeetingHUD(MeetingHud.Instance);

            if (PlayerControl.AllPlayerControls.Count > 1 && GlobalVariable.Investigator != null && HelperRoles.IsInvestigator(PlayerControl.LocalPlayer.PlayerId))
                PlayerControl.LocalPlayer.nameText.Color = new Color(0.180f, 0.678f, 1f, 1f);

            if (PlayerControl.AllPlayerControls.Count > 1 && GlobalVariable.TimeMaster != null && HelperRoles.IsTimeMaster(PlayerControl.LocalPlayer.PlayerId))
                PlayerControl.LocalPlayer.nameText.Color = new Color(0.49f, 0.49f, 0.49f, 1f);

            if (PlayerControl.AllPlayerControls.Count > 1 && GlobalVariable.Lighter != null && HelperRoles.IsLighter(PlayerControl.LocalPlayer.PlayerId))
                PlayerControl.LocalPlayer.nameText.Color = new Color(186, 91, 19, 255);

            CooldownButton.HudUpdate();
        }
    }
}