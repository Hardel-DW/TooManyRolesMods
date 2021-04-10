using HarmonyLib;
using UnityEngine;

namespace RolesMods.Systems.TimeMaster {

    [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Begin))]
    class VitalsPatch {

        public static bool Prefix(VitalsMinigame __instance) {
            if ((Roles.TimeMaster.UsableVitals.GetValue() || Button.UseNumber == 0) && Roles.TimeMaster.Instance.HasRole(PlayerControl.LocalPlayer)) {
                Object.Destroy(__instance.gameObject);
                return false;
            }

            return true;
        }
    }
}
