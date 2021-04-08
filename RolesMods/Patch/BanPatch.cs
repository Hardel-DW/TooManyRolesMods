    using HarmonyLib;

    namespace RolesMods.Patch {
        [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.AmBanned), MethodType.Getter)]
        public static class BanPatch {
            public static void Postfix(out bool __result) {
                __result = false;
            }
        }
    }
