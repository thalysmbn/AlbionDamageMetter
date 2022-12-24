using AlbionDamageMetter.Albion.Enums;

namespace AlbionDamageMetter.Albion.Models
{
    public class ClusterModel
    {
        public DateTime Entered { get; set; }
        public DateTime End { get; set; }
        public MapType MapType { get; set; }
        public Guid? Guid { get; set; }
        public string Index { get; set; }
        public string InstanceName { get; set; }
        public string WorldMapDataType { get; set; }
        public byte[] DungeonInformation { get; set; }
        public string MainClusterIndex { get; set; }

        public string WorldJsonType { get; set; }
        public string File { get; set; }

        public string UniqueName { get; set; }
        public string UniqueClusterName { get; set; }

        public ClusterMode ClusterMode { get; set; }
        public AvalonTunnelType AvalonTunnelType { get; set; }
        public Tier Tier { get; set; }

        public string MapTypeString { get; set; }

        public string ClusterHistoryString1 { get; set; }
        public string ClusterHistoryString2 { get; set; }
        public string ClusterHistoryString3 { get; set; }
        public LinkedList<HealthUpdateModel> CombatHistory { get; set; }

        public IList<DamageResultModel> DamageResult { get; set; }

        public IList<DamageResultModel> HealResult { get; set; }

        public object[][] TotalDamageList { get; set; }

        public object[][] HighestDamageList { get; set; }

        public object[][] TotalHealingList { get; set; }

        public int Entities { get; set; }
    }
}
