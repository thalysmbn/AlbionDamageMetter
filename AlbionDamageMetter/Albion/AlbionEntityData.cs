using AlbionDamageMetter.Albion.Enums;
using AlbionDamageMetter.Albion.Models.NetworkModel;
using AlbionDamageMetter.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace AlbionDamageMetter.Albion
{
    public class AlbionEntityData
    {
        public ConcurrentDictionary<long, double> LastPlayersHealth = new();
        private readonly ConcurrentDictionary<Guid, PlayerGameObject> _knownEntities = new();
        private readonly ConcurrentDictionary<Guid, string> _knownPartyEntities = new();

        public void AddEntity(long objectId, Guid userGuid, Guid? interactGuid, string name, string guild, string alliance,
            CharacterEquipment characterEquipment, GameObjectType objectType, GameObjectSubType objectSubType)
        {
            PlayerGameObject gameObject;

            if (_knownEntities.TryRemove(userGuid, out var oldEntity))
            {
                gameObject = new PlayerGameObject(objectId)
                {
                    Name = name,
                    ObjectType = objectType,
                    UserGuid = userGuid,
                    Guild = guild,
                    Alliance = alliance,
                    InteractGuid = interactGuid,
                    ObjectSubType = objectSubType,
                    CharacterEquipment = characterEquipment ?? oldEntity.CharacterEquipment,
                };
            }
            else
            {
                gameObject = new PlayerGameObject(objectId)
                {
                    Name = name,
                    ObjectType = objectType,
                    UserGuid = userGuid,
                    Guild = guild,
                    Alliance = alliance,
                    InteractGuid = interactGuid,
                    ObjectSubType = objectSubType,
                    CharacterEquipment = characterEquipment
                };
            }

            _knownEntities.TryAdd(gameObject.UserGuid, gameObject);
        }

        public void SetParty(Dictionary<Guid, string> party, bool resetPartyBefore = false)
        {
            if (resetPartyBefore)
            {
                ResetPartyMember();
            }

            foreach (var member in party)
            {
                AddToParty(member.Key, member.Value);
            }
        }

        public KeyValuePair<Guid, PlayerGameObject>? GetEntity(long objectId)
        {
            return _knownEntities?.FirstOrDefault(x => x.Value.ObjectId == objectId);
        }

        public KeyValuePair<Guid, PlayerGameObject>? GetEntity(string uniqueName)
        {
            return _knownEntities?.FirstOrDefault(x => x.Value.Name == uniqueName);
        }

        public List<KeyValuePair<Guid, PlayerGameObject>> GetAllEntities(bool onlyInParty = false)
        {
            return new List<KeyValuePair<Guid, PlayerGameObject>>(onlyInParty ? _knownEntities.ToArray().Where(x => IsEntityInParty(x.Value.Name)) : _knownEntities.ToArray());
        }

        public void AddToParty(Guid guid, string username)
        {
            if (_knownPartyEntities.All(x => x.Key != guid))
            {
                _knownPartyEntities.TryAdd(guid, username);
            }
        }

        public void RemoveFromParty(Guid? guid)
        {
            if (guid is { } notNullGuid)
            {
                if (notNullGuid == GetLocalEntity()?.Key)
                {
                    ResetPartyMember();
                    AddLocalEntityToParty();
                }
                else
                {
                    _ = _knownPartyEntities.TryRemove(notNullGuid, out _);
                }
            }
        }

        public void ResetPartyMember()
        {
            _knownPartyEntities.Clear();
        }

        public void AddLocalEntityToParty()
        {
            foreach (var member in _knownEntities.Where(x => x.Value.ObjectSubType == GameObjectSubType.LocalPlayer))
            {
                _knownPartyEntities.TryAdd(member.Key, member.Value.Name);
            }
        }

        public bool IsEntityInParty(string name)
        {
            return _knownPartyEntities.Any(x => x.Value == name);
        }

        public bool IsEntityInParty(long objectId)
        {
            var entity = _knownEntities.FirstOrDefault(x => x.Value.ObjectId == objectId);
            return entity.Value != null && _knownPartyEntities.Any(x => x.Value == entity.Value.Name);
        }

        public bool IsEntityInParty(Guid guid)
        {
            return _knownPartyEntities.Any(x => x.Key == guid);
        }

        public KeyValuePair<Guid, PlayerGameObject>? GetLocalEntity() => _knownEntities?.ToArray().FirstOrDefault(x => x.Value.ObjectSubType == GameObjectSubType.LocalPlayer);

        public void AddDamage(long objectId, long causerId, double healthChange, double newHealthValue)
        {
            var gameObject = GetEntity(causerId);
            var gameObjectValue = gameObject?.Value;

            if (gameObject?.Value == null
                || gameObject.Value.Value?.ObjectType != GameObjectType.Player
                || !IsEntityInParty(gameObject.Value.Value.Name)
                )
            {
                return;
            }

            if (GetHealthChangeType(healthChange) == HealthChangeType.Damage)
            {
                var damageChangeValue = (int)Math.Round(healthChange.ToPositiveFromNegativeOrZero(), MidpointRounding.AwayFromZero);
                if (damageChangeValue <= 0)
                {
                    return;
                }

                gameObject.Value.Value.Damage += damageChangeValue;
            }

            if (GetHealthChangeType(healthChange) == HealthChangeType.Heal)
            {
                var healChangeValue = healthChange;
                if (healChangeValue <= 0)
                {
                    return;
                }

                if (IsMaxHealthReached(objectId, newHealthValue))
                {
                    return;
                }

                gameObject.Value.Value.Heal += (int)Math.Round(healChangeValue, MidpointRounding.AwayFromZero);
            }

            gameObjectValue.CombatStart ??= DateTime.UtcNow;
        }

        public bool IsMaxHealthReached(long objectId, double newHealthValue)
        {
            var playerHealth = LastPlayersHealth?.ToArray().FirstOrDefault(x => x.Key == objectId);
            if (playerHealth?.Value.CompareTo(newHealthValue) == 0)
            {
                return true;
            }

            SetLastPlayersHealth(objectId, newHealthValue);
            return false;
        }

        private void SetLastPlayersHealth(long key, double value)
        {
            if (LastPlayersHealth.ContainsKey(key))
            {
                LastPlayersHealth[key] = value;
            }
            else
            {
                try
                {
                    LastPlayersHealth.TryAdd(key, value);
                }
                catch (Exception e)
                {
                }
            }
        }

        private static HealthChangeType GetHealthChangeType(double healthChange) => healthChange <= 0 ? HealthChangeType.Damage : HealthChangeType.Heal;
    }
}
