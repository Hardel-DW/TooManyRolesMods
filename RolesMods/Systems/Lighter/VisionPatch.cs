using HarmonyLib;

namespace RolesMods.Systems.Lighter {

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
    public class ShipStatusPatch {
        public static void Postfix(ref float __result, ShipStatus __instance, [HarmonyArgument(0)] GameData.PlayerInfo PlayerData) {
            if (GlobalVariable.LightersList != null && HelperRoles.IsLighter(PlayerData.PlayerId)) {
                bool lightSabotage = false;
                bool canSeeDuringLight = RolesMods.LighterSabotageVision.GetValue();

                foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks)
                    if (task.TaskType == TaskTypes.FixLights)  
                        lightSabotage = true;

                if ((lightSabotage && canSeeDuringLight) || !lightSabotage)
                    __result = __instance.MaxLightRadius * PlayerControl.GameOptions.CrewLightMod * RolesMods.LighterMultiplier.GetValue();
            }
        }
    }
}
