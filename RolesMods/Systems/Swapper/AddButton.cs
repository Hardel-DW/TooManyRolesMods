using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace RolesMods.Systems.Swapper {

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public class AddButton {

        private static int mostRecentId;

        public static void GenButton(int index, bool isDead) {
            if (isDead) {
                Roles.Swapper.Instance.Buttons.Add(null);
                Roles.Swapper.Instance.ListOfActives.Add(false);
                return;
            }

            GameObject confirmButton = MeetingHud.Instance.playerStates[index].Buttons.transform.GetChild(0).gameObject;
            GameObject newButton = Object.Instantiate(confirmButton, MeetingHud.Instance.playerStates[index].transform);
            SpriteRenderer renderer = newButton.GetComponent<SpriteRenderer>();
            PassiveButton passive = newButton.GetComponent<PassiveButton>();

            renderer.sprite = ResourceLoader.SwapDisableSprite;
            newButton.transform.position = confirmButton.transform.position - new Vector3(0.5f, 0f, 0f);
            newButton.transform.localScale *= 0.8f;
            newButton.layer = 5;
            newButton.transform.parent = confirmButton.transform.parent.parent;

            passive.OnClick = new Button.ButtonClickedEvent();
            passive.OnClick.AddListener(SetActive(index));
            Roles.Swapper.Instance.Buttons.Add(newButton);
            Roles.Swapper.Instance.ListOfActives.Add(false);

            SwapVotes.Swap1 = null;
            SwapVotes.Swap2 = null;
            bool SetValue = true;
            for (var i = 0; i < Roles.Swapper.Instance.ListOfActives.Count; i++) {
                if (!Roles.Swapper.Instance.ListOfActives[i])
                    continue;

                if (SetValue) {
                    SwapVotes.Swap1 = MeetingHud.Instance.playerStates[i];
                    SetValue = false;
                } else {
                    SwapVotes.Swap2 = MeetingHud.Instance.playerStates[i];
                }
            }


            if (SwapVotes.Swap1 == null || SwapVotes.Swap2 == null) {
                MessageWriter writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.SetSwaps, SendOption.Reliable, -1);
                writer2.Write(sbyte.MaxValue);
                writer2.Write(sbyte.MaxValue);
                AmongUsClient.Instance.FinishRpcImmediately(writer2);
                return;
            }

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.SetSwaps, SendOption.Reliable, -1);
            writer.Write(SwapVotes.Swap1.TargetPlayerId);
            writer.Write(SwapVotes.Swap2.TargetPlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

        }

        private static Action SetActive(int index) {
            void Listener() {
                if (Roles.Swapper.Instance.ListOfActives.Count(x => x) == 2 &&
                    Roles.Swapper.Instance.Buttons[index].GetComponent<SpriteRenderer>().sprite == ResourceLoader.SwapDisableSprite)
                    return;

                Roles.Swapper.Instance.Buttons[index].GetComponent<SpriteRenderer>().color =
                    Roles.Swapper.Instance.ListOfActives[index] ? Color.white : Color.green;

                Roles.Swapper.Instance.Buttons[index].GetComponent<SpriteRenderer>().sprite = Roles.Swapper.Instance.ListOfActives[index] ? ResourceLoader.SwapDisableSprite : ResourceLoader.SwapSprite    ;

                Roles.Swapper.Instance.ListOfActives[index] = !Roles.Swapper.Instance.ListOfActives[index];
                mostRecentId = index;
            }

            return Listener;
        }

        public static void Postfix(MeetingHud __instance) {
            Roles.Swapper.Instance.ListOfActives.Clear();
            Roles.Swapper.Instance.Buttons.Clear();

            if (PlayerControl.LocalPlayer.Data.IsDead || !Roles.Swapper.Instance.HasRole(PlayerControl.LocalPlayer))
                return;

            for (var i = 0; i < __instance.playerStates.Length; i++)
                GenButton(i, __instance.playerStates[i].AmDead);
        }
    }
}
