using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Systems.TimeMaster {
    public static class Time {

        public static float recordTime = RolesMods.TimeMasterDuration.GetValue();
        public static bool isRewinding = false;
        private static List<TimePoint> pointsInTime = new List<TimePoint>();
        private static long deadtime;
        private static bool isDead = false;
            
        public static void Record() {
            if (pointsInTime.Count > Mathf.Round(recordTime / UnityEngine.Time.fixedDeltaTime)) {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }

            if (PlayerControl.LocalPlayer != null) {
                pointsInTime.Insert(0, new TimePoint(
                    PlayerControl.LocalPlayer.transform.position,
                    PlayerControl.LocalPlayer.gameObject.GetComponent<Rigidbody2D>().velocity,
                    DateTimeOffset.Now.ToUnixTimeSeconds()
                ));

                if (PlayerControl.LocalPlayer.Data.IsDead && !isDead) {
                    isDead = true;
                    deadtime = DateTimeOffset.Now.ToUnixTimeSeconds();
                }

                if (!PlayerControl.LocalPlayer.Data.IsDead && isDead) {
                    isDead = false;
                    deadtime = 0;
                }
            }
        }

        public static void Rewind() {
            if (pointsInTime.Count > 0) {
                TimePoint currentTimePoint = pointsInTime[0];

                PlayerControl.LocalPlayer.transform.position = currentTimePoint.Position;
                PlayerControl.LocalPlayer.gameObject.GetComponent<Rigidbody2D>().velocity = currentTimePoint.Velocity;

                if (isDead && currentTimePoint.Unix < deadtime && PlayerControl.LocalPlayer.Data.IsDead && RolesMods.EnableReiveTimeMaster.GetValue()) {
                    PlayerControl.LocalPlayer.Revive();
                    var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == PlayerControl.LocalPlayer.PlayerId);

                    if (body != null)
                        UnityEngine.Object.Destroy(body.gameObject);

                    deadtime = 0;
                    isDead = false;

                    MessageWriter write = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.TimeRevive, SendOption.None, -1);
                    write.Write(PlayerControl.LocalPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(write);
                }

                pointsInTime.RemoveAt(0);
            } else {
                StopRewind();
            }
        }

        public static void StartRewind() {
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
        }
    }
}
