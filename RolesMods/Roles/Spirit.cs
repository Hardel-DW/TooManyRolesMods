using Harion;
using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.Enumerations;
using UnityEngine;

namespace RolesMods.Roles {


    [RegisterInCustomRoles(typeof(Spirit))]
    public class Spirit : CustomRole<Spirit> {
        // Color: 5b00C2FF
        public static CustomNumberOption SpiritPercent = CustomOption.AddNumber("Spirit", "<color=#5b00C2FF>Spirit Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.DeadHolder);
        public static CustomNumberOption NumberSpirit = CustomOption.AddNumber("Number Spirit", 1f, 1f, 10f, 1f, SpiritPercent);
        public static CustomToggleOption CanVoteMultipleTime = CustomOption.AddToggle("Can Vote Multiple time", false, SpiritPercent);

        public Spirit() : base() {
            GameOptionFormat();
            Name = "Spirit";
            Color = new Color(0.356f, 0f, 0.760f, 1f);
            TasksDescription = "<color=#5b00C2FF>Spirit: You can vote while being dead!</color>";
            VisibleBy = VisibleBy.Dead;
            Side = PlayerSide.Everyone;
            GiveTasksAt = Moment.OnDie;
            GiveRoleAt = Moment.StartGame;
            RoleType = RoleType.Dead;
            RoleActive = true;
            ShowIntroCutScene = false;
        }

        public override void OnGameEnded() {
            //Systems.Spirit.MeetingHudPopulateButtonsPatch.SpiritHasVoteds.Clear();
        }

        public override void OnGameStarted() {
            //HardelApiPlugin.DeadSeeAllRoles.SetValue(false);
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) SpiritPercent.GetValue();
            NumberPlayers = (int) NumberSpirit.GetValue();
        }
        private void GameOptionFormat() {
            SpiritPercent.ValueStringFormat = (option, value) => $"{value}%";
            SpiritPercent.ShowChildrenConidtion = () => SpiritPercent.GetValue() > 0;

            NumberSpirit.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
