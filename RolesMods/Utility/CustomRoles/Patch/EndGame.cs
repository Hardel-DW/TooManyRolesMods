using HarmonyLib;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    public static class EndGameManagerPatch {
        public static bool Prefix(EndGameManager __instance) {
            foreach (var Role in RoleManager.AllRoles) {
                Role.ClearRole();
                Role.OnGameEnded();
            }

            return true;
        }
    }
}
