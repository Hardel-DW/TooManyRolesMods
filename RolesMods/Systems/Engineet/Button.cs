/*using HardelAPI.Utility;
using HarmonyLib;
using Hazel;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;

namespace RolesMods.Systems.Engineer {

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class Button {
        public static CooldownButton button;
        public static int UseNumber = 1;

        public static void Postfix(HudManager __instance) {
            button = new CooldownButton
                (() => OnClick(),
                Roles.Engineer.EngineerCooldown.GetValue(),
                Plugin.LoadSpriteFromEmbeddedResources("RolesMods.Resources.Rewind.png", 250f),
                250,
                new Vector2(0f, 0f),
                __instance,
                () => OnUpdate(button)
            );
        }


        private static void OnClick() {
            if (UseNumber > 0) {
                FixSabotage();
                UseNumber--;
            }
        }

        private static void OnUpdate(CooldownButton button) {
            if (Roles.TimeMaster.Instance.AllPlayers != null && PlayerControl.LocalPlayer != null)
                if (Roles.TimeMaster.Instance.HasRole(PlayerControl.LocalPlayer.PlayerId))
                    if (PlayerControl.LocalPlayer.Data.IsDead || UseNumber <= 0) 
                        button.SetCanUse(false);
                    else button.SetCanUse(!MeetingHud.Instance);
        }

        private static void FixSabotage() {
            SabotageSystemType system = ShipStatus.Instance.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>();
            Il2CppArrayBase<IActivatable> specials = system.specials.ToArray();
            if (!system.dummy.IsActive | specials.Any(s => s.IsActive))
                return;

            if (ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>().IsActive)
                FixComms();
            if (ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<ReactorSystemType>().IsActive)
                FixReactor(SystemTypes.Reactor);
            if (ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>().IsActive)
                FixOxygen();
            if (ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>().IsActive)
                FixMiraComms();
            if (ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<HeliSabotageSystem>().IsActive)
                FixAirshipReactor();

            SwitchSystem fixLight = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
            if (fixLight.IsActive) FixLights(fixLight);

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.EngineerFix, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.NetId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        private static bool FixComms() {
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 0);
            return false;
        }

        private static bool FixMiraComms() {
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 0);
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 1);
            return false;
        }

        private static bool FixAirshipReactor() {
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 16 | 0);
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 16 | 1);
            return false;
        }

        private static bool FixReactor(SystemTypes system) {
            ShipStatus.Instance.RpcRepairSystem(system, 16);
            return false;
        }

        private static bool FixOxygen() {
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.LifeSupp, 16);
            return false;
        }

        private static bool FixLights(SwitchSystem lights) {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.FixLights, SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            lights.ActualSwitches = lights.ExpectedSwitches;
            return false;
        }
    }
}*/