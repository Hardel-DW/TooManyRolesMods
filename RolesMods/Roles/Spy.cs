using Essentials.Options;
using HardelAPI.CustomRoles;
using HardelAPI.Enumerations;
using UnityEngine;

namespace RolesMods.Roles {
    [RegisterInCustomRoles(typeof(Spy))]
    public class Spy : CustomRole<Spy> {
        // Color: #00bd7aff
        public static CustomOptionHeader SpyHeader = CustomOptionHeader.AddHeader("<color=#00bd7aff>Spy Options :</color>");
        public static CustomNumberOption SpyPercent = CustomOption.AddNumber("Spy Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberSpy = CustomOption.AddNumber("Number Spy", 1f, 1f, 10f, 1f);
        public static CustomNumberOption SpyCooldown = CustomOption.AddNumber("Cooldown Spy", 30f, 10f, 120f, 5f);
        public static CustomNumberOption SpyDuration = CustomOption.AddNumber("Duration Spy", 10f, 5f, 30f, 2.5f);
        public static CustomNumberOption NumberUse = CustomOption.AddNumber("Number Use", 5f, 1f, 20f, 1f);
        public static CustomToggleOption SpySeeApproxitiveColor = CustomOption.AddToggle("Spy see Black/White color on admin", false);
        public static bool CanUse;

        public Spy() : base() {
            GameOptionFormat();
            TasksDescription = "<color=#00bd7aff>Spy: Get more information on admin and vital</color>";
            IntroDescription = "Get more information !";
            Name = "Spy";
            Side = PlayerSide.Crewmate;
            GiveTasksAt = Moment.StartGame;
            RoleActive = true;
            Color = new Color(0f, 0.741f, 0.478f, 1f);
        }

        public override void OnInfectedStart() {
            Systems.Spy.Button.isActive = false;

            Systems.TimeMaster.Button.buttonTime.MaxTimer = SpyCooldown.GetValue();
            Systems.TimeMaster.Button.buttonTime.EffectDuration = SpyDuration.GetValue();
            Systems.Spy.Button.UseNumber = (int) NumberUse.GetValue();
            PercentApparition = (int) SpyPercent.GetValue();
            NumberPlayers = (int) NumberSpy.GetValue();
        }

        private void GameOptionFormat() {
            SpyHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            SpyPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberSpy.ValueStringFormat = (option, value) => $"{value} players";

            SpyCooldown.ValueStringFormat = (option, value) => $"{value}s";
            SpyDuration.ValueStringFormat = (option, value) => $"{value}s";
            NumberUse.ValueStringFormat = (option, value) => $"{value} time";
        }
    }
}