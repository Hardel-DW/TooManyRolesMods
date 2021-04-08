using HarmonyLib;
using Hazel;
using RolesMods.Systems.Psychic;
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

            if (callId == (byte) CustomRPC.SendOverlayPsychic) {
                bool show = reader.ReadBoolean();

                if (PsychicSystems.psychicOverlay != null)
                    PsychicSystems.psychicOverlay.SetActive(show);

                return false;
            }

            return true;
        }
    }
}
