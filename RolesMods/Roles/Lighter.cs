﻿using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.CustomRoles.Abilities;
using Harion.CustomRoles.Abilities.Light;
using Harion.Enumerations;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Roles {
    [RegisterInCustomRoles(typeof(Lighter))]
    public class Lighter : CustomRole<Lighter> {
        // Color: BA5B13FF
        public static CustomNumberOption LighterPercent = CustomOption.AddNumber("Lighter", "<color=#BA5B13FF>Lighter Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.CrewmateHolder);
        public static CustomNumberOption NumberLighter = CustomOption.AddNumber("Number Lighter", 1f, 1f, 10f, 1f, LighterPercent);
        public static CustomNumberOption LighterMultiplier = CustomOption.AddNumber("Lighter Multiplier", 1.5f, 0f, 5f, 0.05f, LighterPercent);
        public static CustomToggleOption LighterSabotageVision = CustomOption.AddToggle("Lighter sees during electrical sabotage", true, LighterPercent);

        public override List<Ability> Abilities { get; set; } = new List<Ability>() {
            new LightAbility() {
                CanSeeDuringLightSabotage = true,
                LightValueMultiplier = NumberLighter.GetValue()
            }
        };

        public Lighter() : base() {
            GameOptionFormat();
            TasksDescription = "<color=#BA5B13FF>Lighter: Your vision is better than crewmate</color>";
            IntroDescription = "Your vision is better than crewmate";
            Name = "Lighter";
            Side = PlayerSide.Crewmate;
            GiveTasksAt = Moment.StartGame;
            RoleActive = true;
            Color = new Color(0.729f, 0.356f, 0.074f, 1f);
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) LighterPercent.GetValue();
            NumberPlayers = (int) NumberLighter.GetValue();
        }

        public override void OnGameStarted() {
            LightAbility ability = GetAbility<LightAbility>();
            if (ability != null) {
                ability.LightValueMultiplier = LighterMultiplier.GetValue();
                ability.CanSeeDuringLightSabotage = LighterSabotageVision.GetValue();
            }
        }

        private void GameOptionFormat() {
            LighterPercent.ValueStringFormat = (option, value) => $"{value}%";
            LighterPercent.ShowChildrenConidtion = () => LighterPercent.GetValue() > 0;

            NumberLighter.ValueStringFormat = (option, value) => $"{value} players";
            LighterMultiplier.ValueStringFormat = (option, value) => $"x {value}";
        }
    }
}
