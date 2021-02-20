using HarmonyLib;

namespace RolesMods.Systems.Lighter {

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
    public static class VisionPatch {

        public static bool Prefix(GameData.PlayerInfo player, ShipStatus __instance, ref float __result) {
            if (player.PlayerId == PlayerControl.LocalPlayer.PlayerId) {
                __result = 10f;
                return false;
            }

            return true;
        }
	}
}
