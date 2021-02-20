using RolesMods.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Systems.Investigator {
    class FootPrint {
        public static List<FootPrint> allFootprint = new List<FootPrint>();

        private float footPrintSize;
        private Color footPrintColor;
        private Vector3 footPrintPosition;
        private float footPrintDuration;
        private int footPrintUnixTime;
        private GameObject footPrint;
        private PlayerControl player;
        private SpriteRenderer spriteRenderer;

        public FootPrint(float footPrintSize, float footPrintDuration, PlayerControl player) {
            this.footPrintSize = footPrintSize;
            this.footPrintColor = Palette.PlayerColors[(int) player.Data.ColorId];
            this.footPrintPosition = player.transform.position;
            this.footPrintDuration = footPrintDuration;
            this.player = player;
            this.footPrintUnixTime = (int) DateTimeOffset.Now.ToUnixTimeSeconds();
            Start();
        }

        private void Start() {
            Color playerColor = Palette.PlayerColors[(int) player.Data.ColorId];

            footPrint = new GameObject("FootPrint");
            footPrint.transform.position = footPrintPosition;
            footPrint.transform.localPosition = footPrintPosition;
            footPrint.transform.SetParent(player.transform.parent);
            spriteRenderer = footPrint.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = HelperSprite.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Circle.png", footPrintSize);
            spriteRenderer.color = new Color(playerColor.r, playerColor.g, playerColor.b, playerColor.a);

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
            
            if (alpha < 0 || alpha > 1) {
                alpha = 0;
            }

            footPrintColor = new Color(footPrintColor.r, footPrintColor.g, footPrintColor.b, alpha);
            spriteRenderer.color = footPrintColor;
            
            if (footPrintUnixTime + (int) footPrintDuration < currentUnixTime)
                DestroyThis();
        }

        public float FootPrintSize {
            get => footPrintSize;
            set => footPrintSize = value;
        }

        public Color FootPrintColor {
            get => footPrintColor;
            set => footPrintColor = value;
        }

        public Vector3 FootPrintPosition {
            get => footPrintPosition;
            set => footPrintPosition = value;
        }

        public float FootPrintDuration {
            get => footPrintDuration;
            set => footPrintDuration = value;
        }
    }
}
