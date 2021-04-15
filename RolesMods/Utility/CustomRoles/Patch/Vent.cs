using HarmonyLib;
using UnityEngine;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(Vent), "CanUse")]
    public static class VentPatch {
        public static bool Prefix(Vent __instance, ref float __result, [HarmonyArgument(0)] GameData.PlayerInfo playerInfo, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse) {
            bool tempCanUse = false;
            bool tempCouldUse = false;

            foreach (var Role in RoleManager.AllRoles) {
                if (!Role.CanVent && !PlayerControl.LocalPlayer.Data.IsImpostor)
                    continue;

                float maxFloat = float.MaxValue;
                PlayerControl player = playerInfo.Object;
                tempCouldUse = playerInfo.IsImpostor || Role.HasRole(playerInfo.PlayerId) && !playerInfo.IsDead && (player.CanMove || player.inVent);
                tempCanUse = tempCouldUse;
                if (tempCanUse) {
                    maxFloat = Vector2.Distance(player.GetTruePosition(), __instance.transform.position);
                    tempCanUse &= maxFloat <= __instance.UsableDistance;
                }

                __result = maxFloat;
                if (tempCanUse)
                    break;
            }

            couldUse = tempCouldUse;
            canUse = tempCanUse;
            return false;
        }
    }
}
