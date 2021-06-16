using HardelAPI.Utility.Ability;
using HardelAPI.Utility.Utils;
using HardelAPI.Cooldown;
using SwooperRoles = RolesMods.Roles.Swooper;

namespace RolesMods.Systems.Swooper {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {

        public override void OnClick() {
            Invisbility.LaunchInvisibility(PlayerControl.LocalPlayer, SwooperRoles.SwooperDuration.GetValue(), PlayerControlUtils.GetImpostors());
        }

        public override void OnCreateButton() {
            Timer = SwooperRoles.SwooperCooldown.GetValue();
            EffectDuration = SwooperRoles.SwooperDuration.GetValue();
            UseNumber = 1;
            Roles = SwooperRoles.Instance;
            HasEffectDuration = true;
            DecreamteUseNimber = UseNumberDecremantion.OnClick;
            SetSprite("RolesMods.Resources.Rewind.png", 250);
        }
    }
}