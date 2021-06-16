using HardelAPI.CustomOptions;
using HardelAPI.CustomRoles;
using HardelAPI.Enumerations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Seer))]
    public class Seer : CustomRole<Seer> {
        // Color: #dbc0a4ff
        public static CustomOptionHeader SeerHeader = CustomOptionHeader.AddHeader("<color=#dbc0a4ff>Seer Options :</color>");
        public static CustomNumberOption SeerPercent = CustomOption.AddNumber("Seer Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberSeer = CustomOption.AddNumber("Number Seer", 1f, 1f, 10f, 1f);
        public static CustomNumberOption SeerCooldown = CustomOption.AddNumber("Seer Cooldown", 30f, 10f, 120f, 5f);
        public static CustomNumberOption SeerUseNumber = CustomOption.AddNumber("Number of uses", 1f, 1f, 10f, 1f);
        public static CustomNumberOption SeerPercentSeeRole = CustomOption.AddNumber("Seer Percent for discover role", 50f, 0f, 100f, 5f);
        public static CustomToggleOption ShowGoodOrBad = CustomOption.AddToggle("Revaal roles by Good or Bad", true);

        public Seer() : base() {
            GameOptionFormat();
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.858f, 0.752f, 0.643f, 1f);
            Name = "Seer";
            IntroDescription = "You see everything";
            TasksDescription = "<color=#dbc0a4ff>Seer: Inspect player and try to set their role</color>";
        }

        public override void OnInfectedStart() {
            Systems.Seer.Button.Instance.MaxTimer = SeerCooldown.GetValue();
            Systems.Seer.Button.Instance.UseNumber = (int) SeerUseNumber.GetValue();
            PercentApparition = (int) SeerPercent.GetValue();
            NumberPlayers = (int) NumberSeer.GetValue();
        }

        public override void OnGameStarted() {
            List<PlayerControl> allPlayerTargatable = PlayerControl.AllPlayerControls.ToArray().ToList();
            PlayerControl playerToRemove = allPlayerTargatable.FirstOrDefault(p => p.PlayerId == PlayerControl.LocalPlayer.PlayerId);
            allPlayerTargatable.Remove(playerToRemove);

            Systems.Seer.Button.Instance.AllPlayersTargetable = allPlayerTargatable;
        }

        private void GameOptionFormat() {
            SeerHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            SeerPercent.ValueStringFormat = (option, value) => $"{value}%";
            SeerPercentSeeRole.ValueStringFormat = (option, value) => $"{value}%";

            NumberSeer.ValueStringFormat = (option, value) => $"{value} players";
        }
    }
}
