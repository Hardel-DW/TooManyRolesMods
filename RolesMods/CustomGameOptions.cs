namespace RolesMods {

    public static class CustomGameOptions {
        public static bool EnableInvestigator;
        public static float NumberInvestigator;
        public static float footPrintSize;
        public static float fontPrintInterval;
        public static float fontPrintDuration;
        public static bool AnonymousFootPrint;

        public static void SetConfigSettings() {
            EnableInvestigator = RolesMods.EnableInvestigator.GetValue();
            NumberInvestigator = RolesMods.NumberInvestigator.GetValue();
            footPrintSize = RolesMods.footPrintSize.GetValue();
            fontPrintInterval = RolesMods.fontPrintInterval.GetValue();
            fontPrintDuration = RolesMods.fontPrintDuration.GetValue();;
            AnonymousFootPrint = RolesMods.AnonymousFootPrint.GetValue();
        }
    }
}
