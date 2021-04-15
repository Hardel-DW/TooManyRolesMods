using System;
using System.Collections.Generic;
using System.Linq;
using Reactor;
using TMPro;
using UnityEngine;

namespace RolesMods.Utility {

    [RegisterInIl2Cpp]
    class MinimapAllHerePoint : MonoBehaviour {
        public MinimapAllHerePoint(IntPtr ptr) : base(ptr) { }
        public static MinimapAllHerePoint Instance;
        private MapBehaviour Map = null;
        private List<SpriteRenderer> playerSpriteIcon = new List<SpriteRenderer>();
        private List<SpriteRenderer> HerePoints = new List<SpriteRenderer>();
        private List<TextMeshPro> HerePointTexts = new List<TextMeshPro>();
        private DateTime CreationTime = DateTime.Now;
        public bool AnonymousPlayerMinimap = false;
        public bool ShowDead = false;
        public float Duration = float.MaxValue;

        private void OnEnable() {
            gameObject.AddComponent<BoxCollider2D>();
            Map = GetComponent<MapBehaviour>();

            List<SpriteRenderer> playerSpriteIcon = new List<SpriteRenderer>();
            foreach (var player in PlayerControl.AllPlayerControls) {
                if (!player.Data.IsDead) {
                    SpriteRenderer herePoint = Instantiate(Map.HerePoint, Map.HerePoint.transform.parent);

                    if (AnonymousPlayerMinimap) {
                        PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, herePoint);
                    } else {
                        player.SetPlayerMaterialColors(herePoint);
                        TextMeshPro text = Instantiate(HudManager.Instance.TaskText, Map.HerePoint.transform.parent);
                        text.text = player.Data.PlayerName;
                        text.transform.SetParent(herePoint.transform);
                        text.transform.position = herePoint.transform.position;
                        text.transform.localScale = herePoint.transform.localScale;
                        HerePointTexts.Add(text);
                    }

                    playerSpriteIcon.Add(herePoint);
                }
            }
            HerePoints = playerSpriteIcon;

            if (Instance)
                Destroy(Instance);

            Instance = this;
        }

        private void OnDestroy() {
            Instance = null;
        }

        private void Update() {
            if (CreationTime.AddSeconds(Duration) < DateTime.Now) {
                StopMinimap();
                return;
            }

            for (var i = 0; i < HerePoints.Count; i++) {
                if (!PlayerControl.AllPlayerControls[i].Data.IsDead) {
                    var vector = PlayerControl.AllPlayerControls[i].transform.position;
                    vector /= ShipStatus.Instance.MapScale;
                    vector.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
                    vector.z = -1f;
                    HerePoints[i].transform.localPosition = vector;
                    if (AnonymousPlayerMinimap) {
                        HerePointTexts[i].transform.position = HerePoints[i].transform.position + new Vector3(0, 0.3f, 0);
                        HerePointTexts[i].text = PlayerControl.AllPlayerControls[i].Data.PlayerName;
                    }
                }
            }
        }

        private void StopMinimap() {
            HerePoints.ToList().ForEach(x => Destroy(x.gameObject));
            HerePointTexts.ToList().ForEach(x => Destroy(x.gameObject));
            HerePoints.Clear();
            HerePointTexts.Clear();
            DestroyableSingleton<HudManager>.Instance.ShowMap((Action<MapBehaviour>) (map => map.gameObject.SetActive(false)));
            DestroyableSingleton<HudManager>.Instance.SetHudActive(true);
        }
    }
}
