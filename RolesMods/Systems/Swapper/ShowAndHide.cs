/*using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace RolesMods.Systems.Swapper {
    public class ShowHideButtons {
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
        public static class Confirm {
            public static bool Prefix(MeetingHud __instance) {
                if (!Roles.Swapper.Instance.HasRole(PlayerControl.LocalPlayer))
                    return true;

                foreach (var button in Roles.Swapper.Instance.Buttons.Where(button => button != null)) {
                    if (button.GetComponent<SpriteRenderer>().color == Color.white)
                        button.SetActive(false);

                    button.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                }

                if (Roles.Swapper.Instance.ListOfActives.Count(x => x) == 2) {
                    bool Set = true;
                    for (var i = 0; i < Roles.Swapper.Instance.ListOfActives.Count; i++) {
                        if (!Roles.Swapper.Instance.ListOfActives[i])
                            continue;

                        if (Set) {
                            SwapVotes.Swap1 = __instance.playerStates[i];
                            Set = false;
                        } else
                            SwapVotes.Swap2 = __instance.playerStates[i];
                    }
                }

                if (SwapVotes.Swap1 == null || SwapVotes.Swap2 == null)
                    return true;

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.SetSwaps, SendOption.Reliable, -1);
                writer.Write(SwapVotes.Swap1.TargetPlayerId);
                writer.Write(SwapVotes.Swap2.TargetPlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                return true;
            }
        }

        public static byte[] CalculateVotes(MeetingHud __instance) {
            var self = Mayor.RegisterExtraVotes.CalculateAllVotes(__instance);
            var array = new byte[Mathf.Max(PlayerControl.AllPlayerControls.Count + 1, 11)];
            for (var i = 0; i < array.Length; i++) {
                if (SwapVotes.Swap1 == null || SwapVotes.Swap2 == null) {
                    array[i] = self[i];
                    continue;
                }

                if (i == SwapVotes.Swap1.TargetPlayerId + 1) {
                    array[SwapVotes.Swap2.TargetPlayerId + 1] = self[i];
                } else if (i == SwapVotes.Swap2.TargetPlayerId + 1) {
                    array[SwapVotes.Swap1.TargetPlayerId + 1] = self[i];
                } else {
                    array[i] = self[i];
                }
            }

            return array;
        }

        public static void RpcVotingComplete(MeetingHud __instance, byte[] states, GameData.PlayerInfo exiled, bool tie) {
            if (AmongUsClient.Instance.AmClient)
                __instance.VotingComplete(states, exiled, tie);

            MessageWriter messageWriter = AmongUsClient.Instance.StartRpc(__instance.NetId, 23, SendOption.Reliable);
            messageWriter.WriteBytesAndSize(states);
            messageWriter.Write(exiled?.PlayerId ?? byte.MaxValue);
            messageWriter.Write(tie);
            messageWriter.EndMessage();
        }


        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CheckForEndVoting))]
        public static class CheckForEndVoting {
            public static bool Prefix(MeetingHud __instance) {
                if (__instance.playerStates.All(playerState => playerState.AmDead || playerState.DidVote)) {
                    byte[] self = CalculateVotes(__instance);
                    Il2CppStructArray<byte> selfIl2 = self;
                    bool tie;

                    int maxIdx = Extensions.IndexOfMax(selfIl2, (Func<byte, int>) (p => p), out tie) - 1;
                    GameData.PlayerInfo exiled = GameData.Instance.AllPlayers.ToArray().FirstOrDefault(v => v.PlayerId == maxIdx);
                    byte[] array = new byte[10];

                    foreach (var playerVoteArea in __instance.playerStates)
                        array[playerVoteArea.TargetPlayerId] = playerVoteArea.GetState();

                    RpcVotingComplete(__instance, array, exiled, tie);
                }

                return false;
            }
        }
    }
}*/