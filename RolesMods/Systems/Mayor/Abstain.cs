using UnityEngine;
using HarmonyLib;
using Harion.CustomRoles;

namespace RolesMods.Systems.Mayor {
    public class Abstain {
        public static void UpdateButton(MeetingHud __instance) {
            PlayerVoteArea skip = __instance.SkipVoteButton;
            Roles.Mayor.Abstain.gameObject.SetActive(skip.gameObject.active && !Roles.Mayor.VotedOnce);
            Roles.Mayor.Abstain.voteComplete = skip.voteComplete;
            Roles.Mayor.Abstain.GetComponent<SpriteRenderer>().enabled = skip.GetComponent<SpriteRenderer>().enabled;
            Roles.Mayor.Abstain.GetComponent<SpriteRenderer>().sprite = ResourceLoader.AbstainButton;
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        public class MeetingHudStart {
            public static void GenButton(MeetingHud __instance) {
                PlayerVoteArea skip = __instance.SkipVoteButton;
                Roles.Mayor.Abstain = Object.Instantiate(skip, skip.transform.parent);
                Roles.Mayor.Abstain.Parent = __instance;
                Roles.Mayor.Abstain.SetTargetPlayerId(251);
                Roles.Mayor.Abstain.transform.localPosition = skip.transform.localPosition + new Vector3(0f, -0.17f, 0f);
                skip.transform.localPosition += new Vector3(0f, 0.20f, 0f);
                UpdateButton(__instance);
            }

            public static void Postfix(MeetingHud __instance) {
                if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                    return;

                GenButton(__instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.ClearVote))]
        public class MeetingHudClearVote {
            public static void Postfix(MeetingHud __instance) {
                if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                    return;

                UpdateButton(__instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
        public class MeetingHudConfirm {
            public static void Postfix(MeetingHud __instance) {
                if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                    return;

                Roles.Mayor.Abstain.ClearButtons();
                UpdateButton(__instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Select))]
        public class MeetingHudSelect {
            public static void Postfix(MeetingHud __instance, [HarmonyArgument(0)] int playerId) {
                if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                    return;

                if (playerId != 251) {
                    Roles.Mayor.Abstain.ClearButtons();
                }

                UpdateButton(__instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
        public class MeetingHudVotingComplete {
            public static void Postfix(MeetingHud __instance) {
                if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                    return;

                UpdateButton(__instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
        public class MeetingHudUpdate {
            public static void Postfix(MeetingHud __instance) {
                if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                    return;

                switch (__instance.state) {
                    case MeetingHud.VoteStates.Discussion:
                        if (__instance.discussionTimer < PlayerControl.GameOptions.DiscussionTime) {
                            Roles.Mayor.Abstain.SetDisabled();
                            break;
                        }

                        Roles.Mayor.Abstain.SetEnabled();
                        break;
                }
                UpdateButton(__instance);
            }
        }
    }
}
