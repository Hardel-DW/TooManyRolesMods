using HarmonyLib;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class VersionShowerPatch {
        public static void Postfix(VersionShower __instance) {
            Reactor.Patches.ReactorVersionShower.Text.text += "\n<color=#2EADFFFF>Too Many Roles</color> by Hardel";
        }
    }
}