using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Systems.Psychic {

	public static class PsychicSystems {
		public static bool isPsychicActivated = false;
		public static List<SpriteRenderer> herePoints = new List<SpriteRenderer>();
		public static List<TextRenderer> texts = new List<TextRenderer>();
		public static GameObject psychicOverlay;

		public static void ClearAllPlayers() {
			try {
				herePoints.ToList().ForEach(x => Object.Destroy(x.gameObject));
				texts.ToList().ForEach(x => Object.Destroy(x.gameObject));
				herePoints.Clear();
				texts.Clear();
			} catch { }
		}

		public static void SyncOverlay(bool show) {
			MessageWriter write = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.SendOverlayPsychic, SendOption.None, -1);
			write.Write(show);
			AmongUsClient.Instance.FinishRpcImmediately(write);

			if (psychicOverlay != null)
				psychicOverlay.SetActive(show);
		}
	}

	[HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.ShowNormalMap))]
	public static class MapBehaviourShowNormalMapPatch {
		public static void Postfix(MapBehaviour __instance) {
			if (!ShipStatus.Instance)
				return;

			if (PsychicSystems.isPsychicActivated && Roles.Psychic.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId)) {
				PsychicSystems.ClearAllPlayers();
				__instance.ColorControl.SetColor(new Color(0.894f, 0f, 1f, 1f));

				var playerSpriteIcon = new List<SpriteRenderer>();
				foreach (var player in PlayerControl.AllPlayerControls) {
					if (!player.Data.IsDead) {				
						SpriteRenderer herePoint = Object.Instantiate(__instance.HerePoint, __instance.HerePoint.transform.parent);

						if (Roles.Psychic.AnonymousPlayerMinimap.GetValue()) {
							PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, herePoint);
						} else {
							player.SetPlayerMaterialColors(herePoint);
							TextRenderer text = Object.Instantiate(HudManager.Instance.TaskText, __instance.HerePoint.transform.parent);
							text.Text = player.Data.PlayerName;
							text.transform.SetParent(herePoint.transform);
							text.transform.position = herePoint.transform.position;
							text.transform.localScale = herePoint.transform.localScale;
							text.Centered = true;
							PsychicSystems.texts.Add(text);
						}

						playerSpriteIcon.Add(herePoint);
					}
				}
				PsychicSystems.herePoints = playerSpriteIcon;
			}
		}
	}

	[HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.FixedUpdate))]
	public static class MapBehaviourFixedUpdatePatch {
		public static void Postfix(MapBehaviour __instance) {
			if (!ShipStatus.Instance) 
				return;

			if (PsychicSystems.isPsychicActivated && Roles.Psychic.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId)) {
				for (var i = 0; i < PsychicSystems.herePoints.Count; i++) {
					if (!PlayerControl.AllPlayerControls[i].Data.IsDead) {
						var vector = PlayerControl.AllPlayerControls[i].transform.position;
						vector /= ShipStatus.Instance.MapScale;
						vector.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
						vector.z = -1f;
						PsychicSystems.herePoints[i].transform.localPosition = vector;
						if (!Roles.Psychic.AnonymousPlayerMinimap.GetValue()) {
							PsychicSystems.texts[i].transform.position = PsychicSystems.herePoints[i].transform.position + new Vector3(0, 0.3f, 0);
							PsychicSystems.texts[i].Text = PlayerControl.AllPlayerControls[i].Data.PlayerName;
						}
					}	
				}
			}
		}
	}

	[HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.Close))]
	public static class MapBehaviourClosePatch {
		public static void Postfix(MapBehaviour __instance) {
			PsychicSystems.ClearAllPlayers();
		}
	}
}
