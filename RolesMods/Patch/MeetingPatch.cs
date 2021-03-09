using HarmonyLib;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class MeetingClosePatch {

        public static void Postfix(MeetingHud __instance) {
            if (GlobalVariable.TimeMaster != null) {
                if (HelperRoles.IsTimeMaster(PlayerControl.LocalPlayer.PlayerId)) {
                    GlobalVariable.buttonTime.Timer = RolesMods.TimeMasterCooldown.GetValue();
                }
            }

            if (GlobalVariable.PsychicList != null) {
                if (HelperRoles.IsPsychic(PlayerControl.LocalPlayer.PlayerId)) {
                    GlobalVariable.buttonPsychic.Timer = RolesMods.PsychicCooldown.GetValue();
                }
            }
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    public static class MeetingUpdatePatch {
        public static void Postfix(MeetingHud __instance) {
            if (GlobalVariable.TimeMaster != null) {
                if (HelperRoles.IsTimeMaster(PlayerControl.LocalPlayer.PlayerId)) {
                    GlobalVariable.buttonTime.SetCanUse(false);
                }
            }

            if (GlobalVariable.PsychicList != null) {
                if (HelperRoles.IsPsychic(PlayerControl.LocalPlayer.PlayerId)) {
                    GlobalVariable.buttonPsychic.SetCanUse(false);
                }
            }
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Awake))]
    public static class MeetingStartPatch {
        public static void Postfix(MeetingHud __instance) {
            Systems.TimeMaster.Time.StopRewind();
            Systems.Psychic.MiniMapPlayers.SyncOverlay(false);
        }
    }
}
