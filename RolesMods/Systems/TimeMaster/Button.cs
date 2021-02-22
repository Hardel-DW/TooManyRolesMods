using HarmonyLib;
using RolesMods.Utility.Enumerations;
using UnityEngine;

namespace RolesMods.Systems.TimeMaster {

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class Button {
        public static void Postfix(HudManager __instance) {
            GlobalVariable.buttonTime = new CooldownButton
                (() => OnClick(),
                RolesMods.TimeMasterCooldown.GetValue(),
                "RolesMods.Resources.Rewind.png",
                500f,
                new Vector2(0f, 0f),
                Visibility.Everyone,
                __instance,
                RolesMods.TimeMasterDuration.GetValue(),
                () => OnEffectEnd(),
                () => OnUpdate(GlobalVariable.buttonTime)
            );
        }

        private static void OnEffectEnd() {
            Time.StopRewind();
        }

        private static void OnClick() {
            Time.StartRewind();
        }

        private static void OnUpdate(CooldownButton button) {
            if (GlobalVariable.TimeMaster != null && PlayerControl.LocalPlayer != null)
                if (PlayerControl.LocalPlayer.PlayerId == GlobalVariable.TimeMaster.PlayerId)
                    if (PlayerControl.LocalPlayer.Data.IsDead)
                        button.SetCanUse(false);
                    else
                        button.SetCanUse(true);

            if (Time.isRewinding)
                for (int i = 0; i < 2; i++)
                    Time.Rewind();
            else
                Time.Record();
        }
    }
}