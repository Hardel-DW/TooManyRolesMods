using Essentials.Options;
using HardelAPI.CustomRoles;
using HardelAPI.Enumerations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Lover))]
    public class Lover : CustomRole<Lover> {
        // Color: ##f570dcff
        public static CustomOptionHeader LoverHeader = CustomOptionHeader.AddHeader("<color=#f570dcff>Lover Options :</color>");
        public static CustomNumberOption LoverPercent = CustomOption.AddNumber("Lover Apparition", 0f, 0f, 100f, 5f);
        public static CustomToggleOption LoverDies = CustomOption.AddToggle("Lover die with both", false);
        private static PlayerControl Target;
        private bool TargetIsDead = false;

        public Lover() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            RoleActive = true;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            NumberPlayers = 2;
            Color = new Color(0.388f, 0.227f, 0.215f, 1f);
            Name = "Lover";
            IntroDescription = $"You are in <color=#f570dcff>Love</color> with <color=#f570dcff>{Target?.name}</color>";
            TasksDescription = $"Lover: You are in <color=#f570dcff>Love</color> with <color=#f570dcff>{Target?.name}</color>";
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
                if (both != null)
                    Target = both;
            }
        }

        public override void OnUpdate(PlayerControl Player) {
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
            LoverHeader.HudStringFormat = (option, name, value) => $"\n{name}";
            LoverPercent.ValueStringFormat = (option, value) => $"{value}%";
        }
    }
}
