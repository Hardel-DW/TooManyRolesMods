using Harion.Reactor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Systems.Investigator {
    class FootPrint {
        public static List<FootPrint> allFootprint = new List<FootPrint>();
        private Color footPrintColor;
        private GameObject footPrint;
        private SpriteRenderer spriteRenderer;
        public PlayerControl player;
        public Vector2 position;

        public FootPrint(float footPrintSize, float footPrintDuration, PlayerControl player) {
            this.footPrintColor = Palette.PlayerColors[(int) player.Data.ColorId];
            this.player = player;
            this.position = player.transform.position;
            if (Roles.Investigator.AnonymousFootPrint.GetValue())
                this.footPrintColor = new Color(0.2f, 0.2f, 0.2f, 1f);

            footPrint = new GameObject("FootPrint");
            footPrint.transform.position = position;
            footPrint.transform.localPosition = position;
            footPrint.transform.localScale = new Vector2(footPrintSize, footPrintSize);
            footPrint.transform.SetParent(player.transform.parent);
            footPrint.transform.Rotate(Vector3.forward * Vector2.SignedAngle(Vector2.up, player.gameObject.GetComponent<Rigidbody2D>().velocity));

            spriteRenderer = footPrint.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = ResourceLoader.FootprintSprite;
            spriteRenderer.color = footPrintColor;

            footPrint.SetActive(true);
            allFootprint.Add(this);
            Coroutines.Start(CoFadeOutAndDestroy(footPrintDuration));
        }

        public IEnumerator CoFadeOutAndDestroy(float duration) {
            for (float time = 0f; time < duration; time += Time.deltaTime) {
                footPrintColor = Palette.PlayerColors[(int) player.Data.ColorId];

                if (spriteRenderer)
                    spriteRenderer.color = new Color(footPrintColor.r, footPrintColor.g, footPrintColor.b, Mathf.Clamp(1f - time / duration, 0f, 1f));

                yield return null;
            }

            Object.Destroy(footPrint);
            allFootprint.Remove(this);
        }
    }
}
