using Essentials.Options;
using RolesMods.Utility.CustomRoles;
using RolesMods.Utility.Enumerations;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Investigator))]
    public class Investigator : CustomRole<Investigator> {
        // Color: 2EADFFFF
        public static CustomOptionHeader InvestigatorHeader = CustomOptionHeader.AddHeader("<color=#2EADFFFF>Investigator Options :</color>");
        public static CustomNumberOption InvestigatorPercent = CustomOption.AddNumber("Investigator Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberInvestigator = CustomOption.AddNumber("Number Investigator", 1f, 1f, 10f, 1f);
        public static CustomNumberOption footPrintSize = CustomOption.AddNumber("Footprint Size", 0.5f, 0.3f, 1f, 0.1f);
        public static CustomNumberOption fontPrintInterval = CustomOption.AddNumber("Footprint Interval", 3f, 1f, 5f, 0.25f);
        public static CustomNumberOption fontPrintDuration = CustomOption.AddNumber("Footprint Duration", 10f, 3f, 30f, 1f);
        public static CustomToggleOption AnonymousFootPrint = CustomOption.AddToggle("Anonymous Footprint", false);
        public static CustomToggleOption VentFootprintVisible = CustomOption.AddToggle("Footprint are visible arround vent", false);

        public Investigator() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            RoleActive = true;
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.180f, 0.678f, 1f, 1f);
            Name = "Investigator";
            IntroDescription = "Find all imposters by examining footprints";
            TasksDescription = "<color=#2EADFFFF>Investigator: You can see everyone's footprint.</color>";
        }

        public override void OnGameEnded() {
            Systems.Investigator.FootPrint.allFootprint.Clear();
        }

        public override void OnGameStart() {
            Systems.Investigator.FootPrint.allFootprint.Clear();
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) InvestigatorPercent.GetValue();
            NumberPlayers = (int) NumberInvestigator.GetValue();
        }

        private void GameOptionFormat() {
            InvestigatorHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            InvestigatorPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberInvestigator.ValueStringFormat = (option, value) => $"{value} players";
            footPrintSize.ValueStringFormat = (option, value) => $"{value} unit";
            fontPrintInterval.ValueStringFormat = (option, value) => $"{value}s";
            fontPrintDuration.ValueStringFormat = (option, value) => $"{value}s";
        }
    }
}
