/*using HarmonyLib;

namespace RolesMods.Systems.Sheriff {

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    static class MurderPlayerPatch {
        static bool trueImpostorIntance = false;

        static void Prefix(ref PlayerControl __instance, [HarmonyArgument(0)] ref PlayerControl target) {
            trueImpostorIntance = __instance.Data.IsImpostor;

            if (!trueImpostorIntance && !target.Data.IsImpostor)
                __instance.Data.IsImpostor = true;
        }

        static void Postfix(ref PlayerControl __instance, [HarmonyArgument(0)] ref PlayerControl target) {
            if (!trueImpostorIntance)
                __instance.Data.IsImpostor = false;
        }
    }
}*/