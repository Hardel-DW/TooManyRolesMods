using Harion.CustomOptions;
using Harion.CustomRoles;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Mayor))]
    public class Mayor : CustomRole<Mayor> {
        // Color: #614474FF
        public static CustomNumberOption MayorPercent = CustomOption.AddNumber("Mayor", "<color=#614474FF>Mayor Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.CrewmateHolder);
        public static CustomNumberOption NumberMayor = CustomOption.AddNumber("Number Mayor", 1f, 1f, 10f, 1f, MayorPercent);
        
        public static List<byte> ExtraVotes = new List<byte>();
        public static int VoteBank { get; set; }
        public static bool SelfVote { get; set; }
        public static bool VotedOnce { get; set; }
        public static PlayerVoteArea Abstain { get; set; }
        public static bool CanVote => VoteBank > 0 && !SelfVote;

        public Mayor() : base() {
            GameOptionFormat();
            Color = new Color(0.380f, 0.266f, 0.454f, 1f);
            Name = "Mayor";
            IntroDescription = "Everone trust on you";
            TasksDescription = "<color=#614474FF>Mayor: Your vote count as twice</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) MayorPercent.GetValue();
            NumberPlayers = (int) NumberMayor.GetValue();
        }

        private void GameOptionFormat() {
            MayorPercent.ValueStringFormat = (option, value) => $"{value}%";
            MayorPercent.ShowChildrenConidtion = () => MayorPercent.GetValue() > 0;

            NumberMayor.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
