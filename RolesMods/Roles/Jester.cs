using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.Enumerations;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Jester))]
    public class Jester : CustomRole<Jester> {
        // Color: D10052FF
        public static CustomNumberOption JesterPercent = CustomOption.AddNumber("Jester", "<color=#D10052FF>Jester Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.NeutralHolder);
        public static CustomNumberOption NumberJester = CustomOption.AddNumber("Number Jester", 1f, 1f, 10f, 1f, JesterPercent);

        public Jester() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            HasTask = false;
            RoleActive = true;
            ForceExiledReveal = true;
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.819f, 0f, 0.321f, 1f);
            Name = "Jester";
            IntroDescription = "Get voted out";
            TasksDescription = "<color=#D10052FF>Jester: You are an Jester, Get voted out</color>";
            OutroDescription = "Jester Win";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) JesterPercent.GetValue();
            NumberPlayers = (int) NumberJester.GetValue();
        }

        public override void OnExiledPlayer(PlayerControl PlayerExiled) {
            if (HasRole(PlayerExiled))
                ForceEndGame(new List<PlayerControl>() { PlayerExiled });
        }

        private void GameOptionFormat() {
            JesterPercent.ValueStringFormat = (option, value) => $"{value}%";
            JesterPercent.ShowChildrenConidtion = () => JesterPercent.GetValue() > 0;

            NumberJester.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
