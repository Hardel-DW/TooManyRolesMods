using HarmonyLib;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class VersionShowerPatch {
        public static void Postfix(VersionShower __instance) {
            Reactor.Patches.ReactorVersionShower.Text.Text += "\n[2EADFFFF]Too Many Roles[] by Hardel";
        }
    }
}