using Essentials.Options;
using RolesMods.Utility.CustomRoles;
using RolesMods.Utility.Enumerations;
using UnityEngine;

namespace RolesMods.Roles {


    [RegisterInCustomRoles(typeof(Lighter))]
    public class Lighter : CustomRole<Lighter> {
        // Color: BA5B13FF
        public static CustomOptionHeader LighterHeader = CustomOptionHeader.AddHeader("\n[BA5B13FF]Lighter Options :[]");
        public static CustomToggleOption EnableLighter = CustomOption.AddToggle("Enable Lighter", false);
        public static CustomNumberOption NumberLighter = CustomOption.AddNumber("Number Lighter", 1f, 1f, 10f, 1f);
        public static CustomNumberOption LighterMultiplier = CustomOption.AddNumber("Lighter Multiplier", 1.5f, 0f, 5f, 0.05f);
        public static CustomToggleOption LighterSabotageVision = CustomOption.AddToggle("Lighter sees during electrical sabotage", true);
        
        public Lighter() : base() {
            TasksDescription = "[BA5B13FF]Lighter: Your vision is better than crewmate[]";
            IntroDescription = "Your vision is better than crewmate";
            Name = "Lighter";
            Side = PlayerSide.Crewmate;
            Color = new Color(0.729f, 0.356f, 0.074f, 1f);
        }

        public override void OnInfectedStart() {
            RoleActive = EnableLighter.GetValue();
            NumberPlayers = (int) NumberLighter.GetValue();
        }
    }
}
