using RolesMods.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods {
    public static class GlobalVariable {

        // Roles
        public static List<PlayerControl> PsychicList = new List<PlayerControl>();
        public static List<PlayerControl> InvestigatorsList = new List<PlayerControl>();
        public static List<PlayerControl> LightersList = new List<PlayerControl>();
        public static PlayerControl TimeMaster;

        //  Misc
        public static bool isGameStarted = false;
        public static bool ispsychicActivated = false;
        public static List<SpriteRenderer> herePoints = new List<SpriteRenderer>();
        public static List<TextRenderer> texts = new List<TextRenderer>();

        // Button
        internal static CooldownButton buttonTime;
        internal static CooldownButton buttonPsychic;
    }
}
