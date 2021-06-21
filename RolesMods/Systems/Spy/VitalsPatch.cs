/*using Harion.Data;
using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

namespace RolesMods.Systems.Spy {

    [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Begin))]
    class VitalsMinigameBeginPatch {
        static void Postfix(VitalsMinigame __instance) {

            if (__instance.vitals.Length > 10) {
                for (int i = 0; i < __instance.vitals.Length; i++) {
                    var vitalsPanel = __instance.vitals[i];
                    var player = GameData.Instance.AllPlayers[i];
                    vitalsPanel.Text.text = player.PlayerName.Length >= 4 ? player.PlayerName.Substring(0, 4).ToUpper() : player.PlayerName.ToUpper();
                }
            }
        }
    }

    [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Update))]
    class VitalsPatch {
        static void Postfix(VitalsMinigame __instance) {
            // Hacker show time since death            
            for (int i = 0; i < __instance.vitals.Length; i++) {
                VitalsPanel vitalsPanel = __instance.vitals[i];
                GameData.PlayerInfo player = GameData.Instance.AllPlayers[i];

                // Crowded scaling
                float scale = 10f / Mathf.Max(10, __instance.vitals.Length);
                vitalsPanel.transform.localPosition = new Vector3((float) i * 0.6f * scale + -2.7f, 0.2f, -1f);
                vitalsPanel.transform.localScale = new Vector3(scale, scale, vitalsPanel.transform.localScale.z);

                // Hacker update
                if (vitalsPanel.IsDead) {
                    DeadPlayer deadPlayer = DeadPlayer.deadPlayers?.Where(x => x.player?.PlayerId == player?.PlayerId)?.FirstOrDefault();
                    if (deadPlayer != null && deadPlayer.timeOfDeath != null) {
                        float timeSinceDeath = ((float) (DateTime.UtcNow - deadPlayer.timeOfDeath).TotalMilliseconds);

                        if (Button.Instance.IsEffectActive)
                            vitalsPanel.Text.text = Math.Round(timeSinceDeath / 1000) + "s";
                        else if (__instance.vitals.Length > 10)
                            vitalsPanel.Text.text = player.PlayerName.Length >= 4 ? player.PlayerName.Substring(0, 4).ToUpper() : player.PlayerName.ToUpper();
                        else
                            vitalsPanel.Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(Palette.ShortColorNames[(int) player.ColorId], new UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>(0));
                    }
                }
            }
        }
    }
}
*/