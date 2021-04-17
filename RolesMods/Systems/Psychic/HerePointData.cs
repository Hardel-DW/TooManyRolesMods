using TMPro;
using UnityEngine;

namespace RolesMods.Systems.Psychic {
    class HerePointData {
        public SpriteRenderer herePoint { get; set; }
        public TextMeshPro text { get; set; }
        public PlayerControl player { get; set; }

        public HerePointData(SpriteRenderer herePoint, PlayerControl player, TextMeshPro text = null) {
            this.herePoint = herePoint;
            this.text = text;
            this.player = player;
        }

        public HerePointData() { }
    }
}
