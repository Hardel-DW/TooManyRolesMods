using UnityEngine;

namespace RolesMods.Utility {
    public class PlayerControlUtils {
        public const float OffsetTruePositionX = 0;
        public const float OffsetTruePositionY = 0.366667f;

        public static Vector2 TruePositionOffset = new Vector2(OffsetTruePositionX, OffsetTruePositionY);

        public static Vector2 Position(PlayerControl player) {
            return player.GetTruePosition() + TruePositionOffset;
        }

        public static PlayerControl FromNetId(uint netId) {
            foreach (var player in PlayerControl.AllPlayerControls)
                if (player.NetId == netId)
                    return player;

            return null;
        }

        public static PlayerControl FromPlayerId(byte id) {
            for (int i = 0; i < PlayerControl.AllPlayerControls.Count; i++)
                if (PlayerControl.AllPlayerControls[i].PlayerId == id)
                    return PlayerControl.AllPlayerControls[i];

            return null;
        }

        /// <summary>
        /// Set the players opacity (hat bugs a bit)
        /// </summary>
        /// <param name="opacity">Opacity value from 0 - 1</param>
        public void SetOpacity(float opacity, PlayerControl player) {
            var toSetColor = new Color(1, 1, 1, opacity);
            player.GetComponent<SpriteRenderer>().color = toSetColor;

            player.HatRenderer.FrontLayer.color = toSetColor;
            player.HatRenderer.BackLayer.color = toSetColor;
            player.HatRenderer.color = toSetColor;
            player.MyPhysics.Skin.layer.color = toSetColor;
            player.nameText.color = toSetColor;
        }

        public void Telportation(Vector2 position, PlayerControl player) {
            player.NetTransform.RpcSnapTo(position);
        }

        public static void KillPlayerArea(Vector2 psotion, PlayerControl murder, float size) {
            foreach (var player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == murder.PlayerId)
                    continue;

                float distance = Vector2.Distance(psotion, Position(player));

                if (distance < size) {
                    murder.MurderPlayer(player);
                }
            }
        }

        public static void KillSelf(PlayerControl player) {
            player.MurderPlayer(FromPlayerId(player.PlayerId));
        }

        public static void KillEveryone(PlayerControl murder) {
            foreach (var player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == murder.PlayerId)
                    continue;

                murder.MurderPlayer(player);
            }
        }

        public static bool AmHost() {
            return AmongUsClient.Instance.AmHost;
        }

        public static PlayerControl GetClosestPlayer(PlayerControl PlayerReference) {
            double distance = double.MaxValue;
            PlayerControl result = null;

            foreach (var player in PlayerControl.AllPlayerControls) {
                float distanceBeetween = Vector2.Distance(player.transform.position, PlayerReference.transform.position);
                if (player.Data.IsDead || player.PlayerId == PlayerReference.PlayerId || distance < distanceBeetween)
                    continue;

                distance = distanceBeetween;
                result = player;
            }

            return result;
        }
    }
}
