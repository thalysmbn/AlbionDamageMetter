using AlbionDamageMetter.Albion.Enums;
using AlbionDamageMetter.Albion.Models.Database;
using AlbionDamageMetter.Services;

namespace AlbionDamageMetter.Albion
{
    public class AlbionClusterData
    {
        public bool ClusterInfoFullyAvailable { get; set; }

        // Change cluster data
        public DateTime Entered { get; private set; }
        public MapType MapType { get; private set; } = MapType.Unknown;
        public Guid? Guid { get; private set; }
        public string Index { get; private set; }
        public string InstanceName { get; private set; }
        public string WorldMapDataType { get; private set; }
        public byte[] DungeonInformation { get; private set; }

        // Join data
        public string MainClusterIndex { get; private set; }

        private AlbionEntityData AlbionEntityData { get; }
        private LocalDatabaseJson LocalDatabaseJson { get; }

        public AlbionClusterData(LocalDatabaseJson localDatabaseJson, AlbionEntityData albionEntityData)
        {
            LocalDatabaseJson = localDatabaseJson;
            AlbionEntityData = albionEntityData;
        }

        public void SaveHistory()
        {
            var clusterHistory = LocalDatabaseJson.GetClusterPartyHistory();
            clusterHistory.InsertOne(new ClusterPartyHistoryDatabaseModel
            {
                Entered = Entered,
                MapType = MapType,
                Guid = Guid,
                Index = Index,
                InstanceName = InstanceName,
                WorldMapDataType = WorldMapDataType,
                DungeonInformation = DungeonInformation,
                MainClusterIndex = MainClusterIndex,
                Entities = AlbionEntityData.GetAllEntities(true),
                CombatHistory = AlbionEntityData.GetCombatHistory()
            });
        }

        public void SetClusterInfo(MapType mapType, Guid? mapGuid, string clusterIndex, string instanceName, string worldMapDataType, byte[] dungeonInformation, string mainClusterIndex)
        {
            Entered = DateTime.Now;
            MapType = mapType;
            Guid = mapGuid;
            Index = clusterIndex;
            InstanceName = instanceName;
            WorldMapDataType = worldMapDataType;
            DungeonInformation = dungeonInformation;

            MainClusterIndex = mainClusterIndex;
        }

        public void SetJoinClusterInfo(string index, string mainClusterIndex)
        {
            Index ??= index;
            MainClusterIndex ??= mainClusterIndex;
            ClusterInfoFullyAvailable = true;
        }
    }
}
