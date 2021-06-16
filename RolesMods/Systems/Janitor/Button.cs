using HardelAPI.Utility.Utils;
using HardelAPI.Cooldown;
using JanitorRoles = RolesMods.Roles.Janitor;

namespace RolesMods.Systems.Janitor {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {

        public override void OnCreateButton() {
            Timer = PlayerControl.GameOptions.KillCooldown;
            DecreamteUseNimber = UseNumberDecremantion.OnClick;
            Closest = HardelAPI.Cooldown.ClosestElement.DeadBody;
            Roles = JanitorRoles.Instance;
            SetSprite("RolesMods.Resources.Rewind.png", 250);
        }

        public override void OnClick() {
            DestroyableSingleton<HudManager>.Instance.KillButton.SetCoolDown(JanitorRoles.JanitorCooldown.GetValue(), PlayerControl.GameOptions.KillCooldown);
            DeadBodyUtils.CleanBodyDuration(GetDeadBodyTarget(), 60);
        }
    }
}