using Harion.CustomOptions;
using Harion.CustomRoles;
using Harion.CustomRoles.Abilities;
using Harion.CustomRoles.Abilities.UsableVent;
using Harion.Enumerations;
using System.Collections.Generic;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(Miner))]
    public class Miner : CustomRole<Miner> {
        // Color: #FF930FFF
        public static CustomNumberOption MinerPercent = CustomOption.AddNumber("<color=#FF0000FF>Miner Apparition</color>", 0f, 0f, 100f, 5f, RoleModPlugin.ImpostorHolder);
        public static CustomNumberOption NumberMiner = CustomOption.AddNumber("Number Miner", 1f, 1f, 10f, 1f, MinerPercent);
        public static CustomNumberOption MinerCooldown = CustomOption.AddNumber("Miner Cooldown", 30f, 10f, 120f, 5f, MinerPercent);
        public static CustomNumberOption MaxUseMiner = CustomOption.AddNumber("Max use", 1f, 1f, 10f, 1f, MinerPercent);

        public override List<Ability> Abilities { get; set; } = new List<Ability>() {
            new VentAbility() { CanVent = true }
        };

        public Miner() : base() {
            GameOptionFormat();
            Side = PlayerSide.Impostor;
            RoleActive = true;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            Color = Palette.ImpostorRed;
            Name = "Miner";
            IntroDescription = "You can be Invisible";
            TasksDescription = "<color=#FF930FFF>Miner: Use your ability to be invisible</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) MinerPercent.GetValue();
            NumberPlayers = (int) NumberMiner.GetValue();
        }

        public override void OnGameStarted() {
            Systems.Miner.Button.Instance.MaxTimer = MinerCooldown.GetValue();
            Systems.Miner.Button.Instance.UseNumber = (int) MaxUseMiner.GetValue();
        }

        private void GameOptionFormat() {
            MinerPercent.ValueStringFormat = (option, value) => $"{value}%";
            MinerPercent.ShowChildrenConidtion = () => MinerPercent.GetValue() > 0;

            NumberMiner.ValueStringFormat = (option, value) => $"{value} players";
            MinerCooldown.ValueStringFormat = (option, value) => $"{value}s";

            MaxUseMiner.ValueStringFormat = (option, value) => $"{value} time";
        }
    }
}
