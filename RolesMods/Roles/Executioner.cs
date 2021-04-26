using Essentials.Options;
using HardelAPI.CustomRoles;
using HardelAPI.Enumerations;
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
            TasksDescription = "<color=#633a37ff>Executioner: Vote your target for win</color>";
            OutroDescription = "Executioner win";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) ExecutionerPercent.GetValue();
            NumberPlayers = (int) NumberExecutioner.GetValue();
            Target = null;
        }

        public override void OnGameStarted() {
            TargetIsDead = false;
            if (HasRole(PlayerControl.LocalPlayer)) {
                List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().Where(player => !(player.PlayerId == PlayerControl.LocalPlayer.PlayerId)).ToList();
                System.Random random = new System.Random();
                Target = players[random.Next(players.Count)];
            }
        }

        public override void OnUpdate(PlayerControl Player) {
            if (Target == null || !HasRole(PlayerControl.LocalPlayer))
                return;

            if (Player.PlayerId != PlayerControl.LocalPlayer.PlayerId)
                return;

            if (Target.Data.IsDead && !TargetIsDead) {
                TargetIsDead = true;
                RemoveImportantTasks(Player);
                RemovePlayer(Player.PlayerId);
            }

            if (PlayerControl.LocalPlayer.Data.IsDead) {
                RemoveImportantTasks(Player);
                RemovePlayer(Player.PlayerId);
            }
        }

        public override void OnExiledPlayer(PlayerControl PlayerExiled) {
            if (HasRole(PlayerControl.LocalPlayer) && Target != null && !PlayerControl.LocalPlayer.Data.IsDead)
                if (PlayerExiled.PlayerId == Target.PlayerId)
                    ForceEndGame(new List<PlayerControl>() { PlayerControl.LocalPlayer });
        }

        private void GameOptionFormat() {
            ExecutionerHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            ExecutionerPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberExecutioner.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
