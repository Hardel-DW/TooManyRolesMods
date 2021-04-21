using HardelAPI.ArrowManagement;
using HardelAPI.Utility;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Systems.Tracker {

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class Button {
        public static CooldownButton buttonTime;
        public static bool usable = true;
        public static ArrowManager Arrow;

        public static void Postfix(HudManager __instance) {
            buttonTime = new CooldownButton
                (() => OnClick(),
                10f,
                Plugin.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Target.png", 250f),
                250,
                new Vector2(0f, 0f),
                __instance,
                () => OnUpdate(buttonTime)
            );
        }

        private static void OnClick() {
            usable = false;
            PlayerButton.InitPlayerButton(
                false, 
                new List<PlayerControl> { PlayerControl.LocalPlayer },
                (Player) => Arrow = new ArrowManager(Player.gameObject, Player.transform.position, true, Roles.Tracker.TargetUpdate.GetValue()), 
                () => usable = true
            );
        }

        private static void OnUpdate(CooldownButton button) {
            if (Roles.Tracker.Instance.AllPlayers != null && PlayerControl.LocalPlayer != null) {
                if (Roles.Tracker.Instance.HasRole(PlayerControl.LocalPlayer) && usable) {
                    if (!PlayerControl.LocalPlayer.Data.IsDead) {
                        button.SetCanUse(!MeetingHud.Instance);
                        return;
                    }
                }
            }

            button.SetCanUse(false);
        }
    }
}