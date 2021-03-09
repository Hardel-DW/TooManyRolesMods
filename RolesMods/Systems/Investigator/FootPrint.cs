﻿using RolesMods.Utility;
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
        private Vector2 velocity;
        private SpriteRenderer spriteRenderer;
        private readonly PlayerControl player;

        public FootPrint(float footPrintSize, float footPrintDuration, PlayerControl player) {
            this.footPrintSize = footPrintSize;
            this.footPrintColor = Palette.PlayerColors[(int) player.Data.ColorId];
            this.footPrintPosition = player.transform.position;
            this.footPrintDuration = footPrintDuration;
            this.velocity = player.gameObject.GetComponent<Rigidbody2D>().velocity;
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
            footPrint.transform.localScale = new Vector2(footPrintSize, footPrintSize);
            footPrint.transform.SetParent(player.transform.parent);
            footPrint.transform.Rotate(Vector3.forward * Vector2.SignedAngle(Vector2.up, velocity));
            spriteRenderer = footPrint.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = HelperSprite.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Footprint.png", 100f);
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
            this.footPrintColor = Palette.PlayerColors[(int) player.Data.ColorId];

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

        public PlayerControl Player => player;
    }
}
