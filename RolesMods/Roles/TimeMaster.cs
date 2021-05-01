using Essentials.Options;
using HardelAPI.CustomRoles;
using HardelAPI.Enumerations;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(TimeMaster))]
    public class TimeMaster : CustomRole<TimeMaster> {
        // Color: 999999FF
        public static CustomOptionHeader TimeMasterHeader = CustomOptionHeader.AddHeader("<color=#999999FF>TimeMaster Options :</color>");
        public static CustomNumberOption TimeMasterPercent = CustomOption.AddNumber("TimeMaster Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberTimeMaster = CustomOption.AddNumber("Number TimeMaster", 1f, 1f, 10f, 1f);
        public static CustomNumberOption TimeMasterDuration = CustomOption.AddNumber("Rewind Duration", 5f, 3f, 30f, 1f);
        public static CustomNumberOption TimeMasterCooldown = CustomOption.AddNumber("Rewind Cooldown", 30f, 10f, 120f, 5f);
        public static CustomNumberOption UseNumber = CustomOption.AddNumber("Number of uses", 1f, 1f, 10f, 1f);
        public static CustomToggleOption EnableReiveTimeMaster = CustomOption.AddToggle("Enable Rivive during rewind", false);
        public static CustomToggleOption UsableVitals = CustomOption.AddToggle("Time Master can use vitals", true);

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
            Systems.TimeMaster.Button.buttonTime.EffectDuration = TimeMasterDuration.GetValue() / 2;
            Systems.TimeMaster.Button.buttonTime.MaxTimer = TimeMasterCooldown.GetValue();
            Systems.TimeMaster.Time.ClearGameHistory();
            Systems.TimeMaster.Button.UseNumber = (int) UseNumber.GetValue();
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) TimeMasterPercent.GetValue();
            NumberPlayers = (int) NumberTimeMaster.GetValue();
        }

        public override void OnMeetingStart(MeetingHud instance) {
            Systems.TimeMaster.Time.StopRewind();
        }

        private void GameOptionFormat() {
            TimeMasterHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            TimeMasterPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberTimeMaster.ValueStringFormat = (option, value) => $"{value} players";
            TimeMasterDuration.ValueStringFormat = (option, value) => $"{value}s";
            TimeMasterCooldown.ValueStringFormat = (option, value) => $"{value}s";
            UseNumber.ValueStringFormat = (option, value) => $"{value} times";
        }
    }
}
