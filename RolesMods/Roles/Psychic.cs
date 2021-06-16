using HardelAPI.CustomOptions;
using UnityEngine;
using HardelAPI.CustomRoles;
using HardelAPI.Enumerations;
using HardelAPI.CustomKeyBinds;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Psychic))]
    public class Psychic : CustomRole<Psychic> {
        // Color: BA02BBFF
        public static CustomOptionHeader PsychicHeader = CustomOptionHeader.AddHeader("<color=#BA02BBFF>Psychic Options :</color>");
        public static CustomNumberOption PsychicPercent = CustomOption.AddNumber("Psychic Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberPsychic = CustomOption.AddNumber("Number Psychic", 1f, 1f, 10f, 1f);
        public static CustomNumberOption PsychicDuration = CustomOption.AddNumber("Vision Duration", 5f, 3f, 30f, 1f);
        public static CustomNumberOption PsychicCooldown = CustomOption.AddNumber("Vision Cooldown", 30f, 10f, 120f, 5f);
        public static CustomToggleOption AnonymousPlayerMinimap = CustomOption.AddToggle("Anonymous player on minimap", false);
        public static CustomToggleOption DeadBodyVisible = CustomOption.AddToggle("Dead body visible", false);

        public Psychic() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.73f, 0f, 0.73f, 1f);
            Name = "Psychic";
            RoleActive = true;
            IntroDescription = "Your can see everyone, everywhere";
            TasksDescription = "<color=#BA02BBFF>Psychic: You can see everyone, everywhere</color>";
        }

        public override void OnGameStarted() {
            Systems.Psychic.Button.Instance.EffectDuration = PsychicDuration.GetValue();
            Systems.Psychic.Button.Instance.MaxTimer = PsychicCooldown.GetValue();

            Systems.Psychic.PsychicMap.isPsychicActivated = false;
            Systems.Psychic.PsychicMap.herePointsDatas.Clear();
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) PsychicPercent.GetValue();
            NumberPlayers = (int) NumberPsychic.GetValue();
        }

        public override void OnMeetingStart(MeetingHud instance) {
            Systems.Psychic.PsychicMap.ClearAllPlayers();
        }

        private void GameOptionFormat() {
            PsychicHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            PsychicPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberPsychic.ValueStringFormat = (option, value) => $"{value} players";
            PsychicDuration.ValueStringFormat = (option, value) => $"{value}s";
            PsychicCooldown.ValueStringFormat = (option, value) => $"{value}s";
        }
    }
}
