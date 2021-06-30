using HarmonyLib;

namespace RolesMods.Systems.Mayor {

    [HarmonyPatch(typeof(PlayerVoteArea))]
    public class Bank {

        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.Select))]
        public static class Select {
            public static bool Prefix(PlayerVoteArea __instance) {
                if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                    return true;

                if (PlayerControl.LocalPlayer.Data.IsDead || __instance.AmDead)
                    return false;

                if (!Roles.Mayor.CanVote || !__instance.Parent.Select(__instance.TargetPlayerId))
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

                if (__instance.Parent.state == MeetingHud.VoteStates.Proceeding || __instance.Parent.state == MeetingHud.VoteStates.Results)
                    return false;

                if (!Roles.Mayor.CanVote)
                    return false;

                if (__instance != Roles.Mayor.Abstain) {
                    Roles.Mayor.VoteBank--;
                    Roles.Mayor.VotedOnce = true;
                } else {
                    Roles.Mayor.SelfVote = true;
                }

                __instance.Parent.Confirm(__instance.TargetPlayerId);
                return false;
            }
        }
    }
}
