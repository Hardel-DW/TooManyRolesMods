using HarmonyLib;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class VersionShowerPatch {
        public static void Postfix(VersionShower __instance) {
            __instance.text.Text += " + [2EADFFFF]RolesMods[] by Hardel";
        }
    }
}