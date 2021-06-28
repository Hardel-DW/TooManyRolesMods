using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.Enumerations;
using Harion.Utility.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Executioner))]
    public class Executioner : CustomRole<Executioner> {
        // Color: #633a37ff
        public static CustomNumberOption ExecutionerPercent = CustomOption.AddNumber("Executioner", "<color=#633A37FF>Executioner Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.NeutralHolder);
        public static CustomNumberOption NumberExecutioner = CustomOption.AddNumber("Number Executioner", 1f, 1f, 10f, 1f, ExecutionerPercent);
        private static PlayerControl Target;

        public Executioner() : base() {
            GameOptionFormat();
            RoleActive = true;
            Side = PlayerSide.Crewmate;
            RoleType = RoleType.Neutral;
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
        }

        public override void OnInfectedEnd() {
            Target = null;

            if (HasRole(PlayerControl.LocalPlayer)) {
                List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().ToList();
                players.RemoveAll(player => Executioner.Instance.HasRole(player));
                players.RemoveAll(player => Jester.Instance.HasRole(player));
                players.RemoveAll(player => player.Data.IsImpostor);
                players.RemoveAll(player => player.PlayerId == PlayerControl.LocalPlayer.PlayerId);

                if (players.Count > 0) {
                    Target = players[new System.Random().Next(players.Count)];
                    specificNameInformation.Add(Target, (new Color(204f / 255f, 53f / 255f, 53f / 255f, 1f), "Target !"));
                }
            }
        }

        public override void AddImportantTasks(PlayerControl Player) {
            string tasks = TasksDescription;
            if (Target != null)
                tasks = $"<color=#a36e6aff>Executioner: Exile <color=#ffffffff>{Target.name}</color> for win</color>";

            ImportantTextTask ImportantTasks = new GameObject("RolesTasks").AddComponent<ImportantTextTask>();
            ImportantTasks.transform.SetParent(Player.transform, false);
            ImportantTasks.Text = tasks;
            Player.myTasks.Insert(0, ImportantTasks);
        }

        public override void OnMeetingStart(MeetingHud instance) {
            if (Target == null || !HasRole(PlayerControl.LocalPlayer))
                return;

            if ((Target.Data.IsDead) || PlayerControl.LocalPlayer.Data.IsDead)
                ResetRole();
        }

        public override void OnExiledPlayer(PlayerControl PlayerExiled) {
            if (Target != null && PlayerControl.LocalPlayer != null)
                if (HasRole(PlayerControl.LocalPlayer) && !PlayerControl.LocalPlayer.Data.IsDead)
                    if (PlayerExiled.PlayerId == Target.PlayerId)
                        RpcForceEndGame(new List<PlayerControl>() { PlayerControl.LocalPlayer });
        }

        private void GameOptionFormat() {
            ExecutionerPercent.ValueStringFormat = (option, value) => $"{value}%";
            ExecutionerPercent.ShowChildrenConidtion = () => ExecutionerPercent.GetValue() > 0;

            NumberExecutioner.ValueStringFormat = (option, value) => $"{value} players";
        }

        private void ResetRole() {
            RemoveImportantTasks(PlayerControl.LocalPlayer);
            AllPlayers.RemovePlayer(PlayerControl.LocalPlayer);

            foreach (var player in specificNameInformation.Keys.ToArray().ToList())
                if (player.PlayerId == Target.PlayerId) 
                    specificNameInformation.Remove(player);

            Target = null;
        }
    }
}