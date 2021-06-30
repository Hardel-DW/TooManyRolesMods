using Harion.Utility.Utils;
using Harion.Cooldown;
using EngineerRoles = RolesMods.Roles.Engineer;

namespace RolesMods.Systems.Engineer {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {

        public override void OnCreateButton() {
            Roles = EngineerRoles.Instance;
            DecreamteUseNimber = UseNumberDecremantion.OnClick;
            UseNumber = 1;
            SetSprite(ResourceLoader.RewindRedSprite);
        }

        public override void OnClick() => SaboatageUtils.FixSabotages();

        public override void OnUpdate() {
            if (EngineerRoles.Instance.AllPlayers != null && PlayerControl.LocalPlayer != null)
                if (EngineerRoles.Instance.HasRole(PlayerControl.LocalPlayer))
                    IsDisable = !SaboatageUtils.SabotageActive();
        }
    }
}