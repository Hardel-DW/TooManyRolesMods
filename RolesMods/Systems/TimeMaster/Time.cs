using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Systems.TimeMaster {
    public static class Time {
        public static Dictionary<byte, List<GameHistory>> PlayersPositions = new Dictionary<byte, List<GameHistory>>();
        public static Dictionary<byte, DateTime> DeadPlayers = new Dictionary<byte, DateTime>();

        public static float recordTime = RolesMods.TimeMasterDuration.GetValue();
        public static bool isRewinding = false;

        public static void Record() {
            foreach (var player in PlayerControl.AllPlayerControls) {
                if (!PlayersPositions.ContainsKey(player.PlayerId))
                    PlayersPositions[player.PlayerId] = new List<GameHistory>();

                var currentPlayer = PlayersPositions.FirstOrDefault(d => d.Key == player.PlayerId);
                while (currentPlayer.Value.Count >= Mathf.Round(RolesMods.TimeMasterDuration.GetValue() / UnityEngine.Time.fixedDeltaTime))
                    currentPlayer.Value.RemoveAt(currentPlayer.Value.Count - 1);
                currentPlayer.Value.Insert(0, new GameHistory(player.transform.position, DateTime.UtcNow, player.gameObject.GetComponent<Rigidbody2D>().velocity));

                if (player.Data.IsDead && !DeadPlayers.ContainsKey(player.PlayerId))
                    DeadPlayers[player.PlayerId] = DateTime.UtcNow;
            }
        }

        public static void Rewind() {
            foreach (var player in PlayerControl.AllPlayerControls) {
                if (!PlayersPositions.ContainsKey(player.PlayerId))
                    continue;

                List<GameHistory> gameHistory = PlayersPositions.FirstOrDefault(d => d.Key == player.PlayerId).Value;
                
                if (gameHistory.Count > 0) {
                    if (!player.inVent) {
                        var currentGemeHistory = gameHistory[0];
                        player.transform.position = currentGemeHistory.position;
                        player.gameObject.GetComponent<Rigidbody2D>().velocity = currentGemeHistory.velocity;

                        if (RolesMods.EnableReiveTimeMaster.GetValue() && player.Data.IsDead)
                            if (DeadPlayers.ContainsKey(player.PlayerId))
                                if (currentGemeHistory.positionTime < DeadPlayers[player.PlayerId])
                                    TimeMasterRevive(player.PlayerId);

                        if (Minigame.Instance) {
                            try {
                                Minigame.Instance.Close();
                            } catch { }
                        }
                    }

                    gameHistory.RemoveAt(0);
                }   
            }

            bool CanStopRewind = true;
            foreach (var player in PlayerControl.AllPlayerControls) {
                if (!PlayersPositions.ContainsKey(player.PlayerId))
                    continue;

                List<GameHistory> gameHistory = PlayersPositions.FirstOrDefault(d => d.Key == player.PlayerId).Value;
                if (gameHistory.Count != 0 || gameHistory == null)
                    CanStopRewind = false;
            }

            if (CanStopRewind)
                StopRewind();
        }

        public static void StartRewind() {
            RolesMods.Logger.LogInfo(RolesMods.EnableReiveTimeMaster.GetValue());

            isRewinding = true;
            PlayerControl.LocalPlayer.moveable = false;
            HudManager.Instance.FullScreen.color = new Color(0f, 0.5f, 0.8f, 0.3f);
            HudManager.Instance.FullScreen.enabled = true;

            MessageWriter write = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.TimeRewind, SendOption.None, -1);
            AmongUsClient.Instance.FinishRpcImmediately(write);
        }

        public static void StopRewind() {
            isRewinding = false;
            PlayerControl.LocalPlayer.moveable = true;
            HudManager.Instance.FullScreen.enabled = false;
            HudManager.Instance.FullScreen.color = new Color(1f, 0f, 0f, 0.3f);
        }

        public static void TimeMasterRevive(byte playerId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == playerId) {
                    player.Revive();
                    var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == playerId);
                    if (body != null)
                        UnityEngine.Object.Destroy(body.gameObject);

                    if (DeadPlayers.ContainsKey(player.PlayerId))
                        DeadPlayers.Remove(player.PlayerId);
                }
            }
        }

        public static void ClearGameHistory() {
            PlayersPositions.Clear();
            DeadPlayers.Clear();
        }
    }
}
