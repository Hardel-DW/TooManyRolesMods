using RolesMods.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods {
    public static class GlobalVariable {

        // Roles
        public static List<PlayerControl> InvestigatorsList = new List<PlayerControl>();
        public static List<PlayerControl> LightersList = new List<PlayerControl>();
        public static PlayerControl TimeMaster;

        //  Misc
        public static bool isGameStarted = false;

        // Button
        internal static CooldownButton buttonTime;
    }
}
