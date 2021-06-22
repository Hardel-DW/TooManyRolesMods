using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.CustomRoles.Abilities;
using Harion.CustomRoles.Abilities.Kill;
using Harion.CustomRoles.Abilities.UsableVent;
using Harion.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(SerialKiller))]
    public class SerialKiller : CustomRole<SerialKiller> {
        // Color: #1aeef9ff
        public static CustomNumberOption SerialKillerPercent = CustomOption.AddNumber("SerialKiller", "<color=#1aeef9ff>Serial Killer Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.NeutralHolder);
        public static CustomNumberOption KillCooldown = CustomOption.AddNumber("Kill Cooldown", 0f, 0f, 100f, 5f, SerialKillerPercent);
        public static CustomToggleOption CanHasCollabo = CustomOption.AddToggle("Can Create collaborator", true, SerialKillerPercent);
        public static CustomNumberOption CreateCollabo = CustomOption.AddNumber("Cooldown button for make collaborator", 0f, 0f, 100f, 5f, SerialKillerPercent);
        public static CustomToggleOption SerialKillerCanVent = CustomOption.AddToggle("Can Vent", true, SerialKillerPercent);

        public override List<Ability> Abilities { get; set; } = new List<Ability>() {
            new KillAbility() {
                CanKill = VisibleBy.Nobody,
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
            SerialKillerPercent.ValueStringFormat = (option, value) => $"{value}%";
            SerialKillerPercent.ShowChildrenConidtion = () => SerialKillerPercent.GetValue() > 0;
        }
    }
}
