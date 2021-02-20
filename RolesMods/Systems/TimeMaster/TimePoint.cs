using UnityEngine;

namespace RolesMods.Systems.TimeMaster {
    class TimePoint {

		private Vector3 position;
		private Vector2 velocity;
        private long unix;

		public TimePoint(Vector3 position, Vector2 velocity, long unix) {
			this.position = position;
			this.velocity = velocity;
            this.unix = unix;
        }

        public Vector3 Position {
            get => position;
            set => position = value;
        }

        public Vector2 Velocity {
            get => velocity;
            set => velocity = value;
        }

        public long Unix {
            get => unix;
            set => unix = value;
        }
    }
}
