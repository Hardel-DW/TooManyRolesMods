using HarmonyLib;
using Hazel;

namespace RolesMods.Systems.Jester {
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Exiled))]
    public static class ExiledPatch {
        public static bool JesterForceEndGame = false;
        public static bool tasksRemoved = false;

        public static void Prefix(PlayerControl __instance) {
            if (Roles.Jester.Instance.HasRole(__instance.PlayerId)) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.DebileWin, Hazel.SendOption.None, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);

                foreach (var player in Roles.Jester.Instance.AllPlayers) {
                    player.Revive();
                    player.Data.IsDead = false;
                    player.Data.IsImpostor = true;
                }

                foreach (var player in PlayerControl.AllPlayerControls) {
                    if (!Roles.Jester.Instance.HasRole(player.PlayerId)) {
                        player.RemoveInfected();
                        player.Die(DeathReason.Exile);
                        player.Data.IsDead = true;
                        player.Data.IsImpostor = false;
                    }
                }

                JesterForceEndGame = true;
                ShipStatus.RpcEndGame(GameOverReason.ImpostorByKill, false);
            }
        }
    }

    [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
    public static class ExileControllerPatch {
        public static void Postfix([HarmonyArgument(0)] GameData.PlayerInfo exiled, ExileController __instance) {
            if (Roles.Jester.Instance.HasRole(exiled.PlayerId)) {
                __instance.completeString = exiled.PlayerName + " was the Jester.";
            }
        }
    }
}
