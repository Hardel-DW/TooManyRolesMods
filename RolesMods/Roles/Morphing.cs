using HardelAPI.CustomOptions;
using HardelAPI.CustomRoles;
using HardelAPI.CustomRoles.Abilities;
using HardelAPI.CustomRoles.Abilities.UsableVent;
using HardelAPI.Enumerations;
using System.Collections.Generic;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Morphing))]
    public class Morphing : CustomRole<Morphing> {
        // Color: #FF930FFF
        public static CustomOptionHeader MorphingHeader = CustomOptionHeader.AddHeader("<color=#FF0000FF>Morphing Options :</color>");
        public static CustomNumberOption MorphingPercent = CustomOption.AddNumber("Morphing Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberMorphing = CustomOption.AddNumber("Number Morphing", 1f, 1f, 10f, 1f);
        public static CustomNumberOption MorphingCooldown = CustomOption.AddNumber("Morphing Cooldown", 30f, 10f, 120f, 5f);
        public static CustomNumberOption MorphingDuration = CustomOption.AddNumber("Morphing Duration", 10f, 2f, 30f, 2f);
        public static CustomNumberOption MaxUseMorphing = CustomOption.AddNumber("Max use", 1f, 1f, 10f, 1f);

        public override List<Ability> Abilities { get; set; } = new List<Ability>() {
            new VentAbility() { CanVent = true }
        };

        public Morphing() : base() {
            GameOptionFormat();
            Side = PlayerSide.Impostor;
            RoleActive = true;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            Color = Palette.ImpostorRed;
            Name = "Morphing";
            IntroDescription = "Je sais pas";
            TasksDescription = "<color=#FF930FFF>Morphing: Faut définir une description pffff</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) MorphingPercent.GetValue();
            NumberPlayers = (int) NumberMorphing.GetValue();
        }

        public override void OnGameStarted() {
            Systems.Morphing.Button.Instance.MaxTimer = MorphingCooldown.GetValue();
            Systems.Morphing.Button.Instance.EffectDuration = MorphingDuration.GetValue();
            Systems.Morphing.Button.Instance.UseNumber = (int) MaxUseMorphing.GetValue();
        }

        private void GameOptionFormat() {
            MorphingHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            MorphingPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberMorphing.ValueStringFormat = (option, value) => $"{value} players";
            MorphingCooldown.ValueStringFormat = (option, value) => $"{value}s";
            MorphingDuration.ValueStringFormat = (option, value) => $"{value}s";

            MaxUseMorphing.ValueStringFormat = (option, value) => $"{value} time";
        }
    }
}
