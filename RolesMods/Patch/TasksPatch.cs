using HarmonyLib;
using System;
using System.Diagnostics;
using UnityEngine;

namespace RolesMods.Patch {

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    class TasksPatch {
        public static void Postfix(PlayerControl __instance) {
            if (PlayerControl.LocalPlayer != null) {
                if (GlobalVariable.InvestigatorsList != null && HelperRoles.IsInvestigator(PlayerControl.LocalPlayer.PlayerId)) {
                    ImportantTextTask ImportantTasks = new GameObject("InvestigatorTasks").AddComponent<ImportantTextTask>();
                    ImportantTasks.transform.SetParent(__instance.transform, false);
                    ImportantTasks.Text = "[2EADFFFF]You are an Investigator, you can see everyone's footprint.[]";
                    __instance.myTasks.Insert(0, ImportantTasks);
                }

                if (GlobalVariable.TimeMaster != null && HelperRoles.IsTimeMaster(PlayerControl.LocalPlayer.PlayerId)) {
                    ImportantTextTask ImportantTasks = new GameObject("TimeMasterTasks").AddComponent<ImportantTextTask>();
                    ImportantTasks.transform.SetParent(__instance.transform, false);
                    ImportantTasks.Text = "[999999FF]TimeMaster: You can travel the time and revive other.[]";
                    __instance.myTasks.Insert(0, ImportantTasks);
                }

                if (GlobalVariable.LightersList != null && HelperRoles.IsLighter(PlayerControl.LocalPlayer.PlayerId)) {
                    ImportantTextTask ImportantTasks = new GameObject("LighterTasks").AddComponent<ImportantTextTask>();
                    ImportantTasks.transform.SetParent(__instance.transform, false);
                    ImportantTasks.Text = "[BA5B13FF]Lighter: Your vision is better than crewmate[]";
                    __instance.myTasks.Insert(0, ImportantTasks);
                }
            }
        }
    }
}
