/*using System;
using HarmonyLib;
using RolesMods.Utility;
using UnityEngine;

namespace RolesMods.Systems.Sheriff {

    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public static class KillPatch {

        [HarmonyPriority(Priority.First)]
        private static bool Prefix(KillButtonManager __instance) {
            if (__instance != DestroyableSingleton<HudManager>.Instance.KillButton || !Roles.Sheriff.Instance.HasRole(PlayerControl.LocalPlayer))
                return true;

            if (!PlayerControl.LocalPlayer.CanMove || PlayerControl.LocalPlayer.Data.IsDead || Roles.Sheriff.SheriffKillTimer() != 0f || !__instance.enabled)
                return false;

            if (!(Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PlayerControlUtils.GetClosestPlayer(PlayerControl.LocalPlayer).transform.position) < GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance]))
                return false;

            PlayerControl ClosestPlayer = PlayerControlUtils.GetClosestPlayer(PlayerControl.LocalPlayer);
            if (ClosestPlayer.Data.IsImpostor || Roles.Jester.Instance.HasRole(ClosestPlayer)) {
                PlayerControl.LocalPlayer.RpcMurderPlayer(ClosestPlayer);
            } else {
                PlayerControl.LocalPlayer.RpcMurderPlayer(PlayerControl.LocalPlayer);
                if (Roles.Sheriff.SheriffCanDie.GetValue())
                    PlayerControl.LocalPlayer.RpcMurderPlayer(ClosestPlayer);
            }

            Roles.Sheriff.LastKilled = DateTime.UtcNow;
            return false;
        }
    }
}
*/