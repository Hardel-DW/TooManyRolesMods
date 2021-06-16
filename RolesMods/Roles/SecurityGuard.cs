using HardelAPI.CustomOptions;
using HardelAPI.CustomRoles;
using HardelAPI.Enumerations;
using HardelAPI.Utility.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(SecurityGuard))]
    public class SecurityGuard : CustomRole<SecurityGuard> {
        // Color: #07db00FF
        public static CustomOptionHeader SecurityGuardHeader = CustomOptionHeader.AddHeader("<color=#d4B40cff>Security Guard Options :</color>");
        public static CustomNumberOption SecurityGuardPercent = CustomOption.AddNumber("SecurityGuard Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberSecurityGuard = CustomOption.AddNumber("Number Security Guard", 1f, 1f, 10f, 1f);
        public static CustomNumberOption CooldownSecurityGuard = CustomOption.AddNumber("Security Guard Cooldown", 30f, 10f, 120f, 5f);
        public static CustomNumberOption NumberScrews = CustomOption.AddNumber("Security Guard Number Of Screws", 1f, 1f, 30f, 1f);
        public static CustomNumberOption ScrewsCams = CustomOption.AddNumber("Number Of Screws Per Cam", 1f, 1f, 10f, 1f);
        public static CustomNumberOption ScrewsVent = CustomOption.AddNumber("Number Of Screws Per Vent", 1f, 1f, 10f, 1f);
        public static List<Vector2> camerasToAdd = new List<Vector2>();
        public static List<Vent> ventsToSeal = new List<Vent>();

        public SecurityGuard() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            RoleActive = true;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.831f, 0.705f, 0.047f, 1f);
            Name = "Security Guard";
            IntroDescription = "Seal vents and place cameras";
            TasksDescription = "<color=#d4B40cff>Security Guard: Seal vents and place cameras</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) SecurityGuardPercent.GetValue();
            NumberPlayers = (int) NumberSecurityGuard.GetValue();
        }

        public override void OnMeetingEnd(MeetingHud instance) {
            if (camerasToAdd.Count > 0)
                CameraUtils.RpcAddMutipleCamera(camerasToAdd);

            if (ventsToSeal.Count > 0)
                VentUtils.RpcSealMultipleVent(ventsToSeal);

            camerasToAdd = new List<Vector2>();
            ventsToSeal = new List<Vent>();
        }

        public override void OnGameStarted() {
            camerasToAdd = new List<Vector2>();
            ventsToSeal = new List<Vent>();
            Systems.SecurityGuard.Button.Instance.MaxTimer = CooldownSecurityGuard.GetValue();

            Systems.SecurityGuard.Button.totalScrews = (int) NumberScrews.GetValue();
            Systems.SecurityGuard.Button.camPrice = (int) ScrewsCams.GetValue();
            Systems.SecurityGuard.Button.ventPrice = (int) ScrewsVent.GetValue();
        }

        private void GameOptionFormat() {
            SecurityGuardHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            SecurityGuardPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberSecurityGuard.ValueStringFormat = (option, value) => $"{value} players";
            CooldownSecurityGuard.ValueStringFormat = (option, value) => $"{value}s";
        }
    }
}
