using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.CustomRoles.Abilities;
using Harion.CustomRoles.Abilities.UsableVent;
using Harion.Enumerations;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Engineer))]
    public class Engineer : CustomRole<Engineer> {
        // Color: #FF930FFF
        public static CustomNumberOption EngineerPercent = CustomOption.AddNumber("Engineer", "<color=#FF930FFF>Engineer Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.CrewmateHolder);
        public static CustomNumberOption NumberEngineer = CustomOption.AddNumber("Number Engineer", 1f, 1f, 10f, 1f, EngineerPercent);
        public static CustomNumberOption MaxUseEngineer = CustomOption.AddNumber("Max use", 1f, 1f, 10f, 1f, EngineerPercent);

        public override List<Ability> Abilities { get; set; } = new List<Ability>() {
            new VentAbility() { CanVent = true }
        };

        public Engineer() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            RoleActive = true;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            Color = new Color(1f, 0.576f, 0.058f, 1f);
            Name = "Engineer";
            IntroDescription = "Maintain important systems on the ship";
            TasksDescription = "<color=#FF930FFF>Engineer: Vent and fix a sabotage from anywhere!</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) EngineerPercent.GetValue();
            NumberPlayers = (int) NumberEngineer.GetValue();
        }

        public override void OnGameStarted() {
            Systems.Engineer.Button.Instance.UseNumber = (int) MaxUseEngineer.GetValue();
        }

        private void GameOptionFormat() {
            EngineerPercent.ValueStringFormat = (option, value) => $"{value}%";
            EngineerPercent.ShowChildrenConidtion = () => EngineerPercent.GetValue() > 0;

            NumberEngineer.ValueStringFormat = (option, value) => $"{value} players";
            MaxUseEngineer.ValueStringFormat = (option, value) => $"{value} time";
        }
    }
}
