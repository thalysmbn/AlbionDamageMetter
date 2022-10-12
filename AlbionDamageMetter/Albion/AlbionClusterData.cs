using AlbionDamageMetter.Albion.Enums;
using AlbionDamageMetter.Albion.Models.Database;
using AlbionDamageMetter.Albion.Models.NetworkModel;
using AlbionDamageMetter.Albion.Network.Events;
using AlbionDamageMetter.Services;

namespace AlbionDamageMetter.Albion
{
    public class AlbionClusterData
    {
        public bool ClusterInfoFullyAvailable { get; set; }

        public ClusterPartyHistoryDatabaseModel CurrentCluster { get; private set; } = new ClusterPartyHistoryDatabaseModel();

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
            CurrentCluster.Entities = AlbionEntityData.GetAllEntities(true);
            CurrentCluster.CombatHistory = AlbionEntityData.GetCombatHistory();
            clusterHistory.InsertOne(CurrentCluster);

        }

        public void SetClusterInfo(MapType mapType, Guid? mapGuid, string clusterIndex, string instanceName, string worldMapDataType, byte[] dungeonInformation, string mainClusterIndex)
        {
            CurrentCluster = new ClusterPartyHistoryDatabaseModel
            {
                Entered = DateTime.Now,
                MapType = mapType,
                Guid = mapGuid,
                Index = clusterIndex,
                InstanceName = instanceName,
                WorldMapDataType = worldMapDataType,
                DungeonInformation = dungeonInformation,
                MainClusterIndex = mainClusterIndex
            };
        }

        public void SetJoinClusterInfo(string index, string mainClusterIndex)
        {
            CurrentCluster.Index = index;
            CurrentCluster.MainClusterIndex = mainClusterIndex;
            ClusterInfoFullyAvailable = true;
        }
    }
}
