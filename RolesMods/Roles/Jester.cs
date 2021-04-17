using Essentials.Options;
using HardelAPI.Utility.CustomRoles;
using HardelAPI.Utility.Enumerations;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Jester))]
    public class Jester : CustomRole<Jester> {
        // Color: D10052FF
        public static CustomOptionHeader JesterHeader = CustomOptionHeader.AddHeader("<color=#D10052FF>Jester Options :</color>");
        public static CustomNumberOption JesterPercent = CustomOption.AddNumber("Jester Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberJester = CustomOption.AddNumber("Number Jester", 1f, 1f, 10f, 1f);

        public Jester() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            RoleActive = true;
            HasTask = false;
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.819f, 0f, 0.321f, 1f);
            Name = "Jester";
            IntroDescription = "Get voted out";
            TasksDescription = "<color=#2EADFFFF>Jester: You are an Jester, Get voted out</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) JesterPercent.GetValue();
            NumberPlayers = (int) NumberJester.GetValue();

            Systems.Jester.ExiledPatch.JesterForceEndGame = false;
            Systems.Jester.ExiledPatch.tasksRemoved = false;
        }

        private void GameOptionFormat() {
            JesterHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            JesterPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberJester.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
