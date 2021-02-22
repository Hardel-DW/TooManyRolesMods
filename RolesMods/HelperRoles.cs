namespace RolesMods {
    public static class HelperRoles {

        public static bool IsTimeMaster(byte playerId) {
            return GlobalVariable.TimeMaster != null ? playerId == GlobalVariable.TimeMaster.PlayerId : false;
        }

        public static bool IsLighter(byte playerId) {
            bool isLighter = false;

            if (GlobalVariable.LightersList != null) {
                for (int i = 0; i < GlobalVariable.LightersList.Count; i++) {
                    if (playerId == GlobalVariable.LightersList[i].PlayerId)
                        isLighter = true;
                }
            }

            return isLighter;
        }

        public static bool IsInvestigator(byte playerId) {
            bool isInvestigator = false;

            if (GlobalVariable.InvestigatorsList != null) {
                for (int i = 0; i < GlobalVariable.InvestigatorsList.Count; i++) {
                    if (playerId == GlobalVariable.InvestigatorsList[i].PlayerId)
                        isInvestigator = true;
                }
            }

            return isInvestigator;
        }

        public static void ClearRoles() {
            if (GlobalVariable.InvestigatorsList != null && GlobalVariable.InvestigatorsList.Count > 0)
                GlobalVariable.InvestigatorsList.Clear();

            if (GlobalVariable.LightersList != null && GlobalVariable.LightersList.Count > 0)
                GlobalVariable.LightersList.Clear();

            if (GlobalVariable.TimeMaster != null)
                GlobalVariable.TimeMaster = null;
        }
    }
}
