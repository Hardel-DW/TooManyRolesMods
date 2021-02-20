namespace RolesMods {
    public static class HelperRoles {

        public static bool IsInvestigator(byte playerId) {
            return playerId == GlobalVariable.Investigator.PlayerId;
        }

        public static bool IsTimeMaster(byte playerId) {
            return playerId == GlobalVariable.TimeMaster.PlayerId;
        }
        public static bool IsLighter(byte playerId) {
            return playerId == GlobalVariable.Lighter.PlayerId;
        }

    }
}
