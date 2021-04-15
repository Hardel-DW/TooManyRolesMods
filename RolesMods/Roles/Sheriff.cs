using Essentials.Options;
using RolesMods.Utility.CustomRoles;
using RolesMods.Utility.Enumerations;
using System;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Sheriff))]
    public class Sheriff : CustomRole<Sheriff> {
        // Color: D1B300FF
        public static CustomOptionHeader SherifHeader = CustomOptionHeader.AddHeader("<color=#D1B300FF>Sherif Options :</color>");
        public static CustomNumberOption SherifPercent = CustomOption.AddNumber("Sherif Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberSherif = CustomOption.AddNumber("Number Sherif", 1f, 1f, 10f, 1f);
        public static CustomNumberOption SheriffKillCooldown = CustomOption.AddNumber("Sherif Cooldown", 15f, 5f, 90f, 10f);
        public static CustomToggleOption TargetDies = CustomOption.AddToggle("Target dies", false);
        
        public Sheriff() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            RoleActive = true;
            CanKill = PlayerSide.Everyone;
            GiveTasksAt = Moment.StartGame;
            GiveRoleAt = Moment.StartGame;
            Color = new Color(0.819f, 0.701f, 0f, 1f);
            Name = "Sheriff";
            IntroDescription = "You can kill the impostors";
            TasksDescription = "<color=#D1B300FF>Sheriff : You can kill the impostors</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) SherifPercent.GetValue();
            NumberPlayers = (int) NumberSherif.GetValue();
        }

        public override void OnGameStart() {
            LastKilled = DateTime.UtcNow;
            LastKilled = LastKilled.AddSeconds(-8.0);
        }

        public override void OnLocalAttempKill(PlayerControl killer, PlayerControl target) {
            if (target.Data.IsImpostor || Jester.Instance.HasRole(target)) {
                killer.RpcMurderPlayer(target);
            } else {
                if (TargetDies.GetValue())
                    killer.RpcMurderPlayer(target);
                killer.RpcMurderPlayer(PlayerControl.LocalPlayer);
            }
        }

        private void GameOptionFormat() {
            SherifHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            SherifPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberSherif.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
