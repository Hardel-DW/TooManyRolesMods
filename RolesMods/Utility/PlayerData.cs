using RolesMods.Utility.Enumerations;

namespace RolesMods.Utility {
    using PlayerDataObj = EGLJNOMOGNP.DCJMABDDJCF;
    using PlayerPrefab = FFGALNAPKCD;

    public class PlayerData {
        /// <summary>
        /// Player Controller API object
        /// </summary>
        public Player PlayerController {
            get;
        }

        /// <summary>
        /// Player Data Object
        /// </summary>
        public PlayerDataObj PlayerDataObject {
            get {
                return PlayerController.PlayerControl.NDGFFHMFGIG;
            }
        }

        /// <summary>
        /// Player Prefab object
        /// </summary>
        public PlayerPrefab PlayerPrefab {
            get {
                return PlayerDataObject.LAOEJKHLKAI;
            }
        }

        /// <summary>
        /// Gets or sets the among us is impostor value
        /// </summary>
        public bool IsImpostor {
            get {
                return PlayerDataObject.DAPKNDBLKIA;
            }

            set {
                PlayerDataObject.DAPKNDBLKIA = value;
            }
        }

        public byte PlayerId {
            get {
                return PlayerDataObject.JKOMCOJCAID;
            }
        }

        public byte ColorId {
            get {
                return PlayerDataObject.EHAHBDFODKC;
            }

            set {
                PlayerDataObject.EHAHBDFODKC = value;
            }
        }

        public ColorType Color {
            get {
                return (ColorType) ColorId;
            }
            set {
                ColorId = (byte) value;
            }
        }

        public uint SkinId {
            get {
                return PlayerDataObject.HPAMBHFDLEH;
            }

            set {
                PlayerDataObject.HPAMBHFDLEH = value;
            }
        }

        public SkinType Skin {
            get {
                return (SkinType) SkinId;
            }

            set {
                SkinId = (uint) value;
            }
        }

        public uint PetId {
            get {
                return PlayerDataObject.AJIBCNMKNPM;
            }
            set {
                PlayerDataObject.AJIBCNMKNPM = value;
            }
        }

        public PetType Pet {
            get {
                return (PetType) PetId;
            }

            set {
                PetId = (uint) value;
            }
        }

        public uint HatId {
            get {
                return PlayerDataObject.AFEJLMBMKCJ;
            }

            set {
                PlayerDataObject.AFEJLMBMKCJ = value;
            }
        }

        public HatType Hat {
            get {
                return (HatType) HatId;
            }

            set {
                HatId = (uint) value;
            }
        }

        /// <summary>
        /// Gets or sets player dead boolean
        /// </summary>
        public bool IsDead {
            get {
                return PlayerDataObject.DLPCKPBIJOE;
            }
            set {
                PlayerDataObject.DLPCKPBIJOE = value;
            }
        }

        /// <summary>
        /// Gets or sets the player name
        /// </summary>
        public string PlayerName {
            get {
                return PlayerDataObject.EIGEKHDAKOH;
            }
            set {
                PlayerDataObject.EIGEKHDAKOH = value;
            }
        }

        /// <summary>
        /// Creates a player data object
        /// </summary>
        /// <param name="controller">Player Controller Object</param>
        public PlayerData(Player controller) {
            PlayerController = controller;
        }

        public PlayerData(PlayerDataObj data) {
            PlayerController = new Player(data.LAOEJKHLKAI);
        }
    }
}