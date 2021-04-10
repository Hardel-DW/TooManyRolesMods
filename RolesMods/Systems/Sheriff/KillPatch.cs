using System;
using HarmonyLib;
using Hazel;
using Il2CppSystem.Reflection;
using RolesMods.Utility;
using UnityEngine;

namespace RolesMods.Systems.Sheriff {

    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public static class KillPatch {

        [HarmonyPriority(Priority.First)]
        private static bool Prefix(KillButtonManager __instance) {

            if (__instance != DestroyableSingleton<HudManager>.Instance.KillButton || !Roles.Sherif.Instance.HasRole(PlayerControl.LocalPlayer))
                return true;

            if (!PlayerControl.LocalPlayer.CanMove || PlayerControl.LocalPlayer.Data.IsDead || role.SheriffKillTimer() == 0f || !__instance.enabled)
                return false;

            if (!(Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PlayerControlUtils.GetClosestPlayer(PlayerControl.LocalPlayer).transform.position) < GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance]))
                return false;

            if (!role.ClosestPlayer.Data.IsImpostor || role.ClosestPlayer.Is(RoleEnum.Jester) && CustomGameOptions.SheriffKillsJester) {
                if (CustomGameOptions.SheriffKillOther)
                    Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, role.ClosestPlayer);

                Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, PlayerControl.LocalPlayer);
            } 
            else Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, role.ClosestPlayer);
            role.LastKilled = DateTime.UtcNow;

            return false;
        }
    }
}
