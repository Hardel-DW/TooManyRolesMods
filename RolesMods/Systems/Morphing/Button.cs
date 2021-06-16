using UnityEngine;
using HardelAPI.Cooldown;
using MorphingRoles = RolesMods.Roles.Morphing;
using HardelAPI.Utility.Helper;

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

            else HardelAPI.Utility.Ability.Morphing.Morph(PlayerControl.LocalPlayer, Sample, true);
        }

        public override void OnEffectEnd() {
            HardelAPI.Utility.Ability.Morphing.Unmorph(PlayerControl.LocalPlayer, true);
        }

        public override void OnUpdate() {
            if (Sample == null && state == MorphingState.Morphed) {
                state = MorphingState.Sample;
                SetSprite(SampleSprite);
                Closest = HardelAPI.Cooldown.ClosestElement.Player;
            } else if (Sample != null && state == MorphingState.Sample) {
                state = MorphingState.Morphed;
                SetSprite(MorphSprite);
                Closest = HardelAPI.Cooldown.ClosestElement.Empty;
            }
        }
    }

    public enum MorphingState {
        Sample,
        Morphed
    }
}
