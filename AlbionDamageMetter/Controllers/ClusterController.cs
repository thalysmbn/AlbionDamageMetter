using AlbionDamageMetter.Albion;
using AlbionDamageMetter.Albion.Models;
using AlbionDamageMetter.Albion.Models.Database;
using AlbionDamageMetter.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlbionDamageMetter.Controllers
{
    [Route("api/cluster", Name = "Cluster")]
    public class ClusterController : Controller
    {
        private readonly AlbionClusterData _albionClusterData;
        private readonly LocalDatabaseJson _localDatabaseJson;

        public ClusterController(AlbionClusterData albionClusterData, LocalDatabaseJson localDatabaseJson)
        {
            _albionClusterData = albionClusterData;
            _localDatabaseJson = localDatabaseJson;
        }

        public ClusterPartyHistoryDatabaseModel Index()
        {
            return _albionClusterData.CurrentCluster;
        }

        [Route("clusters")]
        public List<ClusterListModel> Clusters()
        {
            var clusterParties = _localDatabaseJson.GetClusterPartyHistory();
            return clusterParties.AsQueryable().Select(x => new ClusterListModel
            {
                Entered = x.Entered,
                MapType = x.MapType,
                Guid = x.Guid,
                Index = x.Index,
                InstanceName = x.InstanceName,
                WorldMapDataType = x.WorldMapDataType,
                DungeonInformation = x.DungeonInformation,
                MainClusterIndex = x.MainClusterIndex
            }).OrderByDescending(x => x.Entered).ToList();
        }

        [Route("cluster")]
        public ClusterPartyHistoryDatabaseModel Cluster(string guid)
        {
            var clusterParties = _localDatabaseJson.GetClusterPartyHistory();
            if (clusterParties == null) return new();
            if (Guid.TryParse(guid, out Guid _guid))
                return clusterParties.Find(x => x.Guid == _guid).First();
            return new();
        }
    }
}
