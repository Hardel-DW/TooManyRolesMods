/*using HarmonyLib;
using UnityEngine;

namespace RolesMods.Patch {
    [HarmonyPatch]
    class GameOptionsMenuPatch {
        static float defaultBounds = 0f;

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
        class Start {
            static void Postfix(ref GameOptionsMenu __instance) {
                defaultBounds = __instance.GetComponentInParent<Scroller>().YBounds.max;
            }
        }

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
        class Update {
            static void Postfix(ref GameOptionsMenu __instance) {
                __instance.GetComponentInParent<Scroller>().YBounds.max = 19f;
            }
        }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudGameOptionsPatch {
        public const float increment = 0.2f;
        public static FloatRange scrollBounds = new FloatRange(2.9f, 7.8f);

        public static void Postfix(HudManager __instance) {
            if (PlayerControl.LocalPlayer == null || !PlayerControl.LocalPlayer.CanMove)
                return;

            var pos = __instance.GameSettings.transform.localPosition;
            if (Input.mouseScrollDelta.y > 0F)
                pos = new Vector3(pos.x, Mathf.Clamp(pos.y - increment, scrollBounds.min, scrollBounds.max), pos.z);
            else if (Input.mouseScrollDelta.y < 0f)
                pos = new Vector3(pos.x, Mathf.Clamp(pos.y + increment, scrollBounds.min, scrollBounds.max), pos.z);

            __instance.GameSettings.transform.localPosition = pos;

        }
    }
}
*/