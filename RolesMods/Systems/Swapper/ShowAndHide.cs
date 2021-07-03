using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using RolesMods.Systems.Mayor;
using UnityEngine;
using UnityEngine.UI;

namespace RolesMods.Systems.Swapper {
    public class ShowHideButtons {
        public static Dictionary<byte, int> CalculateVotes(MeetingHud __instance) {
            Dictionary<byte, int> self = RegisterExtraVotes.CalculateAllVotes(__instance);
            if (SwapVotes.Swap1 == null || SwapVotes.Swap2 == null)
                return self;

            int swap1 = 0;
            if (self.TryGetValue(SwapVotes.Swap1.TargetPlayerId, out var value))
                swap1 = value;

            int swap2 = 0;
            if (self.TryGetValue(SwapVotes.Swap2.TargetPlayerId, out var value2))
                swap2 = value2;

            self[SwapVotes.Swap2.TargetPlayerId] = swap1;
            self[SwapVotes.Swap1.TargetPlayerId] = swap2;

            return self;
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
        public static class Confirm {
            public static bool Prefix(MeetingHud __instance) {
                if (!Roles.Swapper.Instance.HasRole(PlayerControl.LocalPlayer))
                    return true;

                foreach (GameObject button in Roles.Swapper.Instance.Buttons.Where(button => button != null)) {
                    if (button.GetComponent<SpriteRenderer>().sprite == ResourceLoader.SwapDisableSprite)
                        button.SetActive(false);

                    button.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                }

                if (Roles.Swapper.Instance.ListOfActives.Count(x => x) == 2) {
                    var toSet1 = true;
                    for (var i = 0; i < Roles.Swapper.Instance.ListOfActives.Count; i++) {
                        if (!Roles.Swapper.Instance.ListOfActives[i])
                            continue;

                        if (toSet1) {
                            SwapVotes.Swap1 = __instance.playerStates[i];
                            toSet1 = false;
                        } else {
                            SwapVotes.Swap2 = __instance.playerStates[i];
                        }
                    }
                }


                if (SwapVotes.Swap1 == null || SwapVotes.Swap2 == null)
                    return true;

                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.SetSwaps, SendOption.Reliable, -1);
                writer.Write(SwapVotes.Swap1.TargetPlayerId);
                writer.Write(SwapVotes.Swap2.TargetPlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                return true;
            }
        }
    }
}