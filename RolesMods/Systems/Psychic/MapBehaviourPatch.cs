using HarmonyLib;

namespace RolesMods.Systems.Psychic {

	[HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.Close))]
	public static class MapBehaviourClosePatch {
		public static void Postfix(MapBehaviour __instance) {
			PsychicMap.ClearAllPlayers();
		}
	}

	[HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.ShowNormalMap))]
	public static class MapBehaviourShowPatch {
		public static void Postfix(MapBehaviour __instance) {
			PsychicMap.StartMap();
		}
	}
}
