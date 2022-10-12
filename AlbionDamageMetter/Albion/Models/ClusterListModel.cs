using AlbionDamageMetter.Albion.Enums;

namespace AlbionDamageMetter.Albion.Models
{
    public class ClusterListModel
    {
        public DateTime Entered { get; set; }
        public MapType MapType { get; set; }
        public Guid? Guid { get; set; }
        public string Index { get; set; }
        public string InstanceName { get; set; }
        public string WorldMapDataType { get; set; }
        public byte[] DungeonInformation { get; set; }
        public string MainClusterIndex { get; set; }
    }
}
