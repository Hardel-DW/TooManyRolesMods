﻿using HardelAPI.Utility.Utils;
using HardelAPI.Cooldown;
using MinerRoles = RolesMods.Roles.Miner;

namespace RolesMods.Systems.Miner {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {

        public override void OnCreateButton() {
            Timer = MinerRoles.MinerCooldown.GetValue();
            Roles = MinerRoles.Instance;
            SetSprite("RolesMods.Resources.Rewind.png", 250);
        }

        public override void OnClick() => VentUtils.PlaceVent(PlayerControl.LocalPlayer.transform.position);
    }
}