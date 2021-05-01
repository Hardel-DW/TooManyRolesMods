using Essentials.Options;
using HardelAPI.CustomRoles;
using HardelAPI.CustomRoles.Abilities;
using HardelAPI.CustomRoles.Abilities.Kill;
using HardelAPI.CustomRoles.Abilities.UsableVent;
using HardelAPI.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(SerialKiller))]
    public class SerialKiller : CustomRole<SerialKiller> {
        // Color: #1aeef9ff
        public static CustomOptionHeader SerialKillerHeader = CustomOptionHeader.AddHeader("<color=#1aeef9ff>Serial Killer and Collaborator Options :</color>");
        public static CustomNumberOption SerialKillerPercent = CustomOption.AddNumber("Serial Killer Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption KillCooldown = CustomOption.AddNumber("Kill Cooldown", 0f, 0f, 100f, 5f);

        public static CustomToggleOption CanHasCollabo = CustomOption.AddToggle("Can Create collaborator", true);
        public static CustomNumberOption CreateCollabo = CustomOption.AddNumber("Cooldown button for make collaborator", 0f, 0f, 100f, 5f);
        public static CustomToggleOption SerialKillerCanVent = CustomOption.AddToggle("Can Vent", true);

        public override List<Ability> Abilities { get; set; } = new List<Ability>() {
            new KillAbility() {
                CanKill = PlayerSide.Nobody,
                KillCooldown = KillCooldown.GetValue()
            },
            new VentAbility() {
                CanVent = false
            }
        };

        public SerialKiller() : base() {
            GameOptionFormat();
            TasksDescription = "<color=#1aeef9ff>Serial Killer: Kill everyone to win</color>";
            IntroDescription = "Kill Everyone !";
            OutroDescription = "Serial Killer win";
            Name = "Serial Killer";
            HasTask = false;
            Side = PlayerSide.Crewmate;
            GiveTasksAt = Moment.StartGame;
            RoleActive = true;
            Color = new Color(0.101f, 0.933f, 0.976f, 1f);
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) SerialKillerPercent.GetValue();
        }

        public override void OnMeetingEnd(MeetingHud instance) {
            GetAbility<KillAbility>().LastKilled = DateTime.UtcNow;
        }

        public override void OnGameStarted() {
            List<PlayerControl> allPlayers = PlayerControl.AllPlayerControls.ToArray().Where(p => !HasRole(p)).ToList();

            KillAbility ability = GetAbility<KillAbility>();
            if (ability != null) {
                ability.KillCooldown = KillCooldown.GetValue();
                ability.LastKilled = DateTime.UtcNow.AddSeconds(-10.0);
                ability.WhiteListKill = allPlayers;
            }

            GetAbility<VentAbility>().CanVent = SerialKillerCanVent.GetValue();
        }

        public override bool WinCriteria() {
            return (AllPlayers.Count + Collaborator.Instance.AllPlayers.Count > 0 && AllPlayers != null);
        }

        public override bool AddEndCriteria() {
            List<PlayerControl> SerialKillers = PlayerControl.AllPlayerControls.ToArray().Where(p => (HasRole(p) || Collaborator.Instance.HasRole(p)) && !p.Data.IsDead).ToList();
            List<PlayerControl> allPlayers = PlayerControl.AllPlayerControls.ToArray().Where(p => !p.Data.IsDead).ToList();
            List<PlayerControl> impostors = PlayerControl.AllPlayerControls.ToArray().Where(p => p.Data.IsImpostor).ToList();

            return (SerialKillers.Count + Collaborator.Instance.AllPlayers.Count >= allPlayers.Count && impostors.Count == 0);
        }

        public override void OnRoleWin() {
            WinPlayer = AllPlayers;
            WinPlayer.AddRange(Collaborator.Instance.AllPlayers);
        }

        private void GameOptionFormat() {
            SerialKillerHeader.HudStringFormat = (option, name, value) => $"\n{name}";
            SerialKillerPercent.ValueStringFormat = (option, value) => $"{value}%";
        }
    }
}
