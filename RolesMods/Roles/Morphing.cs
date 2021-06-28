using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.CustomRoles.Abilities;
using Harion.CustomRoles.Abilities.UsableVent;
using Harion.Enumerations;
using System.Collections.Generic;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Morphing))]
    public class Morphing : CustomRole<Morphing> {
        // Color: #FF930FFF
        public static CustomNumberOption MorphingPercent = CustomOption.AddNumber("Morphing", "<color=#FF0000FF>Morphing Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.ImpostorHolder);
        public static CustomNumberOption NumberMorphing = CustomOption.AddNumber("Number Morphing", 1f, 1f, 10f, 1f, MorphingPercent);
        public static CustomNumberOption MorphingCooldown = CustomOption.AddNumber("Morphing Cooldown", 30f, 10f, 120f, 5f, MorphingPercent);
        public static CustomNumberOption MorphingDuration = CustomOption.AddNumber("Morphing Duration", 10f, 2f, 30f, 2f, MorphingPercent);
        public static CustomNumberOption MaxUseMorphing = CustomOption.AddNumber("Max use", 1f, 1f, 10f, 1f, MorphingPercent);

        public override List<Ability> Abilities { get; set; } = new List<Ability>() {
            new VentAbility() { CanVent = true }
        };

        public Morphing() : base() {
            GameOptionFormat();
            RoleActive = true;
            Side = PlayerSide.Impostor;
            RoleType = RoleType.Impostor;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            Color = Palette.ImpostorRed;
            Name = "Morphing";
            IntroDescription = "Je sais pas";
            TasksDescription = "<color=#FF0000FF>Morphing: Faut définir une description pffff</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) MorphingPercent.GetValue();
            NumberPlayers = (int) NumberMorphing.GetValue();
        }

        public override void OnMeetingEnd(MeetingHud instance) {
            Systems.Morphing.Button.Instance.Sample = null;
            Systems.Morphing.Button.Instance.State = Systems.Morphing.MorphingState.Nothing;
        }

        public override void OnGameStarted() {
            Systems.Morphing.Button.Instance.MaxTimer = MorphingCooldown.GetValue();
            Systems.Morphing.Button.Instance.EffectDuration = MorphingDuration.GetValue();
            Systems.Morphing.Button.Instance.UseNumber = (int) MaxUseMorphing.GetValue();
        }

        private void GameOptionFormat() {
            MorphingPercent.ValueStringFormat = (option, value) => $"{value}%";
            MorphingPercent.ShowChildrenConidtion = () => MorphingPercent.GetValue() > 0;

            NumberMorphing.ValueStringFormat = (option, value) => $"{value} players";
            MorphingCooldown.ValueStringFormat = (option, value) => $"{value}s";
            MorphingDuration.ValueStringFormat = (option, value) => $"{value}s";

            MaxUseMorphing.ValueStringFormat = (option, value) => $"{value} time";
        }
    }
}
