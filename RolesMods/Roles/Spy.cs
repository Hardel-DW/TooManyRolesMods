using Harion.CustomKeyBinds;
using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.Enumerations;
using UnityEngine;

namespace RolesMods.Roles {
    [RegisterInCustomRoles(typeof(Spy))]
    public class Spy : CustomRole<Spy> {
        // Color: #B5DFBCFF
        public static CustomNumberOption SpyPercent = CustomOption.AddNumber("Spy", "<color=#B5DFBCFF>Spy Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.CrewmateHolder);
        public static CustomNumberOption NumberSpy = CustomOption.AddNumber("Number Spy", 1f, 1f, 10f, 1f, SpyPercent);
        public static CustomNumberOption SpyCooldown = CustomOption.AddNumber("Cooldown Spy", 30f, 10f, 120f, 5f, SpyPercent);
        public static CustomNumberOption SpyDuration = CustomOption.AddNumber("Duration Spy", 10f, 5f, 30f, 2.5f, SpyPercent);
        public static CustomNumberOption NumberUse = CustomOption.AddNumber("Number Use", 5f, 1f, 20f, 1f, SpyPercent);
        public static CustomToggleOption SpySeeApproxitiveColor = CustomOption.AddToggle("Spy see Black/White color on admin", false, SpyPercent);
        public static bool CanUse;

        public Spy() : base() {
            GameOptionFormat();
            TasksDescription = "<color=#B5DFBCFF>Spy: Get more information on admin and vital</color>";
            IntroDescription = "Get more information !";
            Name = "Spy";
            Side = PlayerSide.Crewmate;
            GiveTasksAt = Moment.StartGame;
            RoleActive = true;
            Color = new Color(0f, 0.741f, 0.478f, 1f);
        }

        public override void OnInfectedStart() {
            Systems.Spy.Button.Instance.MaxTimer = SpyCooldown.GetValue();
            Systems.Spy.Button.Instance.EffectDuration = SpyDuration.GetValue();
            Systems.Spy.Button.Instance.UseNumber = (int) NumberUse.GetValue();
            PercentApparition = (int) SpyPercent.GetValue();
            NumberPlayers = (int) NumberSpy.GetValue();
        }

        private void GameOptionFormat() {
            SpyPercent.ValueStringFormat = (option, value) => $"{value}%";
            SpyPercent.ShowChildrenConidtion = () => SpyPercent.GetValue() > 0;

            NumberSpy.ValueStringFormat = (option, value) => $"{value} players";

            SpyCooldown.ValueStringFormat = (option, value) => $"{value}s";
            SpyDuration.ValueStringFormat = (option, value) => $"{value}s";
            NumberUse.ValueStringFormat = (option, value) => $"{value} time";
        }
    }
}