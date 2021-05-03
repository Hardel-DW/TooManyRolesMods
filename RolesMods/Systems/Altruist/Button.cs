using HardelAPI.Utility;
using HarmonyLib;
using Hazel;
using Reactor;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RolesMods.Systems.Altruist {

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class Button {
        public static CooldownButton button;
        public static DeadBody closestbody;
        public static int UseNumber = 1;

        public static void Postfix(HudManager __instance) {
            button = new CooldownButton
                (() => OnClick(),
                Roles.Altruist.AltruistCooldown.GetValue(),
                Plugin.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Rewind.png", 250f),
                250,
                new Vector2(0f, 0f),
                __instance,
                () => OnUpdate(button)
            );
        }

        private static void OnClick() {
            if (UseNumber > 0) {
                UseNumber--;

                if (closestbody != null) {
                    PlayerControl.LocalPlayer.RpcMurderPlayer(PlayerControl.LocalPlayer);
                    Coroutines.Start(Ability(closestbody, PlayerControl.LocalPlayer));

                    MessageWriter write = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.AltrusitRevive, SendOption.None, -1);
                    write.Write(closestbody.ParentId);
                    write.Write(PlayerControl.LocalPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(write);
                }
            }
        }

        private static void OnUpdate(CooldownButton button) {
            if (Roles.Altruist.Instance.AllPlayers != null && PlayerControl.LocalPlayer != null) {
                if (Roles.Altruist.Instance.HasRole(PlayerControl.LocalPlayer)) {
                    if (PlayerControl.LocalPlayer.Data.IsDead || UseNumber <= 0)
                        button.SetCanUse(false);
                    else button.SetCanUse(!MeetingHud.Instance);

                    if (closestbody != null) {
                        closestbody.GetComponent<SpriteRenderer>().material.SetFloat("_Outline", 0f);
                        button.isDisable = false;
                    } else {
                        button.isDisable = true;
                    }

                    DeadBody target = PlayerControlUtils.GetClosestDeadBody(PlayerControl.LocalPlayer);
                    if (target != null) {
                        SpriteRenderer component = target.GetComponent<SpriteRenderer>();
                        component.material.SetFloat("_Outline", 1f);
                        component.material.SetColor("_OutlineColor", Roles.Altruist.Instance.Color);
                        closestbody = target;
                    } else {
                        closestbody = null;
                    }
                }
            }
        }

        public static IEnumerator Ability(DeadBody deadbody, PlayerControl altruist) {
            Coroutines.Start(PlayerControlUtils.FlashCoroutine(Roles.Altruist.Instance.Color));
            PlayerControl playerFromDEead = PlayerControlUtils.FromPlayerId(deadbody.ParentId);
            Object.Destroy(deadbody.gameObject);
            yield return new WaitForSeconds(5);
            DeadBody altruistBody = Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == altruist.PlayerId);
            if (altruistBody != null)
                Object.Destroy(altruistBody.gameObject);

            playerFromDEead.Revive();
            yield return true;
        }
    }
}