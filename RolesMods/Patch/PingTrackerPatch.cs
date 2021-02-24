using HarmonyLib;
using UnityEngine;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    [HarmonyPriority(Priority.First)]
    public static class PingTrackerPatch {
        private static Vector3 lastDist = Vector3.zero;

        public static void Postfix(ref PingTracker __instance) {
            if (!GlobalVariable.isGameStarted) {
                AspectPosition aspect = __instance.text.gameObject.GetComponent<AspectPosition>();
                if (aspect.DistanceFromEdge != lastDist) {
                    aspect.DistanceFromEdge += new Vector3(0.6f, 0);
                    aspect.AdjustPosition();

                    lastDist = aspect.DistanceFromEdge;
                }
                __instance.text.Text += $"\n[2294E6FF]Too Many Roles Mods[] \ndiscord.gg/AP9axbXXNC";
            }
        }
    }
}
