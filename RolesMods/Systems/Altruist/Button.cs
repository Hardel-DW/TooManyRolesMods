using Harion.Reactor;
using Harion.Utility.Utils;
using HarmonyLib;
using Hazel;
using System.Collections;
using System.Linq;
using UnityEngine;
using Harion.Cooldown;
using AltruistRoles = RolesMods.Roles.Altruist;

namespace RolesMods.Systems.Altruist {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {

        public override void OnCreateButton() {
            Closest = Harion.Cooldown.ClosestElement.DeadBody;
            Roles = AltruistRoles.Instance;
            DecreamteUseNimber = UseNumberDecremantion.OnClick;
            UseNumber = 1;
            SetSprite(ResourceLoader.RewindRedSprite);
        }

        public override void OnClick() {
            DeadBody body = GetDeadBodyTarget();
            if (body != null) {
                PlayerControl.LocalPlayer.RpcMurderPlayer(PlayerControl.LocalPlayer);
                Coroutines.Start(Ability(body, PlayerControl.LocalPlayer));

                MessageWriter write = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.AltrusitRevive, SendOption.None, -1);
                write.Write(body.ParentId);
                write.Write(PlayerControl.LocalPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(write);
            }
        }

        public static IEnumerator Ability(DeadBody deadbody, PlayerControl altruist) {
            Coroutines.Start(PlayerControlUtils.FlashCoroutine(AltruistRoles.Instance.Color));
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