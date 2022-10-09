using AlbionDamageMetter.Albion.Models.NetworkModel;
using Microsoft.AspNetCore.Mvc;
using System.Timers;

namespace AlbionDamageMetter.Albion.Models.NetworkModel
{
    public class PlayerGameObject : GameObject
    {
        private CharacterEquipment _characterEquipment;
        private Guid _userGuid;
        private Guid? _interactGuid;
        private object _lock = new object();

        private Queue<long> _queueDamage;
        private Dictionary<DateTime, long> _damageList;

        private Queue<long> _queueHeal;
        private Dictionary<DateTime, long> _healList;

        public string Name { get; set; } = "Unknown";
        public string Guild { get; set; }
        public string Alliance { get; set; }
        public long LastUpdate { get; private set; }
        public DateTime? CombatStart { get; set; }
        public long Damage { get; set; }
        public long Heal { get; set; }
        public long Dps { get; set; }

        public PlayerGameObject(long objectId)
        {
            ObjectId = objectId;
            LastUpdate = DateTime.UtcNow.Ticks;
            _queueDamage = new Queue<long>();
            _queueHeal = new Queue<long>();
            DamageList = new Dictionary<DateTime, long>();
            HealList = new Dictionary<DateTime, long>();
        }

        public void OnTimedEvent()
        {
            lock (this._lock)
            {
                long damage = 0;
                long healing = 0;
                while (_queueDamage.Count > 0)
                {
                    damage += _queueDamage.Dequeue();
                }
                while (_queueHeal.Count > 0)
                {
                    healing += _queueHeal.Dequeue();
                }
                Dps = damage;
                Damage += damage;
                Heal += healing;
                var date = DateTime.UtcNow;
                DamageList.Add(date, damage);
                HealList.Add(date, healing);
            }
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

        public Dictionary<DateTime, long> DamageList
        {
            get => _damageList;
            set
            {
                _damageList = value;
            }
        }

        public Dictionary<DateTime, long> HealList
        {
            get => _healList;
            set
            {
                _healList = value;
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

        public void AddDamage(DateTime date, long damage)
        {
            lock (this._lock)
            {
                _queueDamage.Enqueue(damage);
            }
        }

        public void AddHealing(DateTime date, long heal)
        {
            lock (this._lock)
            {
                _queueHeal.Enqueue(heal);
            }
        }

        public void Reset()
        {
            lock (this._lock)
            {
                Heal = 0;
                Damage = 0;
                HealList.Clear();
                DamageList.Clear();
                _queueHeal.Clear();
                _queueDamage.Clear();
            }
        }
    }
}
