using Harion.Utility.Utils;
using Harion.Cooldown;
using JanitorRoles = RolesMods.Roles.Janitor;

namespace RolesMods.Systems.Janitor {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {

        public override void OnCreateButton() {
            MaxTimer = PlayerControl.GameOptions.KillCooldown;
            Timer = PlayerControl.GameOptions.KillCooldown;
            DecreamteUseNimber = UseNumberDecremantion.OnClick;
            Closest = Harion.Cooldown.ClosestElement.DeadBody;
            Roles = JanitorRoles.Instance;
            SetSprite("RolesMods.Resources.Rewind.png", 250);
        }

        public override void OnClick() {
            DeadBody body = GetDeadBodyTarget();
            if (body != null) {
                PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                DeadBodyUtils.CleanBodyDuration(body, 60);
            }
        }
    }
}