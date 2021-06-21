using Harion.ColorDesigner;
using Harion.Utility.Helper;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Systems.Spy {

    [HarmonyPatch]
    class AdminPanel {
        static Dictionary<SystemTypes, List<Color>> players = new Dictionary<SystemTypes, List<Color>>();

        [HarmonyPatch(typeof(MapCountOverlay), nameof(MapCountOverlay.Update))]
        class MapCountOverlayPatch {
            static bool Prefix(MapCountOverlay __instance) {
                __instance.timer += Time.deltaTime;
                if (__instance.timer < 0.1f)
                    return false;

                __instance.timer = 0f;
                players = new Dictionary<SystemTypes, List<Color>>();
                bool commsActive = false;
                foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks)
                    if (task.TaskType == TaskTypes.FixComms)
                        commsActive = true;

                if (!__instance.isSab && commsActive) {
                    __instance.isSab = true;
                    __instance.BackgroundColor.SetColor(Palette.DisabledGrey);
                    __instance.SabotageText.gameObject.SetActive(true);
                    return false;
                }

                if (__instance.isSab && !commsActive) {
                    __instance.isSab = false;
                    __instance.BackgroundColor.SetColor(Color.green);
                    __instance.SabotageText.gameObject.SetActive(false);
                }

                for (int i = 0; i < __instance.CountAreas.Length; i++) {
                    CounterArea counterArea = __instance.CountAreas[i];
                    List<Color> roomColors = new List<Color>();
                    players.Add(counterArea.RoomType, roomColors);

                    if (!commsActive) {
                        PlainShipRoom plainShipRoom = ShipStatus.Instance.FastRooms[counterArea.RoomType];

                        if (plainShipRoom != null && plainShipRoom.roomArea) {
                            int num = plainShipRoom.roomArea.OverlapCollider(__instance.filter, __instance.buffer);
                            int num2 = num;
                            for (int j = 0; j < num; j++) {
                                Collider2D collider2D = __instance.buffer[j];
                                if (!(collider2D.tag == "DeadBody")) {
                                    PlayerControl component = collider2D.GetComponent<PlayerControl>();
                                    if (!component || component.Data == null || component.Data.Disconnected || component.Data.IsDead) {
                                        num2--;
                                    } else if (component?.myRend?.material != null) {
                                        Color color = component.myRend.material.GetColor("_BodyColor");
                                        if (Roles.Spy.SpySeeApproxitiveColor.GetValue()) {
                                            var id = Mathf.Max(0, Palette.PlayerColors.IndexOf(color));
                                            color = ColorCreator.lighterColors.Contains(id) ? Palette.PlayerColors[7] : Palette.PlayerColors[6];
                                        }
                                        roomColors.Add(color);
                                    }
                                } else {
                                    DeadBody component = collider2D.GetComponent<DeadBody>();
                                    if (component) {
                                        GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);
                                        if (playerInfo != null) {
                                            var color = Palette.PlayerColors[playerInfo.ColorId];
                                            if (Roles.Spy.SpySeeApproxitiveColor.GetValue())
                                                color = ColorCreator.lighterColors.Contains(playerInfo.ColorId) ? Palette.PlayerColors[7] : Palette.PlayerColors[6];
                                            roomColors.Add(color);
                                        }
                                    }
                                }
                            }
                            counterArea.UpdateCount(num2);
                        } else {
                            Debug.LogWarning("Couldn't find counter for:" + counterArea.RoomType);
                        }
                    } else {
                        counterArea.UpdateCount(0);
                    }
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(CounterArea), nameof(CounterArea.UpdateCount))]
        class CounterAreaUpdateCountPatch {
            private static Sprite defaultIcon;

            static void Postfix(CounterArea __instance) {
                if (players.ContainsKey(__instance.RoomType)) {
                    List<Color> colors = players[__instance.RoomType];

                    for (int i = 0; i < __instance.myIcons.Count; i++) {
                        PoolableBehavior icon = __instance.myIcons[i];
                        SpriteRenderer renderer = icon.GetComponent<SpriteRenderer>();

                        if (renderer != null) {
                            if (defaultIcon == null)
                                defaultIcon = renderer.sprite;
                            if (Button.Instance.IsEffectActive && colors.Count > i) {
                                renderer.sprite = SpriteHelper.HerePoint.sprite;
                                PlayerControl.SetPlayerMaterialColors(colors[i], renderer);
                            } else {
                                renderer.sprite = defaultIcon;
                                PlayerControl.SetPlayerMaterialColors(5, renderer);
                            }
                        }
                    }
                }
            }
        }
    }
}
