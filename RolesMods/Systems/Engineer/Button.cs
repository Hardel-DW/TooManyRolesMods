using HardelAPI.Utility.Utils;
using HardelAPI.Cooldown;
using EngineerRoles = RolesMods.Roles.Engineer;

namespace RolesMods.Systems.Engineer {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {

        public override void OnCreateButton() {
            Roles = EngineerRoles.Instance;
            DecreamteUseNimber = UseNumberDecremantion.OnClick;
            UseNumber = 1;
            SetSprite("RolesMods.Resources.Rewind.png", 250);
        }

        public override void OnClick() => SaboatageUtils.FixSabotage();
    }
}