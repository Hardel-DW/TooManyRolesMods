using HardelAPI.CustomOptions;
using HardelAPI.CustomRoles;
using HardelAPI.CustomRoles.Abilities;
using HardelAPI.CustomRoles.Abilities.Kill;
using HardelAPI.Enumerations;
using System;
using System.Collections.Generic;
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

        public override List<Ability> Abilities { get; set; } = new List<Ability>() {
            new KillAbility() {
                CanKill = VisibleBy.Everyone,
                KillCooldown = SheriffKillCooldown.GetValue(),
                LastKilled = DateTime.UtcNow.AddSeconds(-10.0),
            }
        };

        public Sheriff() : base() {
            GameOptionFormat();
            RoleActive = true;
            Side = PlayerSide.Crewmate;
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

        public override void OnGameStarted() {
            GetAbility<KillAbility>().LastKilled = DateTime.UtcNow.AddSeconds(-10.0);
        }

        public override void OnMeetingEnd(MeetingHud instance) {
            GetAbility<KillAbility>().LastKilled = DateTime.UtcNow;
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
