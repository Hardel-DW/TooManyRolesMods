using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class RemoveTasksPatch {
        public static void Postfix(HudManager __instance) {
            if (PlayerControl.LocalPlayer != null) {
                foreach (var Role in RoleManager.AllRoles) {
                    if (Role.HasTask || Role.TaskAreRemove)
                        continue;

                    Role.TaskAreRemove = true;
                    foreach (var player in Role.AllPlayers) {
                        if (!(AmongUsClient.Instance.GameMode == GameModes.FreePlay)) {
                            var toRemove = new List<PlayerTask>();
                            foreach (PlayerTask task in player.myTasks)
                                if (task.TaskType != TaskTypes.StartReactor && task.TaskType != TaskTypes.FixComms && task.TaskType != TaskTypes.FixLights && task.TaskType != TaskTypes.ResetReactor && task.TaskType != TaskTypes.ResetSeismic && task.TaskType != TaskTypes.RestoreOxy && task.name != "DebileTasks" && task.name != "_Player")
                                    toRemove.Add(task);

                            foreach (PlayerTask task in toRemove)
                                player.RemoveTask(task);
                        }
                    }
                }
            }
        }
    }
}
