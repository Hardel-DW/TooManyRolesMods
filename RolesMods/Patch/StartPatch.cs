using HarmonyLib;
using UnityEngine;

namespace RolesMods.Patch {
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    class GameEndedPatch {
        public static void Postfix(ShipStatus __instance) {
            GlobalVariable.isGameStarted = true;
            GlobalVariable.ispsychicActivated = false;
            GlobalVariable.texts.Clear();
            GlobalVariable.herePoints.Clear();

            Systems.TimeMaster.Time.ClearGameHistory();
            Systems.Investigator.FootPrint.allFootprint.Clear();
            GlobalVariable.buttonTime.MaxTimer = RolesMods.TimeMasterCooldown.GetValue();
            GlobalVariable.buttonTime.EffectDuration = RolesMods.TimeMasterDuration.GetValue() / 2;
            Systems.TimeMaster.Time.recordTime = RolesMods.TimeMasterDuration.GetValue();

            GlobalVariable.buttonPsychic.MaxTimer = RolesMods.PsychicCooldown.GetValue();
            GlobalVariable.buttonPsychic.EffectDuration = RolesMods.PsychicDuration.GetValue();

            // Create Overaly for Psychic
            GlobalVariable.psychicOverlay = new GameObject { layer = 5, name = "Overlay" };
            var renderer = GlobalVariable.psychicOverlay.AddComponent<SpriteRenderer>();
            renderer.sprite = Utility.HelperSprite.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Overlay.png", 102);

            GlobalVariable.psychicOverlay.transform.SetParent(Camera.main.transform);
            GlobalVariable.psychicOverlay.transform.localPosition = new Vector3(0f, 0f, 0f);
            GlobalVariable.psychicOverlay.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            GlobalVariable.psychicOverlay.SetActive(false);
        }
    }
}