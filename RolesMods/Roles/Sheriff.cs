using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.CustomRoles.Abilities;
using Harion.CustomRoles.Abilities.Kill;
using Harion.Enumerations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Sheriff))]
    public class Sheriff : CustomRole<Sheriff> {
        // Color: D1B300FF
        public static CustomNumberOption SherifPercent = CustomOption.AddNumber("<color=#D1B300FF>Sherif Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.CrewmateHolder);
        public static CustomNumberOption NumberSherif = CustomOption.AddNumber("Number Sherif", 1f, 1f, 10f, 1f, SherifPercent);
        public static CustomNumberOption SheriffKillCooldown = CustomOption.AddNumber("Sherif Cooldown", 15f, 5f, 90f, 10f, SherifPercent);
        public static CustomToggleOption TargetDies = CustomOption.AddToggle("Target dies", false, SherifPercent);

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
            SherifPercent.ValueStringFormat = (option, value) => $"{value}%";
            SherifPercent.ShowChildrenConidtion = () => SherifPercent.GetValue() > 0;

            NumberSherif.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
