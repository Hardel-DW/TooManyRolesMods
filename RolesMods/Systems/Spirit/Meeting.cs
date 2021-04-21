using HardelAPI.Utility;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace RolesMods.Systems.Spirit {

    [HarmonyPatch]
    public static class MeetingHudPopulateButtonsPatch {
        public static List<bool> SpiritHasVoteds = new List<bool>(10);

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
        class MeetingUpdatePatch {
            static void Prefix(MeetingHud __instance) {
                if (__instance.discussionTimer == 0 && SpiritHasVoteds.Count < PlayerControl.AllPlayerControls.ToArray().ToList().Count)
                    SpiritHasVoteds.AddRange(Enumerable.Repeat(default(bool), PlayerControl.AllPlayerControls.ToArray().ToList().Count - SpiritHasVoteds.Count));

                if (Roles.Spirit.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId) && PlayerControl.LocalPlayer.Data.IsDead)
                    if (!SpiritHasVoteds[PlayerControl.LocalPlayer.PlayerId] && __instance.discussionTimer == 0)
                        __instance.SkipVoteButton.gameObject.SetActive(true);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
        class MeetingVotePatch {
            static void Prefix(MeetingHud __instance, [HarmonyArgument(0)] sbyte suspectIdx) {
                if (Roles.Spirit.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId) && PlayerControl.LocalPlayer.Data.IsDead)
                    __instance.CmdCastVote(PlayerControl.LocalPlayer.PlayerId, suspectIdx);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CmdCastVote))]
        class CmdCastVotePatch {
            static void Prefix(MeetingHud __instance, [HarmonyArgument(0)] byte srcPlayerId, [HarmonyArgument(1)] sbyte suspectPlayerId) {
                if (Roles.Spirit.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId) && PlayerControl.LocalPlayer.Data.IsDead) {
                    if (!Roles.Spirit.CanVoteMultipleTime.GetValue() && !SpiritHasVoteds[srcPlayerId])
                        SpiritHasVoteds[srcPlayerId] = true;
                    foreach (PlayerVoteArea player in __instance.playerStates) {
                        player.ClearButtons();
                        __instance.SkipVoteButton.gameObject.SetActive(false);
                        if (player.TargetPlayerId == PlayerControl.LocalPlayer.Data.PlayerId) {
                            player.didVote = true;
                            player.votedFor = suspectPlayerId;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CastVote))]
        class CastVoteNormal {
            static void Prefix(MeetingHud __instance, [HarmonyArgument(0)] byte srcPlayerId, [HarmonyArgument(1)] sbyte suspectPlayerId) {
                if (Roles.Spirit.Instance.HasRole(srcPlayerId) && PlayerControlUtils.FromPlayerId(srcPlayerId).Data.IsDead) {

                    foreach (PlayerVoteArea player in __instance.playerStates) {
                        if (player.TargetPlayerId == srcPlayerId) {
                            if (!Roles.Spirit.CanVoteMultipleTime.GetValue() && !SpiritHasVoteds[srcPlayerId])
                                SpiritHasVoteds[srcPlayerId] = true;

                            player.didVote = true;
                            player.votedFor = suspectPlayerId;
                            player.Flag.enabled = true;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.Select))]
        class SelectVoteArea {
            static void Prefix(PlayerVoteArea __instance) {
                if (Roles.Spirit.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId) && PlayerControl.LocalPlayer.Data.IsDead) {
                    MeetingHud MeetingInstance = __instance.Parent;

                    foreach (PlayerVoteArea player in MeetingInstance.playerStates)
                        player.Buttons.SetActive(false);

                    if (!__instance.isDead && __instance.Parent.state != MeetingHud.VoteStates.Discussion && !MeetingInstance.DidVote(PlayerControl.LocalPlayer.PlayerId) && !SpiritHasVoteds[PlayerControl.LocalPlayer.PlayerId])
                        __instance.Buttons.SetActive(true);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.GetState))]
        class GetStatePatch {
            static bool Prefix(ref byte __result, PlayerVoteArea __instance) {
                if (Roles.Spirit.Instance.HasRole((byte) __instance.TargetPlayerId) && __instance.isDead && __instance.didVote && PlayerControl.GameOptions.AnonymousVotes) {
                    __result = (byte) ((int) (__instance.votedFor + 1 & 15) | (0) | (__instance.didVote ? 64 : 0) | (__instance.didReport ? 32 : 0));
                    return false;
                }

                return true;
            }
        }
    }
}