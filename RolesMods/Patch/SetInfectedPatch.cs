using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using UnhollowerBaseLib;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
    class SetInfectedPatch {
        public static void Postfix([HarmonyArgument(0)] Il2CppReferenceArray<GameData.PlayerInfo> infected) {
            List<PlayerControl> playersList = PlayerControl.AllPlayerControls.ToArray().ToList();
            HelperRoles.ClearRoles();

            // TimeMaster
            if (playersList != null && playersList.Count > 0 && RolesMods.EnableTimeMaster.GetValue()) {
                Random random = new Random();
                GlobalVariable.TimeMaster = playersList[random.Next(0, playersList.Count)];
                byte playerId = GlobalVariable.TimeMaster.PlayerId;
                playersList.Remove(GlobalVariable.TimeMaster);
                MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.SetTimeMaster, SendOption.None, -1);
                messageWriter.Write(playerId);
                AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
            }

            // Investigator
            if (playersList != null && playersList.Count > 0 && RolesMods.EnableInvestigator.GetValue()) {
                MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.SetInvestigator, SendOption.None, -1);
                List<byte> playerSelected = new List<byte>();

                for (int i = 0; i < RolesMods.NumberInvestigator.GetValue(); i++) {
                    List<PlayerControl> crewmateList = playersList.FindAll(x => !x.Data.IsImpostor).ToArray().ToList();
                    
                    if (crewmateList != null && crewmateList.Count > 0) {
                        Random random = new Random();
                        PlayerControl selectedPlayer = crewmateList[random.Next(0, crewmateList.Count)];
                        GlobalVariable.InvestigatorsList.Add(selectedPlayer);
                        playersList.Remove(selectedPlayer);
                        playerSelected.Add(selectedPlayer.PlayerId);
                    }
                }

                messageWriter.WriteBytesAndSize(playerSelected.ToArray());
                AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
            }

            // Lighter
            if (playersList != null && playersList.Count > 0 && RolesMods.EnableLighter.GetValue()) {
                MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.SetLighter, SendOption.None, -1);
                List<byte> playerSelected = new List<byte>();

                for (int i = 0; i < RolesMods.NumberLighter.GetValue(); i++) {
                    List<PlayerControl> crewmateList = playersList.FindAll(x => !x.Data.IsImpostor).ToArray().ToList();
                    
                    if (crewmateList != null && crewmateList.Count > 0) {
                        Random random = new Random();
                        PlayerControl selectedPlayer = crewmateList[random.Next(0, crewmateList.Count)];
                        GlobalVariable.LightersList.Add(selectedPlayer);
                        playersList.Remove(selectedPlayer);
                        playerSelected.Add(selectedPlayer.PlayerId);
                    }
                }

                messageWriter.WriteBytesAndSize(playerSelected.ToArray());
                AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
            }
        }
    }
}