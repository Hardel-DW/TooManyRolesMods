using HarmonyLib;
using UnityEngine;
using System.Linq;
using UnityEngine.Experimental.Rendering;

namespace RolesMods.Systems.SecurityGuard {

    [HarmonyPatch]
    class SurveillanceMinigamePatch {
        private static int page = 0;
        private static float timer = 0f;

        [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.Begin))]
        class SurveillanceMinigameBeginPatch {
            public static void Postfix(SurveillanceMinigame __instance) {
                // Add securityGuard cameras
                page = 0;
                timer = 0;
                if (ShipStatus.Instance.AllCameras.Length > 4 && __instance.FilteredRooms.Length > 0) {
                    __instance.textures = __instance.textures.ToList().Concat(new RenderTexture[ShipStatus.Instance.AllCameras.Length - 4]).ToArray();
                    for (int i = 4; i < ShipStatus.Instance.AllCameras.Length; i++) {
                        SurvCamera surv = ShipStatus.Instance.AllCameras[i];
                        Camera camera = UnityEngine.Object.Instantiate<Camera>(__instance.CameraPrefab);
                        camera.transform.SetParent(__instance.transform);
                        camera.transform.position = new Vector3(surv.transform.position.x, surv.transform.position.y, 8f);
                        camera.orthographicSize = 2.35f;
                        RenderTexture temporary = RenderTexture.GetTemporary(256, 256, 16, GraphicsFormat.None);
                        __instance.textures[i] = temporary;
                        camera.targetTexture = temporary;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.Update))]
        class SurveillanceMinigameUpdatePatch {

            public static bool Prefix(SurveillanceMinigame __instance) {
                // Update normal and securityGuard cameras
                timer += Time.deltaTime;
                int numberOfPages = Mathf.CeilToInt(ShipStatus.Instance.AllCameras.Length / 4f);

                bool update = false;

                if (timer > 3f || Input.GetKeyDown(KeyCode.RightArrow)) {
                    update = true;
                    timer = 0f;
                    page = (page + 1) % numberOfPages;
                } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    page = (page + numberOfPages - 1) % numberOfPages;
                    update = true;
                    timer = 0f;
                }

                if ((__instance.isStatic || update) && !PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer)) {
                    __instance.isStatic = false;
                    for (int i = 0; i < __instance.ViewPorts.Length; i++) {
                        __instance.ViewPorts[i].sharedMaterial = __instance.DefaultMaterial;
                        __instance.SabText[i].gameObject.SetActive(false);
                        if (page * 4 + i < __instance.textures.Length)
                            __instance.ViewPorts[i].material.SetTexture("_MainTex", __instance.textures[page * 4 + i]);
                        else
                            __instance.ViewPorts[i].sharedMaterial = __instance.StaticMaterial;
                    }
                } else if (!__instance.isStatic && PlayerTask.PlayerHasTaskOfType<HudOverrideTask>(PlayerControl.LocalPlayer)) {
                    __instance.isStatic = true;
                    for (int j = 0; j < __instance.ViewPorts.Length; j++) {
                        __instance.ViewPorts[j].sharedMaterial = __instance.StaticMaterial;
                        __instance.SabText[j].gameObject.SetActive(true);
                    }
                }
                return false;
            }
        }
    }
}
