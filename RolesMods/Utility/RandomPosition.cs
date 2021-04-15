using RolesMods.Utility.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Utility {
    class RandomPosition {
        public static Dictionary<MapType, Vector2[]> MapLocations = new Dictionary<MapType, Vector2[]>() {
            { MapType.Skeld, new Vector2[] {
                new Vector2(-12.594f, -4.179f),
                new Vector2(-22.746f, -7.206f),
                new Vector2(-19.269f, -9.544f),
                new Vector2(-7.772f, -11.655f),
                new Vector2(-9.825f, -9.023f),
                new Vector2(-2.874f, -16.936f),
                new Vector2(0.336f, -9.152f),
                new Vector2(5.731f, -9.607f),
                new Vector2(-5.044f, -2.642f),
                new Vector2(-1.013f, 5.975f),
                new Vector2(10.772f, 1.998f),
                new Vector2(6.409f, -4.710f),
                new Vector2(17.997f, -5.689f),
                new Vector2(11.165f, -10.334f),
                new Vector2(7.522f, -14.285f),
                new Vector2(1.707f, -14.937f),
                new Vector2(-3.685f, -11.657f),
                new Vector2(-14.163f, -6.832f),
                new Vector2(-18.551f, 2.625f),
                new Vector2(-7.556f, -2.124f)
            }},

            { MapType.Polus, new Vector2[] {
                new Vector2(4.458f, -3.387f),
                new Vector2(3.801F, -7.584F),
                new Vector2(7.193f, -13.089f),
                new Vector2(4.042f, -11.233f),
                new Vector2(0.660f, -15.868f),
                new Vector2(1.516f, -18.669f),
                new Vector2(2.331f, -24.491f),
                new Vector2(9.231f, -25.351f),
                new Vector2(12.681f, -24.541f),
                new Vector2(12.489f, -17.160f),
                new Vector2(17.958f, -25.710f),
                new Vector2(22.225f, -25.008f),
                new Vector2(23.933f, -20.589f),
                new Vector2(31.295f, -11.321f),
                new Vector2(18.055f, -13.020f),
                new Vector2(12.892f, -17.317f),
                new Vector2(6.582f -17,113f),
                new Vector2(23.639f, -2.799f),
                new Vector2(24.928f, -6.877f),
                new Vector2(32.344f, -10.047f),
                new Vector2(34.852f, -5.208f),
                new Vector2(40.516f, -8.102f),
                new Vector2(36.291f, -22.012f)
            }},

            { MapType.MiraHQ, new Vector2[] {
                new Vector2(18.266f, -3.223f),
                new Vector2(28.257f, -2.250f),
                new Vector2(18.293f, 5.045f),
                new Vector2(28.276f, 2.735f),
                new Vector2(17.843f, 11.516f),
                new Vector2(13.750f, 17.214f),
                new Vector2(22.387f, 19.160f),
                new Vector2(13.862f, 23.878f),
                new Vector2(19.330f, 25.309f),
                new Vector2(16.177f, 3.085f),
                new Vector2(16.752f, -1.455f),
                new Vector2(11.755f, 10.300f),
                new Vector2(11.112f, 14.068f),
                new Vector2(2.444f, 13.352f),
                new Vector2(0.414f, 10.087f),
                new Vector2(-5.780f, -2.037f),
                new Vector2(16.752f, -1.455f),
                new Vector2(10.161f, 5.162f)
            }}
        };

        public static Vector2 GetRandomPosition() {
            var random = new System.Random();
            Vector2[] vectors = MapLocations[(MapType) ShipStatus.Instance.Type];
            return vectors[random.Next(vectors.Count())];
        }

        public static Vector2 GetRandomPositionUnique(List<Vector2> WhiteListPosition, float separation) {
            bool RerollPosition;
            List<Vector2> vectors = MapLocations[(MapType) ShipStatus.Instance.Type].ToList();
            Vector2 CurrentPosition;

            do {
                var random = new System.Random();
                RerollPosition = false;
                CurrentPosition = vectors[random.Next(vectors.Count())];

                foreach (var element in WhiteListPosition) {
                    float positionBeetween = Vector2.Distance(element, CurrentPosition);
                    if (positionBeetween < separation) {
                        RerollPosition = true;
                        vectors.Remove(CurrentPosition);
                    }
                }
            } while (RerollPosition && vectors.Count > 0);

            if (vectors.Count == 0)
                throw new Exception("Hardel API => In class RandomPosition, GetRandomPositionUnique Function : No position was found in list");

            return CurrentPosition;
        }
    }
}
