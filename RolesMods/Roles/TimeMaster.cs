using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.Enumerations;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(TimeMaster))]
    public class TimeMaster : CustomRole<TimeMaster> {
        // Color: 999999FF
        public static CustomNumberOption TimeMasterPercent = CustomOption.AddNumber("<color=#999999FF>TimeMaster Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.CrewmateHolder);
        public static CustomNumberOption NumberTimeMaster = CustomOption.AddNumber("Number TimeMaster", 1f, 1f, 10f, 1f, TimeMasterPercent);
        public static CustomNumberOption TimeMasterDuration = CustomOption.AddNumber("Rewind Duration", 5f, 3f, 30f, 1f, TimeMasterPercent);
        public static CustomNumberOption TimeMasterCooldown = CustomOption.AddNumber("Rewind Cooldown", 30f, 10f, 120f, 5f, TimeMasterPercent);
        public static CustomNumberOption UseNumber = CustomOption.AddNumber("Number of uses", 1f, 1f, 10f, 1f, TimeMasterPercent);
        public static CustomToggleOption EnableReiveTimeMaster = CustomOption.AddToggle("Enable Rivive during rewind", false, TimeMasterPercent);
        public static CustomToggleOption UsableVitals = CustomOption.AddToggle("Time Master can use vitals", true, TimeMasterPercent);

        public TimeMaster() : base() {
            GameOptionFormat();
            Side = PlayerSide.Everyone;
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.490f, 0.490f, 0.490f, 1f);
            Name = "Time Master";
            RoleActive = true;
            IntroDescription = "Bend time as you will";
            TasksDescription = "<color=#999999FF>TimeMaster: You can travel the time and revive other.</color>";
        }

        public override void OnGameEnded() {
            Systems.TimeMaster.Time.ClearGameHistory();
        }

        public override void OnGameStarted() {
            Systems.TimeMaster.Time.recordTime = TimeMasterDuration.GetValue() * 2;
            Systems.TimeMaster.Button.Instance.EffectDuration = TimeMasterDuration.GetValue() / 2;

            Systems.TimeMaster.Button.Instance.MaxTimer = TimeMasterCooldown.GetValue();
            Systems.TimeMaster.Button.Instance.UseNumber = (int) UseNumber.GetValue();
            Systems.TimeMaster.Time.ClearGameHistory();
        }   

        public override void OnInfectedStart() {
            PercentApparition = (int) TimeMasterPercent.GetValue();
            NumberPlayers = (int) NumberTimeMaster.GetValue();
        }

        public override void OnMeetingStart(MeetingHud instance) {
            Systems.TimeMaster.Time.StopRewind();
        }

        private void GameOptionFormat() {
            TimeMasterPercent.ValueStringFormat = (option, value) => $"{value}%";
            TimeMasterPercent.ShowChildrenConidtion = () => TimeMasterPercent.GetValue() > 0;

            NumberTimeMaster.ValueStringFormat = (option, value) => $"{value} players";
            TimeMasterDuration.ValueStringFormat = (option, value) => $"{value}s";
            TimeMasterCooldown.ValueStringFormat = (option, value) => $"{value}s";
            UseNumber.ValueStringFormat = (option, value) => $"{value} times";
        }
    }
}
