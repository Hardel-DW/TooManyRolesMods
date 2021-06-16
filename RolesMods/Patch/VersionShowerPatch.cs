using HarmonyLib;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class VersionShowerPatch {
        public static void Postfix(VersionShower __instance) {
            HardelAPI.HarionVersionShower.Text.text += "\n<color=#2EADFFFF>Too Many Roles</color> by Hardel";
        }
    }
}