using HardelAPI.Utility;
using HarmonyLib;
using UnityEngine;

namespace RolesMods.Systems.Spy {

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class Button {
        public static CooldownButton button;
        public static int UseNumber = 1;
        public static bool isActive;

        public static void Postfix(HudManager __instance) {
            button = new CooldownButton
                (() => OnClick(),
                Roles.Spy.SpyCooldown.GetValue(),
                Plugin.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Rewind.png", 250f),
                250,
                new Vector2(0f, 0f),
                __instance,
                Roles.Spy.SpyDuration.GetValue() / 2,
                () => OnEffectEnd(),
                () => OnUpdate()
            );
        }

        private static void OnEffectEnd() {
            UseNumber--;
            isActive = false;
        }

        private static void OnClick() {
            if (UseNumber > 0)
                isActive = true;
        }

        private static void OnUpdate() {
            if (Roles.Spy.Instance.AllPlayers != null && PlayerControl.LocalPlayer != null)
                if (Roles.Spy.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId))
                    if (PlayerControl.LocalPlayer.Data.IsDead || UseNumber <= 0)
                        button.SetCanUse(false);
                    else
                        button.SetCanUse(!MeetingHud.Instance);
        }
    }
}
