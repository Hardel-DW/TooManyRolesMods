using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.Enumerations;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Tracker))]
    public class Tracker : CustomRole<Tracker> {
        // Color: #07db00FF
        public static CustomNumberOption TrackerPercent = CustomOption.AddNumber("<color=#07db00FF>Tracker Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.CrewmateHolder);
        public static CustomNumberOption NumberTracker = CustomOption.AddNumber("Number Tracker", 1f, 1f, 10f, 1f, TrackerPercent);
        public static CustomNumberOption TargetUpdate = CustomOption.AddNumber("Arrow Interval", 5f, 0f, 30f, 0.5f, TrackerPercent);

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

        public override void OnGameStarted() {
            Systems.Tracker.Button.Instance.UseNumber = int.MaxValue;
        }

        public override void OnMeetingStart(MeetingHud instance) {
            if (Systems.Tracker.Button.Arrow != null)
                Object.Destroy(Systems.Tracker.Button.Arrow.Arrow);

            Systems.Tracker.Button.Instance.UseNumber = int.MaxValue;
        }

        private void GameOptionFormat() {
            TrackerPercent.ValueStringFormat = (option, value) => $"{value}%";
            TrackerPercent.ShowChildrenConidtion = () => TrackerPercent.GetValue() > 0;

            NumberTracker.ValueStringFormat = (option, value) => $"{value} players";
            TargetUpdate.ValueStringFormat = (option, value) => $"{value}s";
        }
    }
}
