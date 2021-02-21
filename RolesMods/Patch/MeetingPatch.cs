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
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    public static class MeetingStartPatch {
        public static void Postfix(MeetingHud __instance) {
            if (GlobalVariable.TimeMaster != null) {
                if (HelperRoles.IsTimeMaster(PlayerControl.LocalPlayer.PlayerId)) {
                    GlobalVariable.buttonTime.SetCanUse(false);
                }
            }
        }
    }
}
