using System.Linq;
using Harion.Reactor;
using Harion.Utility.Utils;
using HarmonyLib;
using UnityEngine;

namespace RolesMods.Systems.Swapper {

    [HarmonyPatch(typeof(MeetingHud))]
    public class SwapVotes {
        public static PlayerVoteArea Swap1;
        public static PlayerVoteArea Swap2;

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        public static class MeetingHud_Start {
            public static void Postfix(MeetingHud __instance) {
                Swap1 = null;
                Swap2 = null;
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
        public static class VotingComplete {
            public static void Postfix(MeetingHud __instance) {
                if (Swap1 == null || Swap2 == null)
                    return;

                if (Roles.Swapper.Instance.HasRole(PlayerControl.LocalPlayer))
                    foreach (var button in Roles.Swapper.Instance.Buttons.Where(button => button != null))
                        button.SetActive(false);

                // Player 1
                Transform pool1 = Swap1.PlayerIcon.transform;
                Transform name1 = Swap1.NameText.transform;
                Transform mask1 = Swap1.transform.FindChild("MaskArea");
                Vector2 pooldest1 = pool1.position;
                Vector2 namedest1 = name1.position;
                Vector2 maskdest1 = mask1.position;
                mask1.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                // Player 2
                Transform pool2 = Swap2.PlayerIcon.transform;
                Transform name2 = Swap2.NameText.transform;
                Transform mask2 = Swap2.transform.FindChild("MaskArea");
                Vector2 pooldest2 = pool2.position;
                Vector2 namedest2 = name2.position;
                Vector2 maskdest2 = mask2.position;
                mask2.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                // Slide
                Coroutines.Start(GameObjectUtils.Slide2D(pool1, pooldest1, pooldest2, 2f));
                Coroutines.Start(GameObjectUtils.Slide2D(pool2, pooldest2, pooldest1, 2f));
                Coroutines.Start(GameObjectUtils.Slide2D(name1, namedest1, namedest2, 2f));
                Coroutines.Start(GameObjectUtils.Slide2D(name2, namedest2, namedest1, 2f));
                Coroutines.Start(GameObjectUtils.Slide2D(mask1, maskdest1, maskdest2, 2f));
                Coroutines.Start(GameObjectUtils.Slide2D(mask2, maskdest2, maskdest1, 2f));
            }
        }
    }
}