using Harion.Reactor;
using Harion.Utility.Utils;
using System.Reflection;
using UnityEngine;

namespace RolesMods {
    public static class ResourceLoader {
        private static readonly Assembly myAsembly = Assembly.GetExecutingAssembly();
        public static GameObject Overlay;
        public static Sprite RewindRedSprite;
        public static Sprite RewindSprite;
        public static Sprite FootprintSprite;
        public static Sprite OverlaySprite;
        public static Sprite ForsightSprite;
        public static Sprite AbstainButton;
        public static void LoadAssets() {
            var resourceSteam = myAsembly.GetManifestResourceStream("RolesMods.Resources.bundle-toomanyrole");
            var assetBundle = AssetBundle.LoadFromMemory(resourceSteam.ReadFully());

            Overlay = assetBundle.LoadAsset<GameObject>("Overlay.prefab").DontDestroy();
            RewindRedSprite = assetBundle.LoadAsset<Sprite>("Rewind-Red").DontDestroy();
            RewindSprite = assetBundle.LoadAsset<Sprite>("Rewind").DontDestroy();
            FootprintSprite = assetBundle.LoadAsset<Sprite>("Footprint").DontDestroy();
            OverlaySprite = assetBundle.LoadAsset<Sprite>("Overlay").DontDestroy();
            ForsightSprite = assetBundle.LoadAsset<Sprite>("Forsight").DontDestroy();
        }
    }
}
