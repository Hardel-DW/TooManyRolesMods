using UnityEngine;
using Harion.Cooldown;
using MorphingRoles = RolesMods.Roles.Morphing;
using Harion.Utility.Helper;

namespace RolesMods.Systems.Morphing {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {
        public static PlayerControl Sample;
        public static MorphingState state = MorphingState.Sample;
        private static Sprite SampleSprite;
        private static Sprite MorphSprite;

        public override void OnCreateButton() {
            Timer = MorphingRoles.MorphingCooldown.GetValue();
            EffectDuration = MorphingRoles.MorphingDuration.GetValue();
            Roles = MorphingRoles.Instance;
            HasEffectDuration = true;
            DecreamteUseNimber = UseNumberDecremantion.OnEffectEnd;
            SampleSprite = SpriteHelper.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Rewind.png", 250f);
            MorphSprite = SpriteHelper.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Rewind.png", 250f);
            SetSprite("RolesMods.Resources.Rewind.png", 250);
        }

        public override void OnClick() {
            if (Sample == null) {
                Sample = GetPlayerTarget();
                ForceEnd(false);
            }

            else Harion.Utility.Ability.Morphing.Morph(PlayerControl.LocalPlayer, Sample, true);
        }

        public override void OnEffectEnd() {
            Harion.Utility.Ability.Morphing.Unmorph(PlayerControl.LocalPlayer, true);
        }

        public override void OnUpdate() {
            if (Sample == null && state == MorphingState.Morphed) {
                state = MorphingState.Sample;
                SetSprite(SampleSprite);
                Closest = Harion.Cooldown.ClosestElement.Player;
            } else if (Sample != null && state == MorphingState.Sample) {
                state = MorphingState.Morphed;
                SetSprite(MorphSprite);
                Closest = Harion.Cooldown.ClosestElement.Empty;
            }
        }
    }

    public enum MorphingState {
        Sample,
        Morphed
    }
}
