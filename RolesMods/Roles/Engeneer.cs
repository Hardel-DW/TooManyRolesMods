using Essentials.Options;
using HardelAPI.CustomRoles;
using HardelAPI.CustomRoles.Abilities;
using HardelAPI.CustomRoles.Abilities.UsableVent;
using HardelAPI.Enumerations;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Engineer))]
    public class Engineer : CustomRole<Engineer> {
        // Color: #FF930FFF
        public static CustomOptionHeader EngineerHeader = CustomOptionHeader.AddHeader("<color=#FF930FFF>Engineer Options :</color>");
        public static CustomNumberOption EngineerPercent = CustomOption.AddNumber("Engineer Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberEngineer = CustomOption.AddNumber("Number Engineer", 1f, 1f, 10f, 1f);
        public static CustomNumberOption EngineerCooldown = CustomOption.AddNumber("Fix Cooldown", 30f, 10f, 120f, 5f);
        public static CustomNumberOption MaxUseEngineer = CustomOption.AddNumber("Max use", 1f, 1f, 10f, 1f);

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
            Systems.Engineer.Button.button.MaxTimer = EngineerCooldown.GetValue();
            Systems.Engineer.Button.UseNumber = (int) MaxUseEngineer.GetValue();
        }

        private void GameOptionFormat() {
            EngineerHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            EngineerPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberEngineer.ValueStringFormat = (option, value) => $"{value} players";
            MaxUseEngineer.ValueStringFormat = (option, value) => $"{value} time";
        }
    }
}
