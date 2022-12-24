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

        public ClusterListModel Index()
        {
            var currentcluster = _albionClusterData.CurrentCluster;
            return new ClusterListModel
            {
                Entered = currentcluster.Entered,
                End = currentcluster.End,
                MapType = currentcluster.MapType,
                Guid = currentcluster.Guid,
                Index = currentcluster.Index,
                InstanceName = currentcluster.InstanceName,
                WorldMapDataType = currentcluster.WorldMapDataType,
                DungeonInformation = currentcluster.DungeonInformation,
                MainClusterIndex = currentcluster.MainClusterIndex,
                WorldJsonType = currentcluster.WorldJsonType,
                File = currentcluster.File,
                UniqueName = currentcluster.UniqueName,
                UniqueClusterName = currentcluster.UniqueClusterName,
                ClusterMode = currentcluster.ClusterMode,
                AvalonTunnelType = currentcluster.AvalonTunnelType,
                Tier = currentcluster.Tier,
                MapTypeString = currentcluster.MapTypeString,
                ClusterHistoryString1 = currentcluster.ClusterHistoryString1,
                ClusterHistoryString2 = currentcluster.ClusterHistoryString2,
                ClusterHistoryString3 = currentcluster.ClusterHistoryString3
            };
        }

        [Route("clusters")]
        public List<ClusterListModel> Clusters()
        {
            var clusterParties = _localDatabaseJson.GetClusterPartyHistory();
            return clusterParties.AsQueryable().Select(x => new ClusterListModel
            {
                Entered = x.Entered,
                End = x.End,
                MapType = x.MapType,
                Guid = x.Guid,
                Index = x.Index,
                InstanceName = x.InstanceName,
                WorldMapDataType = x.WorldMapDataType,
                DungeonInformation = x.DungeonInformation,
                MainClusterIndex = x.MainClusterIndex,
                WorldJsonType = x.WorldJsonType,
                File = x.File,
                UniqueName = x.UniqueName,
                UniqueClusterName = x.UniqueClusterName,
                ClusterMode = x.ClusterMode,
                AvalonTunnelType = x.AvalonTunnelType,
                Tier = x.Tier,
                MapTypeString = x.MapTypeString,
                ClusterHistoryString1 = x.ClusterHistoryString1,
                ClusterHistoryString2 = x.ClusterHistoryString2,
                ClusterHistoryString3 = x.ClusterHistoryString3,
                Entities = x.Entities.Count,
                CombatLog = x.CombatHistory.Count
            }).OrderByDescending(x => x.Entered).ToList();
        }

        [HttpGet("data/{entered}")]
        public ClusterModel Cluster(string entered)
        {
            var clusterParties = _localDatabaseJson.GetClusterPartyHistory();
            if (clusterParties == null) return new();
            if (!DateTime.TryParse(entered, out DateTime _entered))
                return new();

            var data = clusterParties.Find(x => x.Entered == _entered).First();
            var members = data.Entities;

            var damageResultList = new List<DamageResultModel>();
            foreach (var member in members)
            {
                damageResultList.Add(new DamageResultModel
                {
                    Name = member.Value.Name,
                    Data = member.Value.CombatDamageList.ToDictionary(x => x.Key, x => x.Value)
                });
            }

            var healResultList = new List<DamageResultModel>();
            foreach (var member in members)
            {
                healResultList.Add(new DamageResultModel
                {
                    Name = member.Value.Name,
                    Data = member.Value.CombatHealList.ToDictionary(x => x.Key, x => x.Value)
                });
            }

            var totalDamageList = new List<Object[]>();
            var totalDamage = members.Sum(x => x.Value.Damage);
            var selectOrderedMembersDamage = members.Select(i => new { i.Value.Name, i.Value.Damage }).OrderByDescending(x => x.Damage);
            foreach (var member in selectOrderedMembersDamage)
            {
                var mathPercentage = member.Damage == 0 ? 0 : (int)(0.5f + ((100f * member.Damage) / totalDamage));
                totalDamageList.Add(new object[] { $"{member.Name} ( {mathPercentage}% )", member.Damage });

            }

            var totalHighestDamageList = new List<Object[]>();
            var totalHighestDamage = members.Sum(x => x.Value.HighestDamage);
            var selectOrderedMembersHighestDamage = members.Select(i => new { i.Value.Name, i.Value.HighestDamage }).OrderByDescending(x => x.HighestDamage);
            foreach (var member in selectOrderedMembersHighestDamage)
            {
                var mathPercentage = member.HighestDamage == 0 ? 0 : (int)(0.5f + ((100f * member.HighestDamage) / totalHighestDamage));
                totalHighestDamageList.Add(new object[] { $"{member.Name} ( {mathPercentage}% )", mathPercentage });

            }

            var totalHealList = new List<Object[]>();
            var totalHeal = members.Sum(x => x.Value.Heal);
            var selectOrderedMembersHeal = members.Select(i => new { i.Value.Name, i.Value.Heal }).OrderByDescending(x => x.Heal);
            foreach (var member in selectOrderedMembersHeal)
            {
                var mathPercentage = member.Heal == 0 ? 0 : (int)(0.5f + ((100f * member.Heal) / totalHeal));
                totalHealList.Add(new object[] { $"{member.Name} ( {mathPercentage}% )", member.Heal });
            }

            return new ClusterModel
            {
                Entered = data.Entered,
                End = data.End,
                MapType = data.MapType,
                Guid = data.Guid,
                Index = data.Index,
                InstanceName = data.InstanceName,
                WorldMapDataType = data.WorldMapDataType,
                DungeonInformation = data.DungeonInformation,
                MainClusterIndex = data.MainClusterIndex,
                WorldJsonType = data.WorldJsonType,
                File = data.File,
                UniqueName = data.UniqueName,
                UniqueClusterName = data.UniqueClusterName,
                ClusterMode = data.ClusterMode,
                AvalonTunnelType = data.AvalonTunnelType,
                Tier = data.Tier,
                MapTypeString = data.MapTypeString,
                ClusterHistoryString1 = data.ClusterHistoryString1,
                ClusterHistoryString2 = data.ClusterHistoryString2,
                ClusterHistoryString3 = data.ClusterHistoryString3,
                CombatHistory = data.CombatHistory,
                DamageResult = damageResultList,
                HealResult = healResultList,
                TotalDamageList = totalDamageList.ToArray(),
                TotalHealingList = totalHealList.ToArray(),
                HighestDamageList = totalHighestDamageList.ToArray(),
                Entities = data.Entities.Count
            };
        }
    }
}
