using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace RolesMods.Utility {
    public class CooldownButton {
        public static bool UsableButton = true;
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
        public float pixelPerUnit;
        private Sprite sprite;
        private Action OnClick;
        public Action OnEffectEnd;
        private Action OnUpdate;
        private HudManager hudManager;
        private bool canUse;

        public CooldownButton(Action OnClick, float Cooldown, string embeddedName, float pixelPerUnit, Vector2 PositionOffset, HudManager hudManager, float EffectDuration, Action OnEffectEnd, Action OnUpdate) {
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

        public CooldownButton(Action OnClick, float Cooldown, string embeddedName, float pixelPerUnit, Vector2 PositionOffset, HudManager hudManager, Action OnUpdate) {
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
            startColorText = killButtonManager.TimerText.color;
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
                        killButtonManager.TimerText.color = new Color(0, 255, 0);
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
            UpdatePosition();
            if (Timer < 0f) {
                killButtonManager.renderer.color = new Color(1f, 1f, 1f, 1f);
                if (isEffectActive) {
                    killButtonManager.TimerText.color = startColorText;
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

        public void UpdatePosition() {
            if (killButtonManager.transform.localPosition.x > 0f)
                killButtonManager.transform.localPosition = new Vector3((killButtonManager.transform.localPosition.x + 1.3f) * -1, killButtonManager.transform.localPosition.y, killButtonManager.transform.localPosition.z) + new Vector3(PositionOffset.x, PositionOffset.y);
        }

        public void ForceClick(bool DoAction) {
            killButtonManager.renderer.color = new Color(1f, 1f, 1f, 0.3f);
            if (hasEffectDuration) {
                isEffectActive = true;
                Timer = EffectDuration;
                killButtonManager.TimerText.color = new Color(0, 255, 0);
            } else {
                Timer = MaxTimer;
            }

            if (DoAction)
                OnClick();
        }

        public void ForceEnd(bool DoAction) {
            Timer = 0f;
            isEffectActive = false;
            killButtonManager.TimerText.color = startColorText;
            if (DoAction)
                OnEffectEnd();
        }

        public void Destroy() {
            UnityEngine.Object.Destroy(killButtonManager.gameObject);
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

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class MeetingClosePatch {
        public static void Postfix() {
            CooldownButton.UsableButton = true;
            foreach (var button in CooldownButton.buttons) {
                button.Timer = button.MaxTimer;
            }
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public static class ButtonResetPatch {
        public static void Postfix(MeetingHud __instance) {
            CooldownButton.UsableButton = false;
            for (int i = 0; i < CooldownButton.buttons.Count; i++) {
                if (CooldownButton.buttons[i].hasEffectDuration) {
                    CooldownButton.buttons[i].OnEffectEnd();
                    CooldownButton.buttons[i].isEffectActive = false;
                }
            }
        }
    }

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class StartPatch {
        public static void Prefix(ShipStatus __instance) {
            CooldownButton.UsableButton = true;
        }
    }
}