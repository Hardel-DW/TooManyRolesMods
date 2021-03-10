using System.Reflection;
using Reactor.Extensions;
using UnityEngine;

namespace RolesMods {
    public static class ResourceLoader {
        private static readonly Assembly myAsembly = Assembly.GetExecutingAssembly();
        public static GameObject Overlay;
        public static Sprite OverlaySprite;

        public static void LoadAssets() {
            var resourceSteam = myAsembly.GetManifestResourceStream("RolesMods.Resources.psychic");
            var assetBundle = AssetBundle.LoadFromMemory(resourceSteam.ReadFully());

            Overlay = assetBundle.LoadAsset<GameObject>("Overlay.prefab").DontDestroy();
            OverlaySprite = assetBundle.LoadAsset<Sprite>("Overlay").DontDestroy();
        }
    }
}
