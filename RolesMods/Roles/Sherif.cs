using Essentials.Options;
using RolesMods.Utility.CustomRoles;
using RolesMods.Utility.Enumerations;
using System;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Sherif))]
    public class Sherif : CustomRole<Sherif> {
        // Color: D1B300FF
        public static CustomOptionHeader SherifHeader = CustomOptionHeader.AddHeader("[D1B300FF]Sherif Options :[]");
        public static CustomNumberOption SherifPercent = CustomOption.AddNumber("Sherif Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberSherif = CustomOption.AddNumber("Number Sherif", 1f, 1f, 10f, 1f);
        public static CustomToggleOption SelfDead = CustomOption.AddToggle("Sherif can die", false);
            
        public Sherif() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            RoleActive = true;
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.819f, 0.701f, 0f, 1f);
            Name = "Sherif";
            IntroDescription = "You can kill the impostors";
            TasksDescription = "[D1B300FF]Sherif : You can kill the impostors[]";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) SherifPercent.GetValue();
            NumberPlayers = (int) NumberSherif.GetValue();
        }

        public override void OnGameStart() {
            foreach (var Player in AllPlayers) {
                sheriff.LastKilled = DateTime.UtcNow;
                sheriff.LastKilled = sheriff.LastKilled.AddSeconds(-8.0);
            }
        }

        private void GameOptionFormat() {
            SherifHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            SherifPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberSherif.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
