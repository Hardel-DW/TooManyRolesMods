using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.Enumerations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Lover))]
    public class Lover : CustomRole<Lover> {
        // Color: #f5b2e4ff
        public static CustomNumberOption LoverPercent = CustomOption.AddNumber("Lover", "<color=#f5b2e4ff>Lover Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.CrewmateHolder);
        public static CustomToggleOption LoverDies = CustomOption.AddToggle("Lover die with both", false, LoverPercent);
        private static PlayerControl Target;
        private bool TargetIsDead = false;

        public Lover() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            RoleActive = true;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            NumberPlayers = 2;
            Color = new Color(0.960f, 0.698f, 0.894f, 1f);
            Name = "Lover";
            IntroDescription = $"You are in <color=#f570dcff>Love</color> with <color=#f5b2e4ff>{Target?.name}</color>";
            TasksDescription = $"Lover: You are in <color=#f5b2e4ff>Love</color> with <color=#f5b2e4ff>{Target?.name}</color>";
            OutroDescription = "Lovers win";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) LoverPercent.GetValue();
        }

        public override bool OnRoleSelectedInInfected(List<PlayerControl> playerHasNoRole) {
            return playerHasNoRole.Count < 2;
        }


        public override void OnIntroCutScene() {
            Target = null;
            TargetIsDead = false;

            if (HasRole(PlayerControl.LocalPlayer)) {
                PlayerControl both = AllPlayers.FirstOrDefault(b => b.PlayerId != PlayerControl.LocalPlayer.PlayerId);
                if (both != null) {
                    Target = both;
                }
            }
        }

        public override void OnUpdate(PlayerControl Player) {
            if (Player.Is<Lover>() && Player.Data.IsImpostor && !specificNameInformation.Keys.Contains(Player))
                specificNameInformation.Add(Player, (Color, "Lover Impostor"));

            if (Target != null && HasRole(PlayerControl.LocalPlayer)) {
                if (Target.Data.IsDead && !TargetIsDead) {
                    TargetIsDead = true;
                    if (LoverDies.GetValue())
                        PlayerControl.LocalPlayer.RpcMurderPlayer(PlayerControl.LocalPlayer);
                }
            }

            if (AmongUsClient.Instance.AmHost) {
                if (AllPlayers.Count == 2 && PlayerControl.AllPlayerControls.ToArray().Where(p => p.Data.IsDead).ToList().Count == 3) {
                    ForceEndGame(AllPlayers);
                }
            }
        }

        private void GameOptionFormat() {
            LoverPercent.ValueStringFormat = (option, value) => $"{value}%";
            LoverPercent.ShowChildrenConidtion = () => LoverPercent.GetValue() > 0;

        }
    }
}
