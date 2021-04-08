using Essentials.Options;
using RolesMods.Utility.CustomRoles;
using RolesMods.Utility.Enumerations;
using System;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Investigator))]
    public class Investigator : CustomRole<Investigator> {
        // Color: 2EADFFFF
        public static CustomOptionHeader InvestigatorHeader = CustomOptionHeader.AddHeader("\n[2EADFFFF]Investigator Options :[]");
        public static CustomToggleOption EnableInvestigator = CustomOption.AddToggle("Enable Investigator", false);
        public static CustomNumberOption NumberInvestigator = CustomOption.AddNumber("Number Investigator", 1f, 1f, 10f, 1f);
        public static CustomNumberOption footPrintSize = CustomOption.AddNumber("Footprint Size", 0.75f, 0.3f, 1f, 0.1f);
        public static CustomNumberOption fontPrintInterval = CustomOption.AddNumber("Footprint Interval", 1f, 0.25f, 5f, 0.25f);
        public static CustomNumberOption fontPrintDuration = CustomOption.AddNumber("Footprint Duration", 10f, 3f, 30f, 1f);
        public static CustomToggleOption AnonymousFootPrint = CustomOption.AddToggle("Anonymous Footprint", false);
        public static CustomToggleOption VentFootprintVisible = CustomOption.AddToggle("Footprint are visible arround vent", false);

        public Investigator() : base() {
            NumberInvestigator.HudStringFormat = (option, name, value) => $"{name}: {value}%";
            Side = PlayerSide.Crewmate;
            Color = new Color(0.180f, 0.678f, 1f, 1f);
            Name = "Investigator";
            IntroDescription = "Find all imposters by examining footprints";
            TasksDescription = "[2EADFFFF]You are an Investigator, you can see everyone's footprint.[]";
        }

        public override void OnGameEnded() {
            Systems.Investigator.FootPrint.allFootprint.Clear();
        }

        public override void OnGameStart() {
            Systems.Investigator.FootPrint.allFootprint.Clear();
        }

        public override void OnInfectedStart() {
            RoleActive = EnableInvestigator.GetValue();
            NumberPlayers = (int) NumberInvestigator.GetValue();
        }
    }
}
