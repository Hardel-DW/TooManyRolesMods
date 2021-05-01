using HardelAPI.Utility;
using HarmonyLib;
using Hazel;
using System.Linq;
using UnityEngine;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    class HandleRpcPatch {

        public static bool Prefix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader) {
            if (callId == (byte) CustomRPC.TimeRewind) {
                Systems.TimeMaster.Time.isRewinding = true;
                PlayerControl.LocalPlayer.moveable = false;
                HudManager.Instance.FullScreen.color = new Color(0f, 0.5f, 0.8f, 0.3f);
                HudManager.Instance.FullScreen.enabled = true;
                return false;
            }

            if (callId == (byte) CustomRPC.TimeRevive) {
                PlayerControl player = PlayerControlUtils.FromPlayerId(reader.ReadByte());
                player.Revive();
                var body = Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == player.PlayerId);

                if (body != null)
                    Object.Destroy(body.gameObject);
                return false;
            }

            if (callId == (byte) CustomRPC.EngineerFix)
                Systems.Engineer.Button.FixSabotage();

            if (callId == (byte) CustomRPC.AltrusitRevive) {
                byte deadBodyId = reader.ReadByte();
                DeadBody deadPlayer = Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == deadBodyId);
                PlayerControl altruist = PlayerControlUtils.FromPlayerId(reader.ReadByte());

                Systems.Altruist.Button.Ability(deadPlayer, altruist);
            }

            return true;
        }
    }
}
