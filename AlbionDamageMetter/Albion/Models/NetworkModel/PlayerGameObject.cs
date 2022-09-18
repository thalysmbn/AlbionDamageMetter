using AlbionDamageMetter.Albion.Models.NetworkModel;
using Microsoft.AspNetCore.Mvc;

namespace AlbionDamageMetter.Albion.Models.NetworkModel
{
    public class PlayerGameObject : GameObject
    {
        private CharacterEquipment _characterEquipment;
        private Guid _userGuid;
        private Guid? _interactGuid;
        public IList<long> DamageList = new List<long>();
        public IList<long> HealList = new List<long>();
        public string Name { get; set; } = "Unknown";
        public string Guild { get; set; }
        public string Alliance { get; set; }
        public long LastUpdate { get; private set; }
        public DateTime? CombatStart { get; set; }
        public long Damage { get; set; }
        public long Heal { get; set; }

        public PlayerGameObject(long objectId)
        {
            ObjectId = objectId;
            LastUpdate = DateTime.UtcNow.Ticks;
        }

        public Guid UserGuid
        {
            get => _userGuid;
            set
            {
                _userGuid = value;
                LastUpdate = DateTime.UtcNow.Ticks;
            }
        }
        public Guid? InteractGuid
        {
            get => _interactGuid;
            set
            {
                _interactGuid = value;
                LastUpdate = DateTime.UtcNow.Ticks;
            }
        }
        public CharacterEquipment CharacterEquipment
        {
            get => _characterEquipment;
            set
            {
                _characterEquipment = value;
                LastUpdate = DateTime.UtcNow.Ticks;
            }
        }
    }
}
