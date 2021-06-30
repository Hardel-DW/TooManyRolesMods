using Harion.CustomRoles;
using Harion.Utility.Utils;
using System.Collections.Generic;
using UnityEngine;
using Harion.Cooldown;
using SeerRoles = RolesMods.Roles.Seer;

namespace RolesMods.Systems.Seer {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {

        public override void OnClick() {
            PlayerControl closestPlayer = GetPlayerTarget();
            if (closestPlayer.Data.IsImpostor)
                RevealRole(closestPlayer, true);
            else {
                int PercentReaveal = new System.Random().Next(0, 100);
                if (PercentReaveal > SeerRoles.SeerPercentSeeRole.GetValue())
                    RevealRole(closestPlayer, false);
                else
                    RevealRole(closestPlayer, true);
            }
        }

        public override void OnCreateButton() {
            Timer = SeerRoles.SeerCooldown.GetValue();
            UseNumber = (int) SeerRoles.SeerUseNumber.GetValue();
            Roles = SeerRoles.Instance;
            DecreamteUseNimber = UseNumberDecremantion.OnClick;
            Closest = Harion.Cooldown.ClosestElement.Player;
            AllPlayersTargetable = new List<PlayerControl>();
            SetSprite(ResourceLoader.RewindRedSprite);
        }

        private void RevealRole(PlayerControl target, bool failed) {
            RoleManager mainRole = RoleManager.GetMainRole(target);
            Color colorDisplay = mainRole != null ? mainRole.Color : target.Data.IsImpostor ?  Palette.ImpostorRed : Palette.White;
            string nameDisplay = mainRole != null ? mainRole.Name : target.Data.IsImpostor ? "Impostor" : "Crewmate";

            if (SeerRoles.ShowGoodOrBad.GetValue())
                nameDisplay = target.Data.IsImpostor ? "Bad" : "Good";

            if (failed) RoleManager.specificNameInformation.Add(target, (Palette.White, "???"));
            else RoleManager.specificNameInformation.Add(target, (colorDisplay, nameDisplay));
            AllPlayersTargetable.RemovePlayer(target);
        }
    }
}