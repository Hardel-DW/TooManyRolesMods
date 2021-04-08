using HarmonyLib;
using RolesMods.Utility.Enumerations;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class TasksPatch {
        public static void Postfix(PlayerControl __instance) {
            foreach (var Role in RoleManager.AllRoles)
                if (Role.GiveTasksAt == Moment.StartGame && Role.HasRole(PlayerControl.LocalPlayer.PlayerId))
                    Role.AddImportantTaks(__instance);
        }
    }
}
