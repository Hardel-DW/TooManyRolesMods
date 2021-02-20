using HarmonyLib;
using Hazel;
using RolesMods.Utility;
using System.Linq;
using UnityEngine;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    class HandleRpcPatch {

        public static bool Prefix(byte callId, MessageReader reader) {
            if (callId == (byte) CustomRPC.SetInvestigator) {
                byte readByte = reader.ReadByte();
                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                    if (player.PlayerId == readByte)
                        GlobalVariable.Investigator = player;

                return false;
            }

            if (callId == (byte) CustomRPC.SetTimeMaster) {
                byte readByte = reader.ReadByte();
                foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                    if (player.PlayerId == readByte) {
                        GlobalVariable.TimeMaster = player;
                    }
                }

                return false;
            }

            if (callId == (byte) CustomRPC.SetLighter) {
                byte readByte = reader.ReadByte();
                foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                    if (player.PlayerId == readByte) {
                        GlobalVariable.Lighter = player;
                    }
                }

                return false;
            }

            if (callId == (byte) CustomRPC.TimeRewind) {
                Systems.TimeMaster.Time.isRewinding = true;
                Player.LocalPlayer.PlayerControl.moveable = false;
                HudManager.Instance.FullScreen.color = new Color(0f, 0.5f, 0.8f, 0.3f);
                HudManager.Instance.FullScreen.enabled = true;
                return false;
            }

            if (callId == (byte) CustomRPC.TimeRevive) {
                Player player = Player.FromPlayerId(reader.ReadByte());
                player.Revive();
                var body = Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == player.PlayerId);

                if (body != null)
                    Object.Destroy(body.gameObject);
                return false;
            }

            return true;
        }
    }
}
