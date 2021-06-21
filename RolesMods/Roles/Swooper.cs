using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.CustomRoles.Abilities;
using Harion.CustomRoles.Abilities.UsableVent;
using Harion.Enumerations;
using System.Collections.Generic;

namespace RolesMods.Roles {
    [RegisterInCustomRoles(typeof(Swooper))]
    public class Swooper : CustomRole<Swooper> {
        // Color: #FF930FFF
        public static CustomNumberOption SwooperPercent = CustomOption.AddNumber("<color=#FF0000FF>Swooper Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.ImpostorHolder);
        public static CustomNumberOption NumberSwooper = CustomOption.AddNumber("Number Swooper", 1f, 1f, 10f, 1f, SwooperPercent);
        public static CustomNumberOption SwooperCooldown = CustomOption.AddNumber("Swooper Cooldown", 30f, 10f, 120f, 5f, SwooperPercent);
        public static CustomNumberOption SwooperDuration = CustomOption.AddNumber("Swooper Duration", 10f, 2f, 30f, 2f, SwooperPercent);
        public static CustomNumberOption MaxUseSwooper = CustomOption.AddNumber("Max use", 1f, 1f, 10f, 1f, SwooperPercent);

        public override List<Ability> Abilities { get; set; } = new List<Ability>() {
            new VentAbility() { CanVent = true }
        };

        public Swooper() : base() {
            GameOptionFormat();
            Side = PlayerSide.Impostor;
            RoleActive = true;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            Color = Palette.ImpostorRed;
            Name = "Swooper";
            IntroDescription = "You can be Invisible";
            TasksDescription = "<color=#FF930FFF>Swooper: Use your ability to be invisible</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) SwooperPercent.GetValue();
            NumberPlayers = (int) NumberSwooper.GetValue();
        }

        public override void OnGameStarted() {
            Systems.Swooper.Button.Instance.MaxTimer = SwooperCooldown.GetValue();
            Systems.Swooper.Button.Instance.EffectDuration = SwooperDuration.GetValue();
            Systems.Swooper.Button.Instance.UseNumber = (int) MaxUseSwooper.GetValue();
        }

        private void GameOptionFormat() {
            SwooperPercent.ValueStringFormat = (option, value) => $"{value}%";
            SwooperPercent.ShowChildrenConidtion = () => SwooperPercent.GetValue() > 0;

            NumberSwooper.ValueStringFormat = (option, value) => $"{value} players";
            SwooperCooldown.ValueStringFormat = (option, value) => $"{value}s";
            SwooperDuration.ValueStringFormat = (option, value) => $"{value}s";

            MaxUseSwooper.ValueStringFormat = (option, value) => $"{value} time";
        }
    }
}
