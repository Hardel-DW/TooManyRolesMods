﻿using HardelAPI.ArrowManagement;
using HardelAPI.Cooldown;
using HardelAPI.Utility;
using System.Collections.Generic;
using TrackerRoles = RolesMods.Roles.Tracker;

namespace RolesMods.Systems.Tracker {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {
        public static ArrowManager Arrow;

        public override void OnClick() {
            UseNumber = 0;
            PlayerButton.InitPlayerButton(
                false, 
                new List<PlayerControl> { PlayerControl.LocalPlayer },
                (Player) => OnCPlayerChoose(Player),
                () => UseNumber = 1
            );
        }

        public override void OnCreateButton() {
            Timer = 10f;
            Roles = TrackerRoles.Instance;
            UseNumber = 1;
            SetSprite("RolesMods.Resources.Target.png", 250);
        }

        private void OnCPlayerChoose(PlayerControl Player) {
            Arrow = new ArrowManager(Player.gameObject, Player.transform.position, true, TrackerRoles.TargetUpdate.GetValue());
        }
    }
}