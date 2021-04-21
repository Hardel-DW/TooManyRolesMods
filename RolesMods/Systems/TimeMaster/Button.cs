using HardelAPI.Utility;
using HarmonyLib;
using UnityEngine;

namespace RolesMods.Systems.TimeMaster {

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class Button {
        public static CooldownButton buttonTime;
        public static int UseNumber = 1;

        public static void Postfix(HudManager __instance) {
            buttonTime = new CooldownButton
                (() => OnClick(),
                Roles.TimeMaster.TimeMasterCooldown.GetValue(),
                Plugin.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Rewind.png", 250f),
                250,
                new Vector2(0f, 0f),
                __instance,
                Roles.TimeMaster.TimeMasterDuration.GetValue() / 2,
                () => OnEffectEnd(),
                () => OnUpdate(buttonTime)
            );
        }

        private static void OnEffectEnd() {
            Time.StopRewind();
        }

        private static void OnClick() {
            if (UseNumber > 0) {
                Time.StartRewind();
                UseNumber--;
            }
        }

        private static void OnUpdate(CooldownButton button) {
            if (Roles.TimeMaster.Instance.AllPlayers != null && PlayerControl.LocalPlayer != null)
                if (Roles.TimeMaster.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId))
                    if (PlayerControl.LocalPlayer.Data.IsDead)
                        button.SetCanUse(false);
                    else button.SetCanUse(!MeetingHud.Instance);

            if (Time.isRewinding)
                for (int i = 0; i < 2; i++)
                Time.Rewind();
            else
                Time.Record();
        }
    }
}