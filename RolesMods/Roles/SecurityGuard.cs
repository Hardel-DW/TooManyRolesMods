using Essentials.Options;
using HardelAPI.CustomRoles;
using HardelAPI.Enumerations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Roles {

    [RegisterInCustomRoles(typeof(SecurityGuard))]
    public class SecurityGuard : CustomRole<SecurityGuard> {
        // Color: #07db00FF
        public static CustomOptionHeader SecurityGuardHeader = CustomOptionHeader.AddHeader("<color=#d4B40cff>Security Guard Options :</color>");
        public static CustomNumberOption SecurityGuardPercent = CustomOption.AddNumber("SecurityGuard Apparition", 0f, 0f, 100f, 5f);
        public static CustomNumberOption NumberSecurityGuard = CustomOption.AddNumber("Number Security Guard", 1f, 1f, 10f, 1f);
        public static CustomNumberOption CooldownSecurityGuard = CustomOption.AddNumber("Security Guard Cooldown", 30f, 10f, 120f, 5f);
        public static CustomNumberOption NumberScrews = CustomOption.AddNumber("Security Guard Number Of Screws", 1f, 1f, 30f, 1f);
        public static CustomNumberOption ScrewsCams = CustomOption.AddNumber("Number Of Screws Per Cam", 1f, 1f, 10f, 1f);
        public static CustomNumberOption ScrewsVent = CustomOption.AddNumber("Number Of Screws Per Vent", 1f, 1f, 10f, 1f);
        public static List<SurvCamera> camerasToAdd = new List<SurvCamera>();
        public static List<Vent> ventsToSeal = new List<Vent>();

        public SecurityGuard() : base() {
            GameOptionFormat();
            Side = PlayerSide.Crewmate;
            RoleActive = true;
            GiveRoleAt = Moment.StartGame;
            GiveTasksAt = Moment.StartGame;
            Color = new Color(0.831f, 0.705f, 0.047f, 1f);
            Name = "Security Guard";
            IntroDescription = "Seal vents and place cameras";
            TasksDescription = "<color=#d4B40cff>Security Guard: Seal vents and place cameras</color>";
        }

        public override void OnInfectedStart() {
            PercentApparition = (int) SecurityGuardPercent.GetValue();
            NumberPlayers = (int) NumberSecurityGuard.GetValue();
        }

        public override void OnGameStarted() {
            camerasToAdd = new List<SurvCamera>();
            ventsToSeal = new List<Vent>();
        }

        public override void OnExiledPlayer(PlayerControl PlayerExiled) {
            if (PlayerExiled.PlayerId == PlayerControl.LocalPlayer.PlayerId) {
                camerasToAdd.ForEach(x => x.gameObject.SetActive(true));
                var allCameras = ShipStatus.Instance.AllCameras.ToList();
                allCameras.AddRange(camerasToAdd);
                ShipStatus.Instance.AllCameras = allCameras.ToArray();
                camerasToAdd = new List<SurvCamera>();

                foreach (Vent vent in ventsToSeal) {
                    PowerTools.SpriteAnim animator = vent.GetComponent<PowerTools.SpriteAnim>();
                    animator?.Stop();
                    //vent.myRend.sprite = animator == null ? SecurityGuard.getStaticVentSealedSprite() : SecurityGuard.getAnimatedVentSealedSprite();
                    vent.name = "SealedVent_" + vent.name;
                }

                ventsToSeal = new List<Vent>();
            }
        }

        private void GameOptionFormat() {
            SecurityGuardHeader.HudStringFormat = (option, name, value) => $"\n{name}";

            SecurityGuardPercent.ValueStringFormat = (option, value) => $"{value}%";
            NumberSecurityGuard.ValueStringFormat = (option, value) => $"{value} players";
            CooldownSecurityGuard.ValueStringFormat = (option, value) => $"{value}s";
        }

        public static void placeCamera(byte[] buff) {
/*            var referenceCamera = UnityEngine.Object.FindObjectOfType<SurvCamera>();
            if (referenceCamera == null)
                return; // Mira HQ

            SecurityGuard.remainingScrews -= SecurityGuard.camPrice;
            SecurityGuard.placedCameras++;

            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));

            var camera = UnityEngine.Object.Instantiate<SurvCamera>(referenceCamera);
            camera.transform.position = new Vector3(position.x, position.y, referenceCamera.transform.position.z - 1f);
            camera.CamName = $"Security Guard Camera {SecurityGuard.placedCameras}";
            if (PlayerControl.GameOptions.MapId == 2 || PlayerControl.GameOptions.MapId == 4)
                camera.transform.localRotation = new Quaternion(0, 0, 1, 1); // Polus and Airship 
            camera.gameObject.SetActive(false);
            camerasToAdd.Add(camera);*/
        }

        public static void sealVent(int ventId) {
/*            Vent vent = ShipStatus.Instance.AllVents.FirstOrDefault((x) => x != null && x.Id == ventId);
            if (vent == null)
                return;

            SecurityGuard.remainingScrews -= SecurityGuard.ventPrice;
            ventsToSeal.Add(vent);*/
        }
    }
}
