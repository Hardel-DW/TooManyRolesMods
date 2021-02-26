using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Systems.Psychic {

	[HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.ShowNormalMap))]
	public static class MapBehaviourShowNormalMapPatch {
		public static void Postfix(MapBehaviour __instance) {
			if (!ShipStatus.Instance) { return; }

			if (GlobalVariable.ispsychicActivated && HelperRoles.IsPsychic(PlayerControl.LocalPlayer.PlayerId)) {
				MiniMapPlayers.ClearAllPlayers();
				__instance.ColorControl.SetColor(Palette.Purple);

				var temp = new List<SpriteRenderer>();
				foreach (var player in PlayerControl.AllPlayerControls) {
					if (!player.Data.IsDead) {				
						SpriteRenderer herePoint = Object.Instantiate(__instance.HerePoint, __instance.HerePoint.transform.parent);

						if (RolesMods.AnonymousPlayerMinimap.GetValue()) {
							PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, herePoint);
						} else {
							player.SetPlayerMaterialColors(herePoint);
							TextRenderer text = Object.Instantiate(HudManager.Instance.TaskText, __instance.HerePoint.transform.parent);
							text.Text = player.Data.PlayerName;
							text.transform.SetParent(herePoint.transform);
							text.transform.position = herePoint.transform.position;
							text.transform.localScale = herePoint.transform.localScale;
							text.Centered = true;
							GlobalVariable.texts.Add(text);
						}

						temp.Add(herePoint);
					}
				}
				GlobalVariable.herePoints = temp;
			}
		}
	}

	[HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.FixedUpdate))]
	public static class MapBehaviourFixedUpdatePatch {
		public static void Postfix(MapBehaviour __instance) {
			if (!ShipStatus.Instance) { return; }

			if (GlobalVariable.ispsychicActivated && HelperRoles.IsPsychic(PlayerControl.LocalPlayer.PlayerId)) {
				for (var i = 0; i < GlobalVariable.herePoints.Count; i++) {
					if (!PlayerControl.AllPlayerControls[i].Data.IsDead) {
						var vector = PlayerControl.AllPlayerControls[i].transform.position;
						vector /= ShipStatus.Instance.MapScale;
						vector.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
						vector.z = -1f;
						GlobalVariable.herePoints[i].transform.localPosition = vector;
						if (!RolesMods.AnonymousPlayerMinimap.GetValue()) {
							GlobalVariable.texts[i].transform.position = GlobalVariable.herePoints[i].transform.position + new Vector3(0, 0.3f, 0);
							GlobalVariable.texts[i].Text = PlayerControl.AllPlayerControls[i].Data.PlayerName;
						}
					}	
				}
			}
		}
	}

	[HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.Close))]
	public static class MapBehaviourClosePatch {
		public static void Postfix(MapBehaviour __instance) {
			try {
				MiniMapPlayers.ClearAllPlayers();
			} catch { }
		}
	}

	public static class MiniMapPlayers {
		public static void ClearAllPlayers() {
            try {
				GlobalVariable.herePoints.ToList().ForEach(x => Object.Destroy(x.gameObject));
				GlobalVariable.texts.ToList().ForEach(x => Object.Destroy(x.gameObject));
				GlobalVariable.herePoints.Clear();
				GlobalVariable.texts.Clear();
            } catch {}
		}
	}
}
