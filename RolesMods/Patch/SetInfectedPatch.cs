using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using UnhollowerBaseLib;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
    class SetInfectedPatch {
        public static void Postfix(Il2CppReferenceArray<GameData.PlayerInfo> infected) {
            List<PlayerControl> playersList = PlayerControl.AllPlayerControls.ToArray().ToList();

            // Investigator
            if (playersList != null && playersList.Count > 0) {
                List<PlayerControl> crewmateList = playersList.FindAll(x => !x.Data.IsImpostor);

                if (crewmateList != null && crewmateList.Count > 0) {
                    Random random = new Random();
                    GlobalVariable.Investigator = crewmateList[random.Next(0, crewmateList.Count)];
                    byte playerId = GlobalVariable.Investigator.PlayerId;
                    playersList.Remove(GlobalVariable.Investigator);
                    MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.SetInvestigator, SendOption.None, -1);
                    messageWriter.Write(playerId);
                    AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
                }
            }

            // TimeMaster
            if (playersList != null && playersList.Count > 0) {
                Random random = new Random();
                GlobalVariable.TimeMaster = playersList[random.Next(0, playersList.Count)];
                byte playerId = GlobalVariable.Investigator.PlayerId;
                playersList.Remove(GlobalVariable.TimeMaster);
                MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.SetTimeMaster, SendOption.None, -1);
                messageWriter.Write(playerId);
                AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
            }

            // Lighter
            if (playersList != null && playersList.Count > 0) {
                List<PlayerControl> crewmateList = playersList.FindAll(x => !x.Data.IsImpostor);

                if (crewmateList != null && crewmateList.Count > 0) {
                    Random random = new Random();
                    GlobalVariable.Lighter = crewmateList[random.Next(0, crewmateList.Count)];
                    byte playerId = GlobalVariable.Lighter.PlayerId;
                    playersList.Remove(GlobalVariable.Lighter);
                    MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.SetLighter, SendOption.None, -1);
                    messageWriter.Write(playerId);
                    AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
                }
            }
        }
    }
}