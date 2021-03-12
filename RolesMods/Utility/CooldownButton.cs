using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using RolesMods.Utility;

namespace RolesMods {
    public class CooldownButton {
        public static List<CooldownButton> buttons = new List<CooldownButton>();
        public KillButtonManager killButtonManager;
        private Color startColorButton = new Color(255, 255, 255);
        private Color startColorText = new Color(255, 255, 255);
        public Vector2 PositionOffset = Vector2.zero;
        public string embeddedName;
        public float MaxTimer = 0f;
        public float Timer = 0f;
        public float EffectDuration = 0f;
        public bool isEffectActive;
        public bool hasEffectDuration;
        public bool enabled = true;
        public int pixelPerUnit;
        private Sprite sprite;
        private Action OnClick;
        private Action OnEffectEnd;
        private Action OnUpdate;
        private HudManager hudManager;
        private bool canUse;

        public CooldownButton(Action OnClick, float Cooldown, string embeddedName, int pixelPerUnit, Vector2 PositionOffset, HudManager hudManager, float EffectDuration, Action OnEffectEnd, Action OnUpdate) {
            this.hudManager = hudManager;
            this.OnClick = OnClick;
            this.OnEffectEnd = OnEffectEnd;
            this.OnUpdate = OnUpdate;
            this.PositionOffset = PositionOffset;
            this.EffectDuration = EffectDuration;
            this.pixelPerUnit = pixelPerUnit;
            this.embeddedName = embeddedName;
            this.sprite = HelperSprite.LoadSpriteFromEmbeddedResources(embeddedName, pixelPerUnit);
            MaxTimer = Cooldown;
            Timer = MaxTimer;
            hasEffectDuration = true;
            isEffectActive = false;
            buttons.Add(this);
            Start();
        }

        public CooldownButton(Action OnClick, float Cooldown, string embeddedName, int pixelPerUnit, Vector2 PositionOffset, HudManager hudManager, Action OnUpdate) {
            this.hudManager = hudManager;
            this.OnClick = OnClick;
            this.OnUpdate = OnUpdate;
            this.PositionOffset = PositionOffset;
            this.embeddedName = embeddedName;
            this.pixelPerUnit = pixelPerUnit;
            this.sprite = HelperSprite.LoadSpriteFromEmbeddedResources(embeddedName, pixelPerUnit);
            MaxTimer = Cooldown;
            Timer = MaxTimer;
            hasEffectDuration = false;
            buttons.Add(this);
            Start();
        }

        private void Start() {
            killButtonManager = UnityEngine.Object.Instantiate(hudManager.KillButton, hudManager.transform);
            startColorButton = killButtonManager.renderer.color;
            startColorText = killButtonManager.TimerText.Color;
            killButtonManager.gameObject.SetActive(true);
            killButtonManager.renderer.enabled = true;
            killButtonManager.renderer.sprite = sprite;
            PassiveButton button = killButtonManager.GetComponent<PassiveButton>();
            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener((UnityEngine.Events.UnityAction) listener);

            void listener() {
                if (Timer < 0f && canUse) {
                    killButtonManager.renderer.color = new Color(1f, 1f, 1f, 0.3f);
                    if (hasEffectDuration) {
                        isEffectActive = true;
                        Timer = EffectDuration;
                        killButtonManager.TimerText.Color = new Color(0, 255, 0);
                    } else {
                        Timer = MaxTimer;
                    }

                    OnClick();
                }
            }
        }

        public static void HudUpdate() {
            buttons.RemoveAll(item => item.killButtonManager == null);
            for (int i = 0; i < buttons.Count; i++) {
                buttons[i].killButtonManager.renderer.sprite = buttons[i].sprite;
                buttons[i].OnUpdate();
                buttons[i].Update();
            }
        }

        private void Update() {
            if (killButtonManager.transform.localPosition.x > 0f)
                killButtonManager.transform.localPosition = new Vector3((killButtonManager.transform.localPosition.x + 1.3f) * -1, killButtonManager.transform.localPosition.y, killButtonManager.transform.localPosition.z) + new Vector3(PositionOffset.x, PositionOffset.y);
            if (Timer < 0f) {
                killButtonManager.renderer.color = new Color(1f, 1f, 1f, 1f);
                if (isEffectActive) {
                    killButtonManager.TimerText.Color = startColorText;
                    Timer = MaxTimer;
                    isEffectActive = false;
                    OnEffectEnd();
                }
            } else {
                if (canUse && (isEffectActive || PlayerControl.LocalPlayer.CanMove))
                    Timer -= Time.deltaTime;

                killButtonManager.renderer.color = new Color(1f, 1f, 1f, 0.3f);
            }
            killButtonManager.gameObject.SetActive(canUse);
            killButtonManager.renderer.enabled = canUse;
            if (canUse) {
                killButtonManager.renderer.material.SetFloat("_Desat", 0f);
                killButtonManager.SetCoolDown(Timer, MaxTimer);
            }
        }

        public void SetCanUse(bool value) {
            this.canUse = value;
        }

        public bool GetCanUse() {
            return this.canUse;
        }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudUpdatePatch {
        public static void Postfix() {
            CooldownButton.HudUpdate();
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    public static class MeetingUpdatePatch {
        public static void Postfix(MeetingHud __instance) {
            foreach (var button in CooldownButton.buttons) {
                button.SetCanUse(false);
            }
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class MeetingClosePatch {
        public static void Postfix() {
            foreach (var button in CooldownButton.buttons) {
                button.Timer = button.MaxTimer;
            }
        }
    }
}