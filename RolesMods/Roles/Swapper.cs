using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.Enumerations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Swapper))]
    public class Swapper : CustomRole<Swapper> {
        // Color: #0cd418ff
        public static CustomNumberOption SwapperPercent = CustomOption.AddNumber("Swapper", "<color=#0cd418ff>Swapper Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.CrewmateHolder);
        public static CustomNumberOption NumberSwapper = CustomOption.AddNumber("Number Swapper", 1f, 1f, 10f, 1f, SwapperPercent);
        public static CustomNumberOption UseNumber = CustomOption.AddNumber("Swap use", 1f, 1f, 10f, 1f, SwapperPercent);

        // Store
        public readonly List<bool> ListOfActives = new List<bool>();
        public readonly List<GameObject> Buttons = new List<GameObject>();
        public static byte playerId1 = Byte.MaxValue;
        public static byte playerId2 = Byte.MaxValue;

        public Swapper() : base() {
            GameOptionFormat();
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.047f, 0.831f, 0.094f, 1f);
            Name = "Swapper";
            IntroDescription = "Swap vote to exile the impostors";
            TasksDescription = "<color=#0cd418ff>Swapper: Swap vote to exile the impostors</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) SwapperPercent.GetValue();
            NumberPlayers = (int) NumberSwapper.GetValue();
        }

        public override void OnMeetingStart(MeetingHud instance) {
            //Systems.Swapper.SwapVotes.Swap1 = null;
            //Systems.Swapper.SwapVotes.Swap2 = null;
        }

        private void GameOptionFormat() {
            SwapperPercent.ValueStringFormat = (option, value) => $"{value}%";
            SwapperPercent.ShowChildrenConidtion = () => SwapperPercent.GetValue() > 0;

            NumberSwapper.ValueStringFormat = (option, value) => $"{value} players";
            UseNumber.ValueStringFormat = (option, value) => $"{value} time";
        }
    }
}