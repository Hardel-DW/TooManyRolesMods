using Harion.CustomOptions;
using Harion.ArrowManagement;
using Harion.CustomRoles;
using Harion.Enumerations;
using Harion.Utility.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Harion.Reactor;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Snitch))]
    public class Snitch : CustomRole<Snitch> {
        // Color: #b6bf62ff
        public static CustomNumberOption SnitchPercent = CustomOption.AddNumber("Snitch", "<color=#b6bf62ff>Snitch Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.CrewmateHolder);
        public static CustomNumberOption NumberSnitch = CustomOption.AddNumber("Number Snitch", 1f, 1f, 10f, 1f, SnitchPercent);
        public static CustomNumberOption TaskCount = CustomOption.AddNumber("Task Count Where Impostors See Snitch", 1f, 1f, 10f, 1f, SnitchPercent);
        public static CustomToggleOption ShowRoleAtStartGame = CustomOption.AddToggle("Show Role at start game", false, SnitchPercent);
        private static Dictionary<PlayerControl, ArrowManager> arrowManagers = new Dictionary<PlayerControl, ArrowManager>();

        public Snitch() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            RoleActive = true;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.7f, 0.75f, 0.4f, 1f);
            Name = "Snitch";
            IntroDescription = "Finish your task to find impostor";
            TasksDescription = "<color=#b6bf62ff>Snitch: Finish your task to find impostor</color>";
        }

        public override void OnInfectedStart() {
            if (ShowRoleAtStartGame.GetValue()) {
                GiveRoleAt = Moment.Never;
                ShowIntroCutScene = false;
                VisibleBy = VisibleBy.Nobody;
            } else {
                GiveRoleAt = Moment.StartGame;
                ShowIntroCutScene = true;
                VisibleBy = VisibleBy.Self;
            }

            PercentApparition = (int) SnitchPercent.GetValue();
            NumberPlayers = (int) NumberSnitch.GetValue();
        }

        public override void OnGameStarted() {
            TryDestroyArrow();
        }

        public override void OnGameEnded() {
            TryDestroyArrow();
        }

        public override void OnMeetingStart(MeetingHud instance) {
            if (arrowManagers != null && arrowManagers.Count > 0)
            foreach (var arrow in arrowManagers)
                arrow.Value.Arrow.SetActive(false);
        }

        public override void OnMeetingEnd(MeetingHud instance) {
            if (arrowManagers != null && arrowManagers.Count > 0)
                foreach (var arrow in arrowManagers)
                    arrow.Value.Arrow.SetActive(true);
        }

        public override void OnAllTaskComplete(PlayerControl Player) {
            if (HasRole(Player) && Player.PlayerId == PlayerControl.LocalPlayer.PlayerId) {
                foreach (var impostor in PlayerControl.AllPlayerControls.ToArray().Where(x => x.Data.IsImpostor)) {
                    if (impostor.Data.IsDead || impostor.Data.Disconnected) 
                        continue;
                    
                    arrowManagers.Add(impostor, new ArrowManager(impostor.gameObject, true, 5f));
                }
            }
        }

        public override void OnTaskLeft(PlayerControl Player, int number) {
            if (HasRole(Player)) {
                if (number == TaskCount.GetValue()) {
                    if (Player.PlayerId == PlayerControl.LocalPlayer.PlayerId) {
                        Coroutines.Start(PlayerControlUtils.FlashCoroutine(Color));
                        VisibleBy = VisibleBy.Self;
                        AddImportantTasks(Player);
                    } else if (PlayerControl.LocalPlayer.Data.IsImpostor) {
                        arrowManagers.Add(Player, new ArrowManager(Player.gameObject, true, 5f));
                    }
                }
            }
        }

        public override void OnUpdate(PlayerControl Player) {
            var arrow = arrowManagers.FirstOrDefault(p => p.Key.PlayerId == Player.PlayerId);
            if (arrow.Value != null && arrow.Key != null) {
                if (arrow.Key.Data.IsDead) {
                    if (arrow.Value.Arrow != null) {
                        Object.Destroy(arrow.Value.Arrow);
                        arrowManagers.Remove(arrow.Key);
                    }
                }
            }
        }

        private void TryDestroyArrow() {
            if (arrowManagers != null && arrowManagers.Count > 0)
                foreach (var arrow in arrowManagers)
                    if (arrow.Value.Arrow != null)
                        Object.Destroy(arrow.Value.Arrow);

            arrowManagers = new Dictionary<PlayerControl, ArrowManager>();
        }

        private void GameOptionFormat() {
            SnitchPercent.ValueStringFormat = (option, value) => $"{value}%";
            SnitchPercent.ShowChildrenConidtion = () => SnitchPercent.GetValue() > 0;

            NumberSnitch.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
