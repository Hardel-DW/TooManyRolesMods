﻿using HarmonyLib;
using RolesMods.Utility.Enumerations;
using UnityEngine;

namespace RolesMods.Systems.TimeMaster {

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class Button {
        public static CooldownButton buttonTime;

        public static void Postfix(HudManager __instance) {
            buttonTime = new CooldownButton
                (() => OnClick(),
                Roles.TimeMaster.TimeMasterCooldown.GetValue(),
                "RolesMods.Resources.Rewind.png",
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
            Time.StartRewind();
        }

        private static void OnUpdate(CooldownButton button) {
            if (Roles.TimeMaster.Instance.AllPlayers != null && PlayerControl.LocalPlayer != null)
                if (Roles.TimeMaster.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId))
                    if (PlayerControl.LocalPlayer.Data.IsDead)
                        button.SetCanUse(false);
                    else button.SetCanUse(true);

            if (Time.isRewinding)
                for (int i = 0; i < 2; i++)
                Time.Rewind();
            else
                Time.Record();
        }
    }
}