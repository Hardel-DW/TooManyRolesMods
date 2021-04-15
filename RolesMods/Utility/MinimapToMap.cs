using System;
using Reactor;
using UnityEngine;

namespace RolesMods.Utility {

    [RegisterInIl2Cpp]
    class MinimapToMap : MonoBehaviour {
        public MinimapToMap(IntPtr ptr) : base(ptr) { }
        public static MinimapToMap Instance;
        private MapBehaviour Map = null;
        public Action<Vector3> GetPosition = null;
        public CooldownButton Button = null;

        private void OnEnable() {
            gameObject.AddComponent<BoxCollider2D>();
            Map = GetComponent<MapBehaviour>();
            if (Instance)
                Destroy(Instance);

            Instance = this;
        }

        private void OnDestroy() {
            Instance = null;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                vector = MapBehaviour.Instance.HerePoint.transform.parent.transform.InverseTransformPoint(vector);
                vector.x /= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
                vector *= ShipStatus.Instance.MapScale;
                GetPosition(vector);
                CloseHud();
            }
        }

        private void CloseHud() {
            DestroyableSingleton<HudManager>.Instance.ShowMap((Action<MapBehaviour>) (map => map.gameObject.SetActive(false)));
            DestroyableSingleton<HudManager>.Instance.SetHudActive(true);
            Button.ForceClick(false);
        }
    }
}
