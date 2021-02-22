using RolesMods.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Systems.Investigator {
    class FootPrint {
        public static List<FootPrint> allFootprint = new List<FootPrint>();

        private readonly float footPrintSize;
        private Color footPrintColor;
        private Vector3 footPrintPosition;
        private readonly float footPrintDuration;
        private readonly int footPrintUnixTime;
        private GameObject footPrint;
        private SpriteRenderer spriteRenderer;
        private readonly PlayerControl player;

        public FootPrint(float footPrintSize, float footPrintDuration, PlayerControl player) {
            this.footPrintSize = footPrintSize;
            this.footPrintColor = Palette.PlayerColors[(int) player.Data.ColorId];
            this.footPrintPosition = player.transform.position;
            this.footPrintDuration = footPrintDuration;
            this.player = player;
            this.footPrintUnixTime = (int) DateTimeOffset.Now.ToUnixTimeSeconds();

            if (RolesMods.AnonymousFootPrint.GetValue())
                this.footPrintColor = new Color(0.2f, 0.2f, 0.2f, 1f);

            Start();
        }

        private void Start() {
            footPrint = new GameObject("FootPrint");
            footPrint.transform.position = footPrintPosition;
            footPrint.transform.localPosition = footPrintPosition;
            footPrint.transform.SetParent(player.transform.parent);
            spriteRenderer = footPrint.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = HelperSprite.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Circle.png", 400f + (float) (footPrintSize * 128 + Math.Pow(Math.Pow(footPrintSize, 1.75d), 2d)));
            spriteRenderer.color = footPrintColor;

            footPrint.SetActive(true);
            allFootprint.Add(this);
        }

        private void DestroyThis() {
            UnityEngine.Object.Destroy(footPrint);
            allFootprint.Remove(this);
        }

        public void Update() {
            int currentUnixTime = (int) DateTimeOffset.Now.ToUnixTimeSeconds();
            float alpha = Mathf.Max((1f - ((currentUnixTime - footPrintUnixTime) / footPrintDuration)), 0f);
            
            if (alpha < 0 || alpha > 1)
                alpha = 0;

            footPrintColor = new Color(footPrintColor.r, footPrintColor.g, footPrintColor.b, alpha);
            spriteRenderer.color = footPrintColor;
            
            if (footPrintUnixTime + (int) footPrintDuration < currentUnixTime)
                DestroyThis();
        }

        public Vector3 FootPrintPosition {
            get => footPrintPosition;
        }
    }
}
