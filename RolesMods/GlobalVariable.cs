using System.Collections.Generic;

namespace RolesMods {
    public static class GlobalVariable {

        // Roles
        public static List<PlayerControl> InvestigatorsList = new List<PlayerControl>();
        public static List<PlayerControl> LightersList = new List<PlayerControl>();
        public static PlayerControl TimeMaster;

        //  Misc
        public static bool isGameStarted = false;
        public static List<int> footprintSizeValues = new List<int>() {4096, 3072, 2048, 1536, 1024, 820, 705, 600, 550, 512 };

        // Button
        internal static CooldownButton buttonTime;
    }
}
