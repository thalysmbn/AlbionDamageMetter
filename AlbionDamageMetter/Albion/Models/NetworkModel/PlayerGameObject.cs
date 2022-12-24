using AlbionDamageMetter.Albion.Models.NetworkModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.Json.Serialization;
using System.Timers;

namespace AlbionDamageMetter.Albion.Models.NetworkModel
{
    public class PlayerGameObject : GameObject
    {
        [JsonIgnore]
        private CharacterEquipment _characterEquipment { get; set; }
        private Guid _userGuid;
        private Guid? _interactGuid;
        private object _lock = new object();

        private Queue<long> _queueDamage;
        private ConcurrentDictionary<DateTime, long> _damageList;
        private ConcurrentDictionary<DateTime, long> _combatDamageList;

        private Queue<long> _queueHeal;
        private ConcurrentDictionary<DateTime, long> _healList;
        private ConcurrentDictionary<DateTime, long> _combatHealList;

        private LinkedList<long> _dpsMathList;

        public string Name { get; set; } = "Unknown";
        public string Guild { get; set; }
        public string Alliance { get; set; }
        [JsonIgnore]
        public long LastUpdate { get; private set; }
        public long Damage { get; set; }
        public long HighestDamage { get; set; }
        public long HighestHeal { get; set; }
        public long Heal { get; set; }

        [JsonIgnore]
        public long Dps { get
            {
                if (_dpsMathList.Count == 0) return 0;
                lock (this._lock)
                    return (long) _dpsMathList.Average();
            }
        }

        public PlayerGameObject(long objectId)
        {
            ObjectId = objectId;
            LastUpdate = DateTime.UtcNow.Ticks;
            _queueDamage = new Queue<long>();
            _queueHeal = new Queue<long>();
            _dpsMathList = new LinkedList<long>();
            DamageList = new ConcurrentDictionary<DateTime, long>();
            HealList = new ConcurrentDictionary<DateTime, long>();
            CombatDamageList = new ConcurrentDictionary<DateTime, long>();
            CombatHealList = new ConcurrentDictionary<DateTime, long>();
        }

        public void OnTimedEvent(DateTime utcNow)
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

                Damage += damage;
                Heal += healing;
                CombatDamageList.AddOrUpdate(utcNow, damage, (key, oldValue) => damage);
                CombatHealList.AddOrUpdate(utcNow, healing, (key, oldValue) => healing);

                LastUpdate = utcNow.Ticks;

                if (_dpsMathList.Count > 5)
                    _dpsMathList.RemoveFirst();
                _dpsMathList.AddLast(damage);

                if (damage > HighestDamage)
                    HighestDamage = damage;

                if (healing > HighestHeal)
                    HighestHeal = healing;
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

        public ConcurrentDictionary<DateTime, long> DamageList
        {
            get
            {
                lock (this._lock) return _damageList;
            }
            set
            {
                _damageList = value;
            }
        }

        public ConcurrentDictionary<DateTime, long> HealList
        {
            get
            {
                lock (this._lock) return _healList;
            }
            set
            {
                _healList = value;
            }
        }

        public ConcurrentDictionary<DateTime, long> CombatDamageList
        {
            get
            {
                lock (this._lock) return _combatDamageList;
            }
            set
            {
                _combatDamageList = value;
            }
        }

        public ConcurrentDictionary<DateTime, long> CombatHealList
        {
            get {
                lock (this._lock)
                    return _combatHealList;
            }
            set
            {
                _combatHealList = value;
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
                _damageList.AddOrUpdate(date, damage, (key, oldValue) => damage);
                _queueDamage.Enqueue(damage);
            }
        }

        public void AddHealing(DateTime date, long heal)
        {
            lock (this._lock)
            {
                _healList.AddOrUpdate(date, heal, (key, oldValue) => heal);
                _queueHeal.Enqueue(heal);
            }
        }

        public void Reset()
        {
            lock (this._lock)
            {
                Heal = 0;
                Damage = 0;
                HighestDamage = 0;
                CombatHealList.Clear();
                CombatDamageList.Clear();
                DamageList.Clear();
                HealList.Clear();
                _queueHeal.Clear();
                _queueDamage.Clear();
            }
        }
    }
}
