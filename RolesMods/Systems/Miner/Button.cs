using Harion.Utility.Utils;
using Harion.Cooldown;
using MinerRoles = RolesMods.Roles.Miner;

namespace RolesMods.Systems.Miner {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {

        public override void OnCreateButton() {
            Timer = MinerRoles.MinerCooldown.GetValue();
            Roles = MinerRoles.Instance;
            UseNumber = 1;
            DecreamteUseNimber = UseNumberDecremantion.OnClick;
            SetSprite(ResourceLoader.RewindRedSprite);
        }

        public override void OnClick() => VentUtils.PlaceVent(PlayerControl.LocalPlayer.transform.position);
    }
}