using HardelAPI.CustomOptions;
using HardelAPI.CustomRoles;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Shifter))]
    public class Shifter : CustomRole<Shifter> {
        // Color: #575757FF
        public static CustomOptionHeader ShifterHeader = CustomOptionHeader.AddHeader("<color=#575757FF>Shifter Options :</color>");
        public static CustomNumberOption ShifterPercent = CustomOption.AddNumber("Shifter Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberShifter = CustomOption.AddNumber("Number Shifter", 1f, 1f, 10f, 1f);
        public static CustomNumberOption Cooldown = CustomOption.AddNumber("Shift Cooldown", 30f, 10f, 120f, 5f);

        public Shifter() : base() {
            GameOptionFormat();
            TasksDescription = "<color=#575757FF>Shifter: Shift your role before the game ends</color>";
            IntroDescription = "Shift your role before the game ends";
            Name = "Shifter";
            HasTask = false;
            Color = new Color(0.341f, 0.341f, 0.341f, 1f);
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) ShifterPercent.GetValue();
            NumberPlayers = (int) NumberShifter.GetValue();
        }

        private void GameOptionFormat() {
            ShifterHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            ShifterPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberShifter.ValueStringFormat = (option, value) => $"{value} players";
            Cooldown.ValueStringFormat = (option, value) => $"{value}s";
        }
    }
}
