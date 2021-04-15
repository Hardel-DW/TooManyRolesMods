using HarmonyLib;
using RolesMods.Roles;

namespace RolesMods.Utility.CustomRoles.Patch {

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudUpdatePatch {
        public static void Postfix(HudManager __instance) {
            foreach (var Role in RoleManager.AllRoles) {
                if (PlayerControl.LocalPlayer != null || (PlayerControl.AllPlayerControls != null && PlayerControl.AllPlayerControls.Count > 0) || (Role.AllPlayers != null && Role.AllPlayers.Count > 0)) {

                    if (MeetingHud.Instance != null) 
                        UpdateMeetingHUD(MeetingHud.Instance, Role);

                    foreach (var PlayerHasRole in Role.AllPlayers) {
                        if (!Role.HasRole(PlayerHasRole))
                            continue;

                        switch (Role.VisibleBy) {
                            case Enumerations.PlayerSide.Self:
                                if (PlayerHasRole.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                                    PlayerHasRole.nameText.color = Role.Color;
                            break;
                            case Enumerations.PlayerSide.Impostor:
                                if (PlayerControl.LocalPlayer.Data.IsImpostor)
                                    PlayerHasRole.nameText.color = Role.Color;
                            break;
                            case Enumerations.PlayerSide.Crewmate:
                                if (!PlayerControl.LocalPlayer.Data.IsImpostor)
                                    PlayerHasRole.nameText.color = Role.Color;
                            break;
                            case Enumerations.PlayerSide.Everyone:
                                PlayerHasRole.nameText.color = Role.Color;
                            break;
                            case Enumerations.PlayerSide.Dead:
                                if (PlayerControl.LocalPlayer.Data.IsDead)
                                    PlayerHasRole.nameText.color = Role.Color;
                            break;
                            case Enumerations.PlayerSide.DeadCrewmate:
                                if (PlayerControl.LocalPlayer.Data.IsDead && !PlayerControl.LocalPlayer.Data.IsImpostor)
                                    PlayerHasRole.nameText.color = Role.Color;
                            break;
                            case Enumerations.PlayerSide.DeadImpostor:
                                if (PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.Data.IsImpostor)
                                    PlayerHasRole.nameText.color = Role.Color;
                            break;
                            case Enumerations.PlayerSide.SameRole:
                                if (Role.HasRole(PlayerControl.LocalPlayer.PlayerId))
                                    PlayerHasRole.nameText.color = Role.Color;
                            break;
                        }
                    }
                }
            }
        }

        public static void UpdateMeetingHUD(MeetingHud __instance, RoleManager Role) {
            foreach (var PlayerHasRole in Role.AllPlayers) {
                foreach (PlayerVoteArea PlayerVA in __instance.playerStates) {
                    PlayerControl Player = PlayerControlUtils.FromPlayerId((byte) PlayerVA.TargetPlayerId);
                    if (PlayerHasRole.PlayerId != Player.PlayerId || !Role.HasRole(PlayerHasRole))
                        continue;

                    switch (Role.VisibleBy) {
                        case Enumerations.PlayerSide.Self:
                        if (PlayerControl.LocalPlayer.PlayerId == PlayerHasRole.PlayerId)
                            PlayerVA.NameText.color = Role.Color;
                        break;
                        case Enumerations.PlayerSide.Impostor:
                        if (PlayerControl.LocalPlayer.Data.IsImpostor)
                            PlayerVA.NameText.color = Role.Color;
                        break;
                        case Enumerations.PlayerSide.Crewmate:
                        if (!PlayerControl.LocalPlayer.Data.IsImpostor)
                            PlayerVA.NameText.color = Role.Color;
                        break;
                        case Enumerations.PlayerSide.Everyone:
                            PlayerVA.NameText.color = Role.Color;
                        break;
                        case Enumerations.PlayerSide.Dead:
                        if (PlayerControl.LocalPlayer.Data.IsDead)
                            PlayerVA.NameText.color = Role.Color;
                        break;
                        case Enumerations.PlayerSide.DeadCrewmate:
                        if (PlayerControl.LocalPlayer.Data.IsDead && !PlayerControl.LocalPlayer.Data.IsImpostor)
                            PlayerVA.NameText.color = Role.Color;
                        break;
                        case Enumerations.PlayerSide.DeadImpostor:
                        if (PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.Data.IsImpostor)
                            PlayerVA.NameText.color = Role.Color;
                        break;
                        case Enumerations.PlayerSide.SameRole:
                        if (Role.HasRole(PlayerControl.LocalPlayer.PlayerId))
                            PlayerVA.NameText.color = Role.Color;
                        break;
                    }
                }
            }
        }
    }
}
