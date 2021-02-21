using HarmonyLib;
using RolesMods.Utility;

namespace RolesMods.Systems.Lighter {

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
    public class ShipStatusPatch {
        public static void Postfix(ref float __result, GameData.PlayerInfo IIEKJBMPELC) {
            if (GlobalVariable.LightersList != null && HelperRoles.IsLighter(IIEKJBMPELC.PlayerId)) {
                bool lightSabotage = false;
                bool canSeeDuringLight = RolesMods.LighterSabotageVision.GetValue();

                foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks) {
                    if (task.TaskType == TaskTypes.FixLights) {
                        lightSabotage = true;
                    }
                }

                if (!lightSabotage)
                    __result *= RolesMods.LighterMultiplier.GetValue();

                RolesMods.Logger.LogInfo(lightSabotage);
                RolesMods.Logger.LogInfo(canSeeDuringLight);
                if (lightSabotage && canSeeDuringLight) {
                    RolesMods.Logger.LogInfo(PlayerControl.GameOptions.CrewLightMod);
                    RolesMods.Logger.LogInfo(RolesMods.LighterMultiplier.GetValue());
                    if (ShipStatus.Instance != null) return;
                    __result = PlayerControl.GameOptions.CrewLightMod * RolesMods.LighterMultiplier.GetValue();
                }
            }
        }
    }
}
