using HarmonyLib;
using RolesMods.Utility.Enumerations;
using UnityEngine;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Die))]
    class DiePatch {
        public static void Prefix(PlayerControl __instance) {
            foreach (var Role in RoleManager.AllRoles) {
                Role.OnDie(__instance);
                if (Role.HasRole(__instance.PlayerId)) {
                    if (Role.GiveRoleAt == Moment.OnDie) { }

                    if (Role.GiveTasksAt == Moment.OnDie)
                        Role.AddImportantTasks(__instance);
                }
            }
        }
    }
}
