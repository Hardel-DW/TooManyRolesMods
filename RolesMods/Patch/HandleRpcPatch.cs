using HarmonyLib;
using Hazel;
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

            if (callId == (byte) CustomRPC.DebileWin) {
                foreach (var player in Roles.Jester.Instance.AllPlayers) {
                    player.Revive();
                    player.Data.IsDead = false;
                    player.Data.IsImpostor = true;
                }

                foreach (var player in PlayerControl.AllPlayerControls) {
                    if (!Roles.Jester.Instance.HasRole(player.PlayerId)) {
                        player.RemoveInfected();
                        player.Die(DeathReason.Exile);
                        player.Data.IsImpostor = false;
                    }
                }

                Systems.Jester.ExiledPatch.JesterForceEndGame = true;
                return false;
            }

            return true;
        }
    }
}
