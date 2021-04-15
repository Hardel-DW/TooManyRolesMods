using HarmonyLib;
using Hazel;
using RolesMods.Utility.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;

namespace RolesMods.Utility.CustomRoles.Patch {
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
    public static class SetInfectedPatch {
        public static void Postfix([HarmonyArgument(0)] Il2CppReferenceArray<GameData.PlayerInfo> infected) {
            List<PlayerControl> playersList = PlayerControl.AllPlayerControls.ToArray().ToList();
            RoleManager.ClearAllRoles();

            foreach (var Role in RoleManager.AllRoles) {
                Role.OnInfectedStart();

                if (!(Role.Side == PlayerSide.Everyone || Role.Side == PlayerSide.Crewmate || Role.Side == PlayerSide.Impostor))
                    throw new Exception($"Error in the selection of players, for the {Role.Name} Role. \n The player Side has only three possible values: Crewmate, Impostors or Everyone, Given: {Role.Side}");

                int PercentApparition = new Random().Next(0, 100);

                Plugin.Logger.LogInfo($"Role: {Role.Name}, Active: {Role.RoleActive}, PercentApparition: {Role.PercentApparition}, Number Player: {Role.NumberPlayers}");
                if (playersList != null && playersList.Count > 0 && Role.RoleActive && Role.PercentApparition > PercentApparition) {

                    MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 250, SendOption.None, -1);
                    messageWriter.Write(Role.RoleId);
                    List<byte> playerSelected = new List<byte>();

                    for (int i = 0; i < Role.NumberPlayers; i++) {
                        List<PlayerControl> PlayerSelectable = playersList;
                        if (Role.Side == PlayerSide.Impostor)
                            PlayerSelectable.RemoveAll(x => !x.Data.IsImpostor);

                        if (Role.Side == PlayerSide.Crewmate)
                            PlayerSelectable.RemoveAll(x => x.Data.IsImpostor);

                        if (PlayerSelectable != null && PlayerSelectable.Count > 0) {
                            Random random = new Random();
                            PlayerControl selectedPlayer = PlayerSelectable[random.Next(0, PlayerSelectable.Count)];
                            Role.AddPlayer(selectedPlayer);
                            playersList.Remove(selectedPlayer);
                            playerSelected.Add(selectedPlayer.PlayerId);
                            Plugin.Logger.LogInfo($"Role: {Role.Name}, Given to: {selectedPlayer.nameText.text}");
                        }
                    }

                    messageWriter.WriteBytesAndSize(playerSelected.ToArray());
                    AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
                }

                Role.WhiteListKill = null;
            }
        }
    }
}
