using Essentials.Options;
using HardelAPI.CustomRoles;
using HardelAPI.Enumerations;
using HardelAPI.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Executioner))]
    public class Executioner : CustomRole<Executioner> {
        // Color: #633a37ff
        public static CustomOptionHeader ExecutionerHeader = CustomOptionHeader.AddHeader("<color=#633a37ff>Executioner Options :</color>");
        public static CustomNumberOption ExecutionerPercent = CustomOption.AddNumber("Executioner Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberExecutioner = CustomOption.AddNumber("Number Executioner", 1f, 1f, 10f, 1f);
        private static PlayerControl Target;
        private bool TargetIsDead = false;

        public Executioner() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            RoleActive = true;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.388f, 0.227f, 0.215f, 1f);
            Name = "Executioner";
            IntroDescription = "Vote goose out to win !";
            TasksDescription = $"<color=#633a37ff>Executioner: Exile {Target?.name} for win</color>";
            OutroDescription = "Executioner win";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) ExecutionerPercent.GetValue();
            NumberPlayers = (int) NumberExecutioner.GetValue();
            Plugin.Logger.LogInfo("Yop");
        }

        public override void OnInfectedEnd() {
            Target = null;
            TargetIsDead = false;

            if (HasRole(PlayerControl.LocalPlayer)) {
                List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().Where(player => 
                    !(player.PlayerId == PlayerControl.LocalPlayer.PlayerId) &&
                    !(player.Is<Executioner>()) &&
                    !(player.Is<Jester>()) &&
                    !(player.Data.IsImpostor)
                ).ToList();

                Plugin.Logger.LogInfo("---------------------");

                foreach (var player in players) {
                    Plugin.Logger.LogInfo($"Can be a target : {player.name}");
                }
                Target = players[new System.Random().Next(players.Count)];

                Plugin.Logger.LogInfo(Target.name + " is the Target");

                if (Target != null) {
                    RefreshTask($"<color=#633a37ff>Executioner: Exile ${Target.name} for win</color>", PlayerControl.LocalPlayer);
                    Plugin.Logger.LogInfo("Test");
                }
            }
        }

        public override void OnMeetingStart(MeetingHud instance) {
            if (Target == null || !HasRole(PlayerControl.LocalPlayer))
                return;

            if (Target.Data.IsDead && !TargetIsDead) {
                TargetIsDead = true;
                RemoveImportantTasks(PlayerControl.LocalPlayer);
                AllPlayers.RemovePlayer(PlayerControl.LocalPlayer);
            }

            if (PlayerControl.LocalPlayer.Data.IsDead) {
                RemoveImportantTasks(PlayerControl.LocalPlayer);
                AllPlayers.RemovePlayer(PlayerControl.LocalPlayer);
            }
        }

        public override void OnExiledPlayer(PlayerControl PlayerExiled) {
            if (HasRole(PlayerControl.LocalPlayer) && Target != null && !PlayerControl.LocalPlayer.Data.IsDead)
                if (PlayerExiled.PlayerId == Target.PlayerId)
                    RpcForceEndGame(new List<PlayerControl>() { PlayerControl.LocalPlayer });
        }

        private void GameOptionFormat() {
            ExecutionerHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            ExecutionerPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberExecutioner.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}