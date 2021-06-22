using Harion.CustomOptions;
using Harion.CustomRoles;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Altruist))]
    public class Altruist : CustomRole<Altruist> {
        // Color: D10052FF
        public static CustomNumberOption AltruistPercent = CustomOption.AddNumber("Altruist", "<color=#D10052FF>Altruist Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.CrewmateHolder);
        public static CustomNumberOption NumberAltruist = CustomOption.AddNumber("Number Altruist", 1f, 1f, 10f, 1f, AltruistPercent);

        public Altruist() : base() {
            GameOptionFormat();
            Color = new Color(0.819f, 0f, 0.321f, 1f);
            Name = "Altruist";
            IntroDescription = "Get voted out";
            TasksDescription = "<color=#D10052FF>Altruist: Give your life to another player</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) AltruistPercent.GetValue();
            NumberPlayers = (int) NumberAltruist.GetValue();
            Systems.Altruist.Button.Instance.UseNumber = 1;
        }

        private void GameOptionFormat() {
            AltruistPercent.ValueStringFormat = (option, value) => $"{value}%";
            AltruistPercent.ShowChildrenConidtion = () => AltruistPercent.GetValue() > 0;

            NumberAltruist.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
