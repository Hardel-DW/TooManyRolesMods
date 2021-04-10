using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    public static class HandleRpcPatch {
        public static bool Prefix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader) {
            if (callId == 250) {
                RoleManager Role = RoleManager.GerRoleById(reader.ReadByte());
                List<byte> selectedPlayers = reader.ReadBytesAndSize().ToList();

                Role.ClearRole();
                for (int i = 0; i < selectedPlayers.Count; i++)
                    Role.AddPlayer(PlayerControlUtils.FromPlayerId(selectedPlayers[i]));

                return false;
            }

            return true;
        }
    }
}
