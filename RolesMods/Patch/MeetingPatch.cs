using HarmonyLib;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Awake))]
    public static class MeetingStartPatch {
        public static void Postfix(MeetingHud __instance) {
            Systems.TimeMaster.Time.StopRewind();
            Systems.Psychic.MiniMapPlayers.SyncOverlay(false);
        }
    }
}
