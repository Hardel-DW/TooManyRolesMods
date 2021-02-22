using HarmonyLib;
using Hazel;
using RolesMods.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    class HandleRpcPatch {

        public static bool Prefix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader) {

            if (callId == (byte) CustomRPC.SetInvestigator) {
                GlobalVariable.InvestigatorsList.Clear();
                List<byte> selectedPlayers = reader.ReadBytesAndSize().ToList();

                for (int i = 0; i < selectedPlayers.Count; i++)
                    GlobalVariable.InvestigatorsList.Add(PlayerControlUtils.FromPlayerId(selectedPlayers[i]));

                return false;
            }

            if (callId == (byte) CustomRPC.SetTimeMaster) {
                GlobalVariable.TimeMaster = null;
                byte readByte = reader.ReadByte();
                foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                    if (player.PlayerId == readByte) {
                        GlobalVariable.TimeMaster = player;
                    }
                }

                return false;
            }

            if (callId == (byte) CustomRPC.SetLighter) {
                GlobalVariable.LightersList.Clear();
                List<byte> selectedPlayers = reader.ReadBytesAndSize().ToList();

                for (int i = 0; i < selectedPlayers.Count; i++)
                    GlobalVariable.LightersList.Add(PlayerControlUtils.FromPlayerId(selectedPlayers[i]));

                return false;
            }

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

            return true;
        }
    }
}
