using Harion.Reactor;
using Harion.Utility.Utils;
using System.Reflection;
using UnityEngine;

namespace RolesMods {
    public static class ResourceLoader {
        private static readonly Assembly myAsembly = Assembly.GetExecutingAssembly();
        // Button Sprite
        public static Sprite RewindRedSprite;
        public static Sprite RewindSprite;
        public static Sprite CorruptSprite;
        public static Sprite TargetSprite;
        public static Sprite ForsightSprite;

        // Roles Sprite
        public static Sprite FootprintSprite;
        public static Sprite FootprintRoundSprite;
        public static Sprite OverlaySprite;
        public static Sprite AbstainButton;
        public static Sprite SwapSprite;
        public static Sprite SwapDisableSprite;

        public static void LoadAssets() {
            var resourceSteam = myAsembly.GetManifestResourceStream("RolesMods.Resources.bundle-toomanyrole");
            var assetBundle = AssetBundle.LoadFromMemory(resourceSteam.ReadFully());

            RewindRedSprite = assetBundle.LoadAsset<Sprite>("Rewind-Red").DontDestroy();
            RewindSprite = assetBundle.LoadAsset<Sprite>("Rewind").DontDestroy();
            CorruptSprite = assetBundle.LoadAsset<Sprite>("CorruptButton").DontDestroy();
            TargetSprite = assetBundle.LoadAsset<Sprite>("Target").DontDestroy();
            ForsightSprite = assetBundle.LoadAsset<Sprite>("Forsight").DontDestroy();

            FootprintSprite = assetBundle.LoadAsset<Sprite>("Footprint").DontDestroy();
            FootprintRoundSprite = assetBundle.LoadAsset<Sprite>("Footprint-Round").DontDestroy();
            OverlaySprite = assetBundle.LoadAsset<Sprite>("Overlay").DontDestroy();
            AbstainButton = assetBundle.LoadAsset<Sprite>("Abstain").DontDestroy();
            SwapSprite = assetBundle.LoadAsset<Sprite>("Switch").DontDestroy();
            SwapDisableSprite = assetBundle.LoadAsset<Sprite>("SwitchDisabled").DontDestroy();
        }
    }
}
