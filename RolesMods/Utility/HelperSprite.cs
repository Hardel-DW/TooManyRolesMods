using Reactor.Extensions;
using System;
using System.IO;
using System.Reflection;
using UnhollowerBaseLib;
using UnityEngine;

namespace RolesMods.Utility {
    class HelperSprite {
        public static Sprite LoadSpriteFromEmbeddedResources(string resource, float PixelPerUnit) {
            try {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream(resource);
                byte[] image = new byte[myStream.Length];
                myStream.Read(image, 0, (int) myStream.Length);
                Texture2D myTexture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                LoadImage(myTexture, image, true);
                return Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), PixelPerUnit);
            } catch { }
            return null;
        }

        public static Sprite LoadHatSprite(string resource) {
            try {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                Texture2D tex = GUIExtensions.CreateEmptyTexture();
                Stream imageStream = assembly.GetManifestResourceStream(resource);
                byte[] img = imageStream.ReadFully();
                LoadImage(tex, img, true);
                tex.DontDestroy();
                Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, (float) tex.width, (float) tex.height), new Vector2(0.5f, 0.8f), 225f);
                sprite.DontDestroy();
                return sprite;
            } catch { }
            return null;
        }

        internal delegate bool d_LoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
        internal static d_LoadImage iCall_LoadImage;
        private static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable) {
            if (iCall_LoadImage == null)
                iCall_LoadImage = IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");

            var il2cppArray = (Il2CppStructArray<byte>) data;

            return iCall_LoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
        }
    }
}
