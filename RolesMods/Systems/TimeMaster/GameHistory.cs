using System;
using UnityEngine;

namespace RolesMods.Systems.TimeMaster {
    public class GameHistory {
        public Vector3 position;
        public DateTime positionTime;
        public Vector2 velocity;

        public GameHistory(Vector3 position, DateTime positionTime, Vector2 velocity) {
            this.position = position;
            this.positionTime = positionTime;
            this.velocity = velocity;
        }
    }
}
