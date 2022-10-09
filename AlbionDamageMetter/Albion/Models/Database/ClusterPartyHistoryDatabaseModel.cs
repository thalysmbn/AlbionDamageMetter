using AlbionDamageMetter.Albion.Enums;
using AlbionDamageMetter.Albion.Models.NetworkModel;
using AlbionDamageMetter.Albion.Network.Events;
using System.Collections.Concurrent;

namespace AlbionDamageMetter.Albion.Models.Database
{
    public class ClusterPartyHistoryDatabaseModel
    {
        public DateTime Entered { get; set; }
        public MapType MapType { get; set; }
        public Guid? Guid { get; set; }
        public string Index { get; set; }
        public string InstanceName { get; set; }
        public string WorldMapDataType { get; set; }
        public byte[] DungeonInformation { get; set; }
        public string MainClusterIndex { get; set; }
        public List<KeyValuePair<Guid, PlayerGameObject>> Entities { get; set; }
        public HealthUpdateEvent[] CombatHistory { get; set; }
    }
}
