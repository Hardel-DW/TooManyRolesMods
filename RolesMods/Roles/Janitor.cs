using HardelAPI.CustomOptions;
using HardelAPI.CustomRoles;
using HardelAPI.CustomRoles.Abilities;
using HardelAPI.CustomRoles.Abilities.UsableVent;
using HardelAPI.Enumerations;
using System.Collections.Generic;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Janitor))]
    public class Janitor : CustomRole<Janitor> {
        // Color: #FF930FFF
        public static CustomOptionHeader JanitorHeader = CustomOptionHeader.AddHeader("<color=#FF0000FF>Janitor Options :</color>");
        public static CustomNumberOption JanitorPercent = CustomOption.AddNumber("Janitor Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberJanitor = CustomOption.AddNumber("Number Janitor", 1f, 1f, 10f, 1f);
        public static CustomNumberOption JanitorCooldown = CustomOption.AddNumber("Janitor Cooldown", 30f, 10f, 120f, 5f);
        public static CustomNumberOption MaxUseJanitor = CustomOption.AddNumber("Max use", 1f, 1f, 10f, 1f);

        public override List<Ability> Abilities { get; set; } = new List<Ability>() {
            new VentAbility() { CanVent = true }
        };

        public Janitor() : base() {
            GameOptionFormat();
            Side = PlayerSide.Impostor;
            RoleActive = true;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            Color = Palette.ImpostorRed;
            Name = "Janitor";
            IntroDescription = "Je sais pas";
            TasksDescription = "<color=#FF930FFF>Janitor: Faut définir une description pffff</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) JanitorPercent.GetValue();
            NumberPlayers = (int) NumberJanitor.GetValue();
        }

        public override void OnGameStarted() {
            Systems.Janitor.Button.Instance.MaxTimer = PlayerControl.GameOptions.KillCooldown;
            Systems.Janitor.Button.Instance.UseNumber = (int) MaxUseJanitor.GetValue();
        }

        public override void OnLocalAttempKill(PlayerControl killer, PlayerControl target) {
            base.OnLocalAttempKill(killer, target);
            Systems.Janitor.Button.Instance.Timer = JanitorCooldown.GetValue();
        }

        private void GameOptionFormat() {
            JanitorHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            JanitorPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberJanitor.ValueStringFormat = (option, value) => $"{value} players";
            JanitorCooldown.ValueStringFormat = (option, value) => $"{value}s";
            
            MaxUseJanitor.ValueStringFormat = (option, value) => $"{value} time";
        }
    }
}
