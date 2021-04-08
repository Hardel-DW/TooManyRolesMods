using RolesMods.Utility.CustomRoles;
using Essentials.Options;
using UnityEngine;
using RolesMods.Utility.Enumerations;

namespace RolesMods.Roles {


    [RegisterInCustomRoles(typeof(Psychic))]
    public class Psychic : CustomRole<Psychic> {
        // Color: BA02BBFF
        public static CustomOptionHeader PsychicHeader = CustomOptionHeader.AddHeader("\n[BA02BBFF]Psychic Options :[]");
        public static CustomToggleOption EnablePsychic = CustomOption.AddToggle("Enable Psychic", false);
        public static CustomNumberOption NumberPsychic = CustomOption.AddNumber("Number Psychic", 1f, 1f, 10f, 1f);
        public static CustomNumberOption PsychicDuration = CustomOption.AddNumber("Vision Duration", 5f, 3f, 30f, 1f);
        public static CustomNumberOption PsychicCooldown = CustomOption.AddNumber("Vision Cooldown", 30f, 10f, 120f, 5f);
        public static CustomToggleOption AnonymousPlayerMinimap = CustomOption.AddToggle("Anonymous player on minimap", false);
        public static CustomToggleOption DeadBodyVisible = CustomOption.AddToggle("Dead body visible", false);

        public Psychic() : base() {
            Side = PlayerSide.Crewmate;
            Color = new Color(0.73f, 0f, 0.73f, 1f);
            Name = "Lighter";
            IntroDescription = "Your can see everyone, everywhere";
            TasksDescription = "[BA02BBFF]Psychic: You can see everyone, everywhere[]";
        }

        public override void OnGameStart() {
            Systems.Psychic.Button.buttonPsychic.EffectDuration = PsychicDuration.GetValue();
            Systems.Psychic.Button.buttonPsychic.MaxTimer = PsychicCooldown.GetValue();

            Systems.Psychic.PsychicSystems.isPsychicActivated = false;
            Systems.Psychic.PsychicSystems.herePoints.Clear();
            Systems.Psychic.PsychicSystems.texts.Clear();
        }

        public override void OnInfectedStart() {
            RoleActive = EnablePsychic.GetValue();
            NumberPlayers = (int) NumberPsychic.GetValue();
        }

        public override void OnMeetingStart() {
            Systems.Psychic.PsychicSystems.SyncOverlay(false);
        }
    }
}
