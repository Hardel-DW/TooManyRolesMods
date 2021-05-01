/*using System.Collections.Generic;
using HarmonyLib;

namespace RolesMods.Systems.Mayor {

    [HarmonyPatch(typeof(PlayerVoteArea))]
    public class Bank {

        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.Select))]
        public static class Select {
            public static bool Prefix(PlayerVoteArea __instance) {
                if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                    return true;

                if (PlayerControl.LocalPlayer.Data.IsDead || __instance.isDead || !Roles.Mayor.CanVote || !__instance.Parent.Select(__instance.TargetPlayerId))
                    return false;

                __instance.Buttons.SetActive(true);
                return false;
            }
        }

        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.VoteForMe))]
        public static class VoteForMe {
            public static bool Prefix(PlayerVoteArea __instance) {
                if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                    return true;

                if (__instance.Parent.state == MeetingHud.VoteStates.Proceeding || __instance.Parent.state == MeetingHud.VoteStates.Results || !Roles.Mayor.CanVote)
                    return false;

                bool noteSelfVote = __instance != Roles.Mayor.Abstain;
                if (noteSelfVote)
                    Roles.Mayor.VoteBank--;

                Roles.Mayor.VotedOnce = noteSelfVote;
                __instance.Parent.Confirm(__instance.TargetPlayerId);
                return false;
            }
        }
    }
}
*/