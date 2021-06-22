using Harion.CustomOptions;
using Harion.CustomRoles;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Shifter))]
    public class Shifter : CustomRole<Shifter> {
        // Color: #575757FF
        public static CustomNumberOption ShifterPercent = CustomOption.AddNumber("Shifter", "<color=#575757FF>Shifter Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.NeutralHolder);
        public static CustomNumberOption NumberShifter = CustomOption.AddNumber("Number Shifter", 1f, 1f, 10f, 1f, ShifterPercent);
        public static CustomNumberOption Cooldown = CustomOption.AddNumber("Shift Cooldown", 30f, 10f, 120f, 5f, ShifterPercent);

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
            ShifterPercent.ValueStringFormat = (option, value) => $"{value}%";
            ShifterPercent.ShowChildrenConidtion = () => ShifterPercent.GetValue() > 0;

            NumberShifter.ValueStringFormat = (option, value) => $"{value} players";
            Cooldown.ValueStringFormat = (option, value) => $"{value}s";
        }
    }
}
