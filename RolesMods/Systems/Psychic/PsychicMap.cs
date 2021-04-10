using System;
using Hazel;
using Reactor;
using Reactor.Extensions;
using UnityEngine;
using RolesMods.Utility;
using System.Collections.Generic;
using System.Linq;

namespace RolesMods.Systems.Psychic {

    [RegisterInIl2Cpp]
    class PsychicMap : MonoBehaviour {
        public PsychicMap(IntPtr ptr) : base(ptr) { }
        public static PsychicMap Instance;
        private MapBehaviour Map;

        public static List<SpriteRenderer> herePoints = new List<SpriteRenderer>();
        public static List<TextRenderer> texts = new List<TextRenderer>();
        public static bool isPsychicActivated = false;

        private void OnEnable() {
            gameObject.AddComponent<BoxCollider2D>();
            Map = GetComponent<MapBehaviour>();
            if (Instance)
                Destroy(Instance);

            Instance = this;
        }

        public void Start() {
            if (!ShipStatus.Instance)
                return;

            if (isPsychicActivated && Roles.Psychic.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId)) {
                ClearAllPlayers();
                Map.ColorControl.SetColor(new Color(0.894f, 0f, 1f, 1f));

                var playerSpriteIcon = new List<SpriteRenderer>();
                foreach (var player in PlayerControl.AllPlayerControls) {
                    if (!player.Data.IsDead) {
                        SpriteRenderer herePoint = Instantiate(Map.HerePoint, Map.HerePoint.transform.parent);

                        if (Roles.Psychic.AnonymousPlayerMinimap.GetValue()) {
                            PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, herePoint);
                        } else {
                            player.SetPlayerMaterialColors(herePoint);
                            TextRenderer text = Instantiate(HudManager.Instance.TaskText, Map.HerePoint.transform.parent);
                            text.Text = player.Data.PlayerName;
                            text.transform.SetParent(herePoint.transform);
                            text.transform.position = herePoint.transform.position;
                            text.transform.localScale = herePoint.transform.localScale;
                            text.Centered = true;
                            texts.Add(text);
                        }

                        playerSpriteIcon.Add(herePoint);
                    }
                }
                herePoints = playerSpriteIcon;
            }
        }

        public void FixedUpdate() {
            if (!ShipStatus.Instance)
                return;

            if (isPsychicActivated && Roles.Psychic.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId)) {
                for (var i = 0; i < herePoints.Count; i++) {
                    if (!PlayerControl.AllPlayerControls[i].Data.IsDead) {
                        var vector = PlayerControl.AllPlayerControls[i].transform.position;
                        vector /= ShipStatus.Instance.MapScale;
                        vector.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
                        vector.z = -1f;
                        herePoints[i].transform.localPosition = vector;
                        if (!Roles.Psychic.AnonymousPlayerMinimap.GetValue()) {
                            texts[i].transform.position = herePoints[i].transform.position + new Vector3(0, 0.3f, 0);
                            texts[i].Text = PlayerControl.AllPlayerControls[i].Data.PlayerName;
                        }
                    }
                }
            }
        }

        public static void ClearAllPlayers() {
            try {
                herePoints.ToList().ForEach(x => Destroy(x.gameObject));
                texts.ToList().ForEach(x => Destroy(x.gameObject));
                herePoints.Clear();
                texts.Clear();
            } catch { }
        }

        private void OnDestroy() {
            Instance = null;
        }
    }
}       
