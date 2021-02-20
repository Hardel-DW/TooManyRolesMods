using System;
using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;

namespace RolesMods.Utility {
    class HelperSprite {
        public static Sprite LoadSpriteFromEmbeddedResources(string resource, float PixelPerUnit) {
            try {
                System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.IO.Stream myStream = myAssembly.GetManifestResourceStream(resource);
                byte[] image = new byte[myStream.Length];
                myStream.Read(image, 0, (int) myStream.Length);
                Texture2D myTexture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                LoadImage(myTexture, image, true);
                return Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), PixelPerUnit);
            } catch { }
            return null;
        }

        public static List<Sprite> LoadTileTextureEmbed(string resource, float PixelPerUnit, int TileX, int TileY) {
            try {
                List<Sprite> sprites = new List<Sprite>();
                System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.IO.Stream myStream = myAssembly.GetManifestResourceStream(resource);
                byte[] image = new byte[myStream.Length];
                myStream.Read(image, 0, (int) myStream.Length);
                Texture2D myTexture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                LoadImage(myTexture, image, true);

                int sizeX = (int) ((float) (myTexture.width / (float) TileX));
                int sizeY = (int) ((float) (myTexture.height / (float) TileY));

                for (int x = 1; x <= TileX; x++) {
                    for (int y = 1; y <= TileY; y++) {
                        sprites.Add(Sprite.Create(myTexture, new Rect(myTexture.width - (sizeX * x), myTexture.height - (sizeY * y), sizeX, sizeY), new Vector2(0.5f, 0.5f), PixelPerUnit, 100, SpriteMeshType.FullRect, Vector4.zero));
                    }
                }

                return sprites;
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
