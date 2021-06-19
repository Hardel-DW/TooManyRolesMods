using HardelAPI.Cooldown;
using TimeMasterRole = RolesMods.Roles.TimeMaster;

namespace RolesMods.Systems.TimeMaster {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {

        public override void OnCreateButton() {
            UseNumber = 1;
            Timer = TimeMasterRole.TimeMasterCooldown.GetValue();
            Roles = TimeMasterRole.Instance;
            EffectDuration = TimeMasterRole.TimeMasterDuration.GetValue() / 2;
            HasEffectDuration = true;
            DecreamteUseNimber = UseNumberDecremantion.OnEffectEnd;
            SetSprite("RolesMods.Resources.Rewind.png", 250);
        }

        public override void OnClick() => Time.StartRewind();

        public override void OnEffectEnd() => Time.StopRewind();

        public override void OnUpdate() {
            if (Time.isRewinding)
                for (int i = 0; i < 2; i++)
                Time.Rewind();
            else
                Time.Record();
        }
    }
}