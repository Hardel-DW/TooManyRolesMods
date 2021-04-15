using System;
using HarmonyLib;
using RolesMods.Utility;
using UnityEngine;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public static class KillPatch {

        [HarmonyPriority(Priority.First)]
        private static bool Prefix(KillButtonManager __instance) {
            foreach (var Role in RoleManager.AllRoles) {
                if (Role.WhiteListKill == null)
                    continue;

                PlayerControl ClosestPlayer = Role.GetClosestTarget(PlayerControl.LocalPlayer);
                if (__instance != DestroyableSingleton<HudManager>.Instance.KillButton || !Role.HasRole(PlayerControl.LocalPlayer))
                    return true;

                if (!PlayerControl.LocalPlayer.CanMove || PlayerControl.LocalPlayer.Data.IsDead || Role.KillTimer() != 0f || !__instance.enabled)
                    return false;

                if (!(Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ClosestPlayer.transform.position) < GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance]))
                    return false;

                Role.OnLocalAttempKill(PlayerControl.LocalPlayer, ClosestPlayer);
                Role.LastKilled = DateTime.UtcNow;
                return false;
            }

            return false;
        }
    }
}
