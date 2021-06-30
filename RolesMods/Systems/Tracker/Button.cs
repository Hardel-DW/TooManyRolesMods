using Harion.ArrowManagement;
using Harion.Cooldown;
using Harion.Utility;
using System.Collections.Generic;
using TrackerRoles = RolesMods.Roles.Tracker;

namespace RolesMods.Systems.Tracker {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {
        public static ArrowManager Arrow;

        public override void OnClick() {
            UseNumber = 0;
            PlayerButton.InitPlayerButton(
                false, 
                new List<PlayerControl> { PlayerControl.LocalPlayer },
                (Player) => OnCPlayerChoose(Player),
                () => UseNumber = int.MaxValue
            );
        }

        public override void OnCreateButton() {
            Timer = 10f;
            Roles = TrackerRoles.Instance;
            SetSprite(ResourceLoader.TargetSprite);
        }

        private void OnCPlayerChoose(PlayerControl Player) {
            Arrow = new ArrowManager(Player.gameObject, Player.transform.position, true, TrackerRoles.TargetUpdate.GetValue());
        }
    }
}