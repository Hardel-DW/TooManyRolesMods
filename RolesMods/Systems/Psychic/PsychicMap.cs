﻿using System;
using Reactor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace RolesMods.Systems.Psychic {

    [RegisterInIl2Cpp]
    class PsychicMap : MonoBehaviour {
        public PsychicMap(IntPtr ptr) : base(ptr) { }
        public static List<HerePointData> herePointsDatas = new List<HerePointData>();
        public static bool isPsychicActivated = false;
        public static PsychicMap Instance;
        private MapBehaviour Map;

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

            if (isPsychicActivated && Roles.Psychic.Instance.HasRole(PlayerControl.LocalPlayer)) {
                ClearAllPlayers();
                Map.ColorControl.SetColor(new Color(0.894f, 0f, 1f, 1f));

                foreach (var player in PlayerControl.AllPlayerControls) {
                    HerePointData herePointData = new HerePointData();
                    SpriteRenderer herePoint = Instantiate(Map.HerePoint, Map.HerePoint.transform.parent);

                    if (Roles.Psychic.AnonymousPlayerMinimap.GetValue()) {
                        PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, herePoint);
                    } else {
                        var text = Instantiate(HudManager.Instance.TaskText, Map.HerePoint.transform.parent);
                        player.SetPlayerMaterialColors(herePoint);
                        text.text = player.Data.PlayerName;
                        text.transform.SetParent(herePoint.transform);
                        text.transform.position = herePoint.transform.position;
                        text.transform.localScale = herePoint.transform.localScale;
                        herePointData.text = text;
                    }

                    herePointData.herePoint = herePoint;
                    herePointData.player = player;
                    herePointsDatas.Add(herePointData);
                }
            }
        }

        public void FixedUpdate() {
            if (!ShipStatus.Instance)
                return;

            if (isPsychicActivated && Roles.Psychic.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId)) {
                foreach (var herePointData in herePointsDatas) {
                    if (!herePointData.player.Data.IsDead) {
                        var vector = herePointData.player.transform.position;
                        vector /= ShipStatus.Instance.MapScale;
                        vector.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
                        vector.z = -1f;

                        herePointData.herePoint.transform.localPosition = vector;
                        if (!Roles.Psychic.AnonymousPlayerMinimap.GetValue()) {
                            herePointData.text.transform.position = herePointData.text.transform.position + new Vector3(0, 0.3f, 0);
                            herePointData.text.text = herePointData.player.Data.PlayerName;
                        }
                    }
                }
            }
        }

        public static void ClearAllPlayers() {
            try {
                herePointsDatas.ToList().ForEach(x => Destroy(x.herePoint.gameObject));
                herePointsDatas.ToList().ForEach(x => Destroy(x.text.gameObject));
                herePointsDatas.Clear();
            } catch { }
        }

        private void OnDestroy() {
            Instance = null;
        }
    }
}       
