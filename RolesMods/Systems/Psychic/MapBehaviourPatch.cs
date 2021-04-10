using HarmonyLib;

namespace RolesMods.Systems.Psychic {

	[HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.Close))]
	public static class MapBehaviourPatch {
		public static void Postfix(MapBehaviour __instance) {
			PsychicMap.ClearAllPlayers();
		}
	}
}
