using RolesMods.Utility.Enumerations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Utility.CustomRoles {

    public class RoleManager {
        public static List<RoleManager> AllRoles = new List<RoleManager>();
        public List<PlayerControl> AllPlayers = new List<PlayerControl>();
        public byte RoleId;
        public string Name = "Not Defined";
        public string TasksDescription = "You can defined the role in the class.";
        public string IntroDescription = "You can defined the role in the class";
        public int NumberPlayers = 1;
        public int PercentApparition = 100;
        public bool ShowIntroCutScene = true;
        public bool CanHasOtherRole = false;
        public bool RoleActive = true;
        public bool HasTask = true;
        /*public bool CanReport = true;
        public bool CanCallMeeting = true;
        public bool CanKill = false;
        public bool CanVent = false;*/
        public Color Color = new Color(1f, 0f, 0f, 1f);
        public PlayerSide Side = PlayerSide.Crewmate;
        public PlayerSide VisibleBy = PlayerSide.Self;
        public Moment GiveTasksAt = Moment.StartGame;
        public Moment GiveRoleAt = Moment.StartGame;
        private System.Type ClassType;
        public bool TaskAreRemove = false;

        public virtual List<IAbility> Abilities { get; set; } = null;

        public virtual List<CooldownButton> Button { get; set; } = null;

        protected RoleManager(System.Type type) {
            ClassType = type;
            RoleId = GetAvailableRoleId();
            AllRoles.Add(this);
            Plugin.Logger.LogInfo($"Role: {type.Name} Loaded, RoleID: {RoleId}");
        }

        private byte GetAvailableRoleId() {
            byte id = 0;

            while (true) {
                if (!AllRoles.Any(v => v.RoleId == id))
                    return id;

                id++;
            }
        }

        public static RoleManager GerRoleById(byte RoleId) {
            return AllRoles.FirstOrDefault(r => r.RoleId == RoleId);
        }

        public static void ClearAllRoles() {
            foreach (var Role in AllRoles) {
                Role.ClearRole();
            }
        }

        public bool HasRole(byte PlayerId) {
            bool HasRoles = false;

            if (AllPlayers != null) {
                for (int i = 0; i < AllPlayers.Count; i++) {
                    if (PlayerId == AllPlayers[i].PlayerId)
                        HasRoles = true;
                }
            }

            return HasRoles;
        }

        public bool HasRole(PlayerControl Player) {
            bool HasRoles = false;

            if (AllPlayers != null) {
                for (int i = 0; i < AllPlayers.Count; i++) {
                    if (Player.PlayerId == AllPlayers[i].PlayerId)
                        HasRoles = true;
                }
            }

            return HasRoles;
        }

        public void ClearRole() {
            AllPlayers.Clear();
        }

        public void AddPlayer(PlayerControl Player) {
            AllPlayers.Add(Player);
        }

        public void AddPlayer(byte PlayerId) {
            AllPlayers.Add(PlayerControlUtils.FromPlayerId(PlayerId));
        }

        public void AddPlayerRange(List<byte> PlayersId) {
            foreach (var PlayerId in PlayersId)
                AllPlayers.Add(PlayerControlUtils.FromPlayerId(PlayerId));
        }

        public void RemovePlayer(byte PlayerId) {
            AllPlayers.Remove(AllPlayers.FirstOrDefault(p => p.PlayerId == PlayerId));
        }

        public void RemovePlayer(PlayerControl Player) {
            AllPlayers.Remove(AllPlayers.FirstOrDefault(p => p.PlayerId == Player.PlayerId));
        }

        public void AddImportantTaks(PlayerControl Player) {
            ImportantTextTask ImportantTasks = new GameObject("RolesTasks").AddComponent<ImportantTextTask>();
            ImportantTasks.transform.SetParent(Player.transform, false);
            ImportantTasks.Text = TasksDescription;
            Player.myTasks.Insert(0, ImportantTasks);
        }

        public virtual void OnGameStart() { }

        public virtual void OnGameEnded() { }

        public virtual void OnMeetingStart() { }

        public virtual void OnMeetingEnd() { }

        public virtual void OnInfectedStart() { }

        public virtual void OnUpdate(PlayerControl __instance) { }

        public virtual void OnDie(PlayerControl __instance) { }

        public virtual void OnRevive(PlayerControl __instance) { }
    }
}
