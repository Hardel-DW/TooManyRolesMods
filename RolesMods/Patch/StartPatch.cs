using HarmonyLib;

namespace RolesMods.Patch {
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    class GameEndedPatch {
        public static void Postfix(ShipStatus __instance) {
            GlobalVariable.isGameStarted = true;
            Systems.TimeMaster.Time.pointsInTime.Clear();
            Systems.Investigator.FootPrint.allFootprint.Clear();
            GlobalVariable.buttonTime.MaxTimer = RolesMods.TimeMasterCooldown.GetValue();
            GlobalVariable.buttonTime.EffectDuration = RolesMods.TimeMasterDuration.GetValue() / 2;
            Systems.TimeMaster.Time.recordTime = RolesMods.TimeMasterDuration.GetValue();
        }
    }
}