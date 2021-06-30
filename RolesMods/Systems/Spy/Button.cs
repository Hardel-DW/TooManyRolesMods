using Harion.Cooldown;
using SpyRoles = RolesMods.Roles.Spy;

namespace RolesMods.Systems.Spy {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {

        public override void OnCreateButton() {
            Timer = SpyRoles.SpyCooldown.GetValue();
            EffectDuration = SpyRoles.SpyDuration.GetValue();
            UseNumber = 1;
            HasEffectDuration = true;
            Roles = SpyRoles.Instance;
            DecreamteUseNimber = UseNumberDecremantion.OnEffectEnd;
            SetSprite(ResourceLoader.RewindRedSprite);
        }
    }
}
