using Essentials.Options;
using RolesMods.Utility.CustomRoles;
using RolesMods.Utility.Enumerations;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(TimeMaster))]
    public class TimeMaster : CustomRole<TimeMaster> {
        // Color: 999999FF
        public static CustomOptionHeader TimeMasterHeader = CustomOptionHeader.AddHeader("\n[999999FF]TimeMaster Options :[]");
        public static CustomToggleOption EnableTimeMaster = CustomOption.AddToggle("Enable TimeMaster", false);
        public static CustomNumberOption NumberTimeMaster = CustomOption.AddNumber("Number TimeMaster", 1f, 1f, 10f, 1f);
        public static CustomNumberOption TimeMasterDuration = CustomOption.AddNumber("Rewind Duration", 5f, 3f, 30f, 1f);
        public static CustomNumberOption TimeMasterCooldown = CustomOption.AddNumber("Rewind Cooldown", 30f, 10f, 120f, 5f);
        public static CustomToggleOption EnableReiveTimeMaster = CustomOption.AddToggle("Enable Rivive during rewind", false);

        public TimeMaster() : base() {
            Side = PlayerSide.Everyone;
            Color = new Color(0.490f, 0.490f, 0.490f, 1f);
            Name = "TimeMaster";
            IntroDescription = "Bend time as you will";
            TasksDescription = "[999999FF]TimeMaster: You can travel the time and revive other.[]";
        }

        public override void OnGameEnded() {
            Systems.TimeMaster.Time.ClearGameHistory();
        }

        public override void OnGameStart() {
            Plugin.Logger.LogInfo(TimeMasterDuration.GetValue());
            Systems.TimeMaster.Time.recordTime = TimeMasterDuration.GetValue() * 2;
            Systems.TimeMaster.Button.buttonTime.EffectDuration = TimeMasterDuration.GetValue() / 2;
            Systems.TimeMaster.Button.buttonTime.MaxTimer = TimeMasterCooldown.GetValue();
            Systems.TimeMaster.Time.ClearGameHistory();
        }

        public override void OnInfectedStart() {
            RoleActive = EnableTimeMaster.GetValue();
            NumberPlayers = (int) NumberTimeMaster.GetValue();
        }

        public override void OnMeetingStart() {
            Systems.TimeMaster.Time.StopRewind();
        }
    }
}
