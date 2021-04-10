using HarmonyLib;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Awake))]
    public static class MeetingStartPatch {
        public static void Postfix(MeetingHud __instance) {
            foreach (var Role in RoleManager.AllRoles)
                Role.OnMeetingStart();
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class MeetingClosePatch {
        public static void Postfix(MeetingHud __instance) {
            foreach (var Role in RoleManager.AllRoles)
                Role.OnMeetingEnd();
        }
    }

}
