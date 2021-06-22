using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.CustomRoles.Abilities;
using Harion.CustomRoles.Abilities.UsableVent;
using Harion.Enumerations;
using System.Collections.Generic;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Janitor))]
    public class Janitor : CustomRole<Janitor> {
        // Color: #FF930FFF
        public static CustomNumberOption JanitorPercent = CustomOption.AddNumber("Janitor", "<color=#FF0000FF>Janitor Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.ImpostorHolder);
        public static CustomNumberOption NumberJanitor = CustomOption.AddNumber("Number Janitor", 1f, 1f, 10f, 1f, JanitorPercent);
        public static CustomNumberOption JanitorCooldown = CustomOption.AddNumber("Janitor Cooldown", 30f, 10f, 120f, 5f, JanitorPercent);
        public static CustomNumberOption MaxUseJanitor = CustomOption.AddNumber("Max use", 1f, 1f, 10f, 1f, JanitorPercent);

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
            JanitorPercent.ValueStringFormat = (option, value) => $"{value}%";
            JanitorPercent.ShowChildrenConidtion = () => JanitorPercent.GetValue() > 0;

            NumberJanitor.ValueStringFormat = (option, value) => $"{value} players";
            JanitorCooldown.ValueStringFormat = (option, value) => $"{value}s";
            
            MaxUseJanitor.ValueStringFormat = (option, value) => $"{value} time";
        }
    }
}
