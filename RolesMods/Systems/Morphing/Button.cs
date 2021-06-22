using UnityEngine;
using Harion.Cooldown;
using MorphingRoles = RolesMods.Roles.Morphing;
using Harion.Utility.Helper;

namespace RolesMods.Systems.Morphing {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {
        private static Sprite SampleSprite;
        private static Sprite MorphSprite;
        public PlayerControl Sample = null;
        public MorphingState State = MorphingState.Nothing;

        public override void OnCreateButton() {
            Timer = MorphingRoles.MorphingCooldown.GetValue();
            EffectDuration = MorphingRoles.MorphingDuration.GetValue();
            Roles = MorphingRoles.Instance;
            HasEffectDuration = true;
            DecreamteUseNimber = UseNumberDecremantion.OnEffectEnd;
            SampleSprite = SpriteHelper.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Target.png", 250f);
            MorphSprite = SpriteHelper.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Rewind.png", 250f);
            SetNothingState();
        }

        public override void OnClick() {
            if (State == MorphingState.Nothing) {
                Sample = GetPlayerTarget();
                SetSampleState();
                ForceEnd(false);
                return;
            }
            else if (State == MorphingState.Sample) {
                if (Sample == null) {
                    SetNothingState();
                    return;
                }

                Harion.Utility.Ability.Morphing.Morph(PlayerControl.LocalPlayer, Sample, true);
                SetMorphingState();
            }
        }

        public override void OnEffectEnd() {
            Harion.Utility.Ability.Morphing.Unmorph(PlayerControl.LocalPlayer, true);
            SetNothingState();
        }

        public override void OnUpdate() {
            if (Sample == null && State == MorphingState.Sample) {
                SetNothingState();
            } else if (Sample != null && State == MorphingState.Nothing) {
                SetSampleState();
            }
        }

        private void SetNothingState() {
            State = MorphingState.Nothing;
            SetSprite(SampleSprite);
            Closest = Harion.Cooldown.ClosestElement.Player;
            Timer = MaxTimer;
        }

        private void SetSampleState() {
            State = MorphingState.Sample;
            SetSprite(MorphSprite);
            Closest = Harion.Cooldown.ClosestElement.Empty;
        }

        private void SetMorphingState() {
            State = MorphingState.Morphed;
            Closest = Harion.Cooldown.ClosestElement.Empty;
        }
    }

    public enum MorphingState {
        Nothing,
        Sample,
        Morphed
    }
}
