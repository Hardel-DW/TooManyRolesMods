using Essentials.Options;
using HardelAPI.CustomRoles;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Altruist))]
    public class Altruist : CustomRole<Altruist> {
        // Color: D10052FF
        public static CustomOptionHeader AltruistHeader = CustomOptionHeader.AddHeader("<color=#D10052FF>Altruist Options :</color>");
        public static CustomNumberOption AltruistPercent = CustomOption.AddNumber("Altruist Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberAltruist = CustomOption.AddNumber("Number Altruist", 1f, 1f, 10f, 1f);
        public static CustomNumberOption AltruistCooldown = CustomOption.AddNumber("Seer Cooldown", 30f, 10f, 120f, 5f);
        public static CustomNumberOption AltruistUseNumber = CustomOption.AddNumber("Number of uses", 1f, 1f, 10f, 1f);

        public Altruist() : base() {
            GameOptionFormat();
            Color = new Color(0.819f, 0f, 0.321f, 1f);
            Name = "Altruist";
            IntroDescription = "Get voted out";
            TasksDescription = "<color=#2EADFFFF>Jester: You are an Jester, Get voted out</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) AltruistPercent.GetValue();
            NumberPlayers = (int) NumberAltruist.GetValue();
        }

        private void GameOptionFormat() {
            AltruistHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            AltruistPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberAltruist.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
