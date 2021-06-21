using System;
using Harion.Cooldown;
using PsychicRoles = RolesMods.Roles.Psychic;

namespace RolesMods.Systems.Psychic {

    [RegisterCooldownButton]
    public class Button : CustomButton<Button> {

        public override void OnCreateButton() {
            Roles = PsychicRoles.Instance;
            Timer = PsychicRoles.PsychicCooldown.GetValue();
            EffectDuration = PsychicRoles.PsychicDuration.GetValue();
            HasEffectDuration = true;
            DecreamteUseNimber = UseNumberDecremantion.OnClick;
            SetSprite("RolesMods.Resources.Foresight.png", 1000);
        }

        public override void OnEffectEnd() {
            PsychicMap.isPsychicActivated = false;
            PsychicMap.ClearAllPlayers();

            if (MapBehaviour.Instance != null) {
                HudManager.Instance.OpenMap();
                HudManager.Instance.OpenMap();
            }
        }

        public override void OnClick() {
            PsychicMap.isPsychicActivated = true;
            DestroyableSingleton<HudManager>.Instance.ShowMap((Action<MapBehaviour>) (map => {
                map.gameObject.SetActive(true);
                map.gameObject.AddComponent<PsychicMap>();
                DestroyableSingleton<HudManager>.Instance.SetHudActive(false);
            }));
        }
    }
}
