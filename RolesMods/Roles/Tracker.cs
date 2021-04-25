using Essentials.Options;
using HardelAPI.CustomRoles;
using HardelAPI.Enumerations;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Tracker))]
    public class Tracker : CustomRole<Tracker> {
        // Color: #07db00FF
        public static CustomOptionHeader TrackerHeader = CustomOptionHeader.AddHeader("<color=#07db00FF>Tracker Options :</color>");
        public static CustomNumberOption TrackerPercent = CustomOption.AddNumber("Tracker Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberTracker = CustomOption.AddNumber("Number Tracker", 1f, 1f, 10f, 1f);
        public static CustomNumberOption TargetUpdate = CustomOption.AddNumber("Arrow Interval", 5f, 0f, 30f, 0.5f);

        public Tracker() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            RoleActive = true;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.027f, 0.858f, 0f, 1f);
            Name = "Tracker";
            IntroDescription = "Track a player and do your tasks";
            TasksDescription = "<color=#07db00FF>Tracker: You can see the player\nposition with an arrow.</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) TrackerPercent.GetValue();
            NumberPlayers = (int) NumberTracker.GetValue();
        }

        public override void OnMeetingStart(MeetingHud instance) {
            if (Systems.Tracker.Button.Arrow != null)
                Object.Destroy(Systems.Tracker.Button.Arrow.Arrow);

            Systems.Tracker.Button.usable = true;
        }

        public override void OnGameStarted() {
            Systems.Tracker.Button.usable = true;
        }

        private void GameOptionFormat() {
            TrackerHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            TrackerPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberTracker.ValueStringFormat = (option, value) => $"{value} players";
            TargetUpdate.ValueStringFormat = (option, value) => $"{value}s";
        }
    }
}
