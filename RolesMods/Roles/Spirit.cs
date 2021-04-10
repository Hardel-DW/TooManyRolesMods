using Essentials.Options;
using RolesMods.Utility.CustomRoles;
using RolesMods.Utility.Enumerations;
using UnityEngine;

namespace RolesMods.Roles {


    [RegisterInCustomRoles(typeof(Spirit))]
    public class Spirit : CustomRole<Spirit> {
        // Color: 5b00C2FF
        public static CustomOptionHeader SpiritHeader = CustomOptionHeader.AddHeader("[5b00C2FF]Spirit Options :[]");
        public static CustomNumberOption SpiritPercent = CustomOption.AddNumber("Spirit Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberSpirit = CustomOption.AddNumber("Number Spirit", 1f, 1f, 10f, 1f);
        public static CustomToggleOption CanVoteMultipleTime = CustomOption.AddToggle("Can Vote Multiple time", false);

        public Spirit() : base() {
            GameOptionFormat();
            Name = "Spirit";
            Color = new Color(0.356f, 0f, 0.760f, 1f);
            TasksDescription = "[5b00C2FF]Spirit: You can vote while being dead![]";
            VisibleBy = PlayerSide.Dead;
            Side = PlayerSide.Everyone;
            GiveTasksAt = Moment.OnDie;
            GiveRoleAt = Moment.StartGame;
            RoleActive = true;
            CanHasOtherRole = true;
            ShowIntroCutScene = false;
        }

        public override void OnGameEnded() {
            Systems.Spirit.MeetingHudPopulateButtonsPatch.SpiritHasVoteds.Clear();
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) SpiritPercent.GetValue();
            NumberPlayers = (int) NumberSpirit.GetValue();
        }
        private void GameOptionFormat() {
            SpiritHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            SpiritPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberSpirit.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
