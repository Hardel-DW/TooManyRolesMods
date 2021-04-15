using RolesMods.Utility.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RolesMods.Utility.CustomRoles {

    public class RoleManager {
        public static List<RoleManager> AllRoles = new List<RoleManager>();
        public List<PlayerControl> AllPlayers = new List<PlayerControl>();
        public List<PlayerControl> WhiteListKill = null;
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
        public bool CanVent = false;
        public float KillCooldown = 0f;
        public DateTime LastKilled;
        public Color Color = new Color(1f, 0f, 0f, 1f);
        public PlayerSide Side = PlayerSide.Crewmate;
        public PlayerSide CanKill = PlayerSide.Nobody;
        public PlayerSide VisibleBy = PlayerSide.Self;
        public Moment GiveTasksAt = Moment.StartGame;
        public Moment GiveRoleAt = Moment.StartGame;
        private Type ClassType;
        public bool TaskAreRemove = false;

        public virtual List<IAbility> Abilities { get; set; } = null;

        public virtual List<CooldownButton> Button { get; set; } = null;

        protected RoleManager(Type type) {
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

        // Clear the list of all roles
        public static void ClearAllRoles() {
            foreach (var Role in AllRoles) {
                Role.ClearRole();
            }
        }

        // Has Roles
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

        // Player List
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

        public void AddPlayerRange(List<PlayerControl> Players) {
            AllPlayers.AddRange(Players);
        }

        public void RemovePlayer(byte PlayerId) {
            AllPlayers.Remove(AllPlayers.FirstOrDefault(p => p.PlayerId == PlayerId));
        }

        public void RemovePlayer(PlayerControl Player) {
            AllPlayers.Remove(AllPlayers.FirstOrDefault(p => p.PlayerId == Player.PlayerId));
        }

        // Kill WhiteList
        public void AddPlayerTokillWhiteLisT(PlayerControl Player) {
            WhiteListKill.Add(Player);
        }

        public void AddPlayerTokillWhiteLisT(byte PlayerId) {
            WhiteListKill.Add(PlayerControlUtils.FromPlayerId(PlayerId));
        }

        public void AddPlayerRangeTokillWhiteLisT(List<byte> PlayersId) {
            foreach (var PlayerId in PlayersId)
                WhiteListKill.Add(PlayerControlUtils.FromPlayerId(PlayerId));
        }

        public void AddPlayerRangeTokillWhiteLisT(List<PlayerControl> Players) {
            WhiteListKill.AddRange(Players);
        }

        public void RemovePlayerTokillWhiteLisT(byte PlayerId) {
            WhiteListKill.Remove(WhiteListKill.FirstOrDefault(p => p.PlayerId == PlayerId));
        }

        public void RemovePlayerTokillWhiteLisT(PlayerControl Player) {
            WhiteListKill.Remove(WhiteListKill.FirstOrDefault(p => p.PlayerId == Player.PlayerId));
        }

        public void ClearKillWhiteList() {
            WhiteListKill.Clear();
        }

        // Add tasks to player
        public void AddImportantTasks(PlayerControl Player) {
            ImportantTextTask ImportantTasks = new GameObject("RolesTasks").AddComponent<ImportantTextTask>();
            ImportantTasks.transform.SetParent(Player.transform, false);
            ImportantTasks.Text = TasksDescription;
            Player.myTasks.Insert(0, ImportantTasks);
        }

        // Kill Ability
        public virtual void DefineKillWhiteList() {
            List<PlayerControl> AllPlayer = PlayerControl.AllPlayerControls.ToArray().ToList();

            WhiteListKill = CanKill switch
            {
                PlayerSide.Everyone => AllPlayer,
                PlayerSide.Impostor => AllPlayer.FindAll(p => p.Data.IsImpostor),
                PlayerSide.Crewmate => AllPlayer.FindAll(p => !p.Data.IsImpostor),
                PlayerSide.SameRole => AllPlayer.FindAll(p => HasRole(p)),
                _ => null
            };

            if (CanKill == PlayerSide.Nobody && WhiteListKill == null && PlayerControl.LocalPlayer.Data.IsImpostor)
                WhiteListKill = AllPlayer.FindAll(p => !p.Data.IsImpostor);
        }

        public float KillTimer() {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastKilled;
            var cooldown = KillCooldown * 1000f;
            if (cooldown - (float) timeSpan.TotalMilliseconds < 0f)
                return 0;

            return (cooldown - (float) timeSpan.TotalMilliseconds) / 1000f;
        }

        public PlayerControl GetClosestTarget(PlayerControl PlayerReference) {
            double distance = double.MaxValue;
            PlayerControl result = null;

            foreach (var player in WhiteListKill) {
                float distanceBeetween = Vector2.Distance(player.transform.position, PlayerReference.transform.position);
                if (player.Data.IsDead || player.PlayerId == PlayerReference.PlayerId || distance < distanceBeetween)
                    continue;

                distance = distanceBeetween;
                result = player;
            }

            return result;
        }

        // Event
        public virtual void OnGameStart() { }

        public virtual void OnGameEnded() { }

        public virtual void OnMeetingStart() { }

        public virtual void OnMeetingEnd() { }

        public virtual void OnInfectedStart() { }

        public virtual void OnLocalAttempKill(PlayerControl killer, PlayerControl target) {
            killer.RpcMurderPlayer(target);
        }

        public virtual void OnUpdate(PlayerControl __instance) { }

        public virtual void OnDie(PlayerControl __instance) { }

        public virtual void OnRevive(PlayerControl __instance) { }
    }
}
