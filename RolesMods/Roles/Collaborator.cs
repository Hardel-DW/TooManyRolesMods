using Harion.CustomRoles;
using Harion.CustomRoles.Abilities;
using Harion.CustomRoles.Abilities.UsableVent;
using Harion.Enumerations;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Collaborator))]
    public class Collaborator : CustomRole<Collaborator> {
        // Color: #1aeef9ff
        public override List<Ability> Abilities { get; set; } = new List<Ability>() {
            new VentAbility() {
                CanVent = false
            }
        };

        public Collaborator() : base() {
            TasksDescription = "<color=#1aeef9ff>Collaborator: Help the serial killer to win</color>";
            Name = "Collaborator";
            HasTask = false;
            RoleType = RoleType.Undefined;
            Side = PlayerSide.Crewmate;
            GiveTasksAt = Moment.Never;
            ShowIntroCutScene = false;
            NumberPlayers = 0;
            RoleActive = true;
            Color = new Color(0.101f, 0.933f, 0.976f, 1f);
        }

        public override void OnGameStarted() {
            GetAbility<VentAbility>().CanVent = SerialKiller.SerialKillerCanVent.GetValue();
        }
    }
}
