using System;
using System.Collections.Generic;
using System.Linq;
using Harion.CustomRoles;
using HarmonyLib;
using InnerNet;
using UnhollowerBaseLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RolesMods.Systems.Mayor {

    [HarmonyPatch(typeof(MeetingHud))]
    public class RegisterExtraVotes {
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
        public static class Confirm {
            public static bool Prefix(MeetingHud __instance) {
                if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                    return true;

                if (__instance.state != MeetingHud.VoteStates.Voted)
                    return true;

                __instance.state = MeetingHud.VoteStates.NotVoted;
                return true;
            }

            [HarmonyPriority(Priority.First)]
            public static void Postfix(MeetingHud __instance) {
                if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                    return;

                if (Roles.Mayor.CanVote)
                    __instance.SkipVoteButton.gameObject.SetActive(true);
            }
        }

        [HarmonyPatch(nameof(MeetingHud.Update))]
        public static void Postfix(MeetingHud __instance) {
            if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                return;

            if (PlayerControl.LocalPlayer.Data.IsDead)
                return;

            if (__instance.TimerText.text.Contains("Can Vote"))
                return;

            __instance.TimerText.text = $"Can Vote: {Roles.Mayor.VoteBank} time(s) | {__instance.TimerText.text}";
        }

        public static Dictionary<byte, int> CalculateAllVotes(MeetingHud __instance) {
            Dictionary<byte, int> dictionary = new Dictionary<byte, int>();
            for (var i = 0; i < __instance.playerStates.Length; i++) {
                PlayerVoteArea playerVoteArea = __instance.playerStates[i];

                if (!playerVoteArea.DidVote || playerVoteArea.AmDead || playerVoteArea.VotedFor == PlayerVoteArea.MissedVote || playerVoteArea.VotedFor == PlayerVoteArea.DeadVote)
                    continue;

                if (dictionary.TryGetValue(playerVoteArea.VotedFor, out var num))
                    dictionary[playerVoteArea.VotedFor] = num + 1;
                else
                    dictionary[playerVoteArea.VotedFor] = 1;
            }

            foreach (RoleManager Role in RoleManager.AllRoles) {
                foreach (byte number in Roles.Mayor.ExtraVotes) {
                    if (dictionary.TryGetValue(number, out var num))
                        dictionary[number] = num + 1;
                    else
                        dictionary[number] = 1;
                }
            }

            return dictionary;
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CastVote))]
        public static class CastVote {
            public static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)] byte srcPlayerId, [HarmonyArgument(1)] byte suspectPlayerId) {

                PlayerControl player = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.PlayerId == srcPlayerId);
                if (!Roles.Mayor.Instance.HasRole(PlayerControl.LocalPlayer))
                    return true;

                PlayerVoteArea playerVoteArea = __instance.playerStates.ToArray().First(pv => pv.TargetPlayerId == srcPlayerId);

                if (playerVoteArea.AmDead)
                    return false;

                if (PlayerControl.LocalPlayer.PlayerId == srcPlayerId || AmongUsClient.Instance.GameMode != GameModes.LocalGame) {
                    SoundManager.Instance.PlaySound(__instance.VoteLockinSound, false, 1f);
                }

                if (playerVoteArea.DidVote) {
                    Roles.Mayor.ExtraVotes.Add(suspectPlayerId);
                    Roles.Mayor.VoteBank--;
                } else {
                    playerVoteArea.SetVote(suspectPlayerId);
                    playerVoteArea.Flag.enabled = true;
                    PlayerControl.LocalPlayer.RpcSendChatNote(srcPlayerId, ChatNoteTypes.DidVote);
                }
                __instance.Cast<InnerNetObject>().SetDirtyBit(1U);
                __instance.CheckForEndVoting();

                return false;
            }
        }

        [HarmonyPatch(nameof(MeetingHud.Start))]
        public static void Prefix() {
            Roles.Mayor.ExtraVotes.Clear();
            if (Roles.Mayor.VoteBank < 0)
                Roles.Mayor.VoteBank = 0;

            Roles.Mayor.VoteBank++;
            Roles.Mayor.SelfVote = false;
            Roles.Mayor.VotedOnce = false;
        }

        private static void Vote(MeetingHud __instance, GameData.PlayerInfo votingPlayer, int amountOfVotes, GameObject origin, bool isMayor = false) {
            SpriteRenderer renderer = Object.Instantiate(__instance.PlayerVotePrefab);
            if (PlayerControl.GameOptions.AnonymousVotes && isMayor)
                PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, renderer);
            else
                PlayerControl.SetPlayerMaterialColors(votingPlayer.ColorId, renderer);

            renderer.transform.SetParent(origin.transform);
            renderer.transform.localPosition = __instance.VoteOrigin + new Vector3(__instance.VoteButtonOffsets.x * amountOfVotes, 0f, 0f);
            renderer.transform.localScale = Vector3.zero;
            __instance.StartCoroutine(Effects.Bloop(amountOfVotes * 0.3f, renderer.transform, 1f, 0.5f));
            origin.GetComponent<VoteSpreader>().AddVote(renderer);
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateResults))]
        public static class PopulateResults {
            public static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)] Il2CppStructArray<MeetingHud.VoterState> statess) {
                Dictionary<int, int> allNums = new Dictionary<int, int>();

                __instance.TitleText.text = Object.FindObjectOfType<TranslationController>().GetString(StringNames.MeetingVotingResults, Array.Empty<Il2CppSystem.Object>());
                int amountOfSkippedVoters = 0;
                for (var i = 0; i < __instance.playerStates.Length; i++) {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    playerVoteArea.ClearForResults();
                    allNums.Add(i, 0);

                    for (var stateIdx = 0; stateIdx < statess.Length; stateIdx++) {
                        MeetingHud.VoterState voteState = statess[stateIdx];
                        GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(voteState.VoterId);
                        if (playerInfo == null) {
                            Debug.LogError(string.Format("Couldn't find player info for voter: {0}", voteState.VoterId));
                        } else if (i == 0 && voteState.SkippedVote) {
                            __instance.BloopAVoteIcon(playerInfo, amountOfSkippedVoters, __instance.SkippedVoting.transform);
                            amountOfSkippedVoters++;
                        } else if (voteState.VotedForId == playerVoteArea.TargetPlayerId) {
                            Vote(__instance, playerInfo, allNums[i], playerVoteArea.gameObject);
                            allNums[i]++;
                        }
                    }
                }

                foreach (PlayerControl PlayerMayor in Roles.Mayor.Instance.AllPlayers) {
                    GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(PlayerMayor.PlayerId);
                    foreach (var extraVote in Roles.Mayor.ExtraVotes) {
                        if (extraVote == PlayerVoteArea.HasNotVoted || extraVote == PlayerVoteArea.MissedVote || extraVote == PlayerVoteArea.DeadVote) {
                            continue;
                        }

                        if (extraVote == PlayerVoteArea.SkippedVote) {
                            Vote(__instance, playerInfo, amountOfSkippedVoters, __instance.SkippedVoting, true);
                            amountOfSkippedVoters++;
                        } else {
                            for (var i = 0; i < __instance.playerStates.Length; i++) {
                                var area = __instance.playerStates[i];
                                if (extraVote != area.TargetPlayerId)
                                    continue;

                                Vote(__instance, playerInfo, allNums[i], area.gameObject, true);
                                allNums[i]++;
                            }
                        }
                    }
                }

                return false;
            }
        }
    }
}