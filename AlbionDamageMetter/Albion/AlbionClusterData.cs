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

        public void SaveHistory(ClusterPartyHistoryDatabaseModel cluster)
        {
            var clusterHistory = LocalDatabaseJson.GetClusterPartyHistory();
            cluster.Entities = AlbionEntityData.GetAllEntities(true);
            cluster.CombatHistory = AlbionEntityData.GetCombatHistory();
            cluster.End = DateTime.UtcNow;
            clusterHistory.InsertOne(cluster);
        }

        public void SetClusterInfo(MapType mapType, Guid? mapGuid, string clusterIndex, string instanceName, string worldMapDataType, byte[] dungeonInformation, string mainClusterIndex)
        {
            CurrentCluster = new ClusterPartyHistoryDatabaseModel
            {
                Entered = DateTime.UtcNow,
                MapType = mapType,
                Guid = mapGuid,
                Index = clusterIndex,
                InstanceName = instanceName,
                WorldMapDataType = worldMapDataType,
                DungeonInformation = dungeonInformation,

                MainClusterIndex = mainClusterIndex,

                UniqueName = WorldData.GetUniqueNameOrNull(clusterIndex),
                UniqueClusterName = WorldData.GetUniqueNameOrDefault(clusterIndex) ?? instanceName ?? string.Empty,
                MapTypeString = GetMapTypeName(mapType)
            };
        }

        public void SetJoinClusterInfo(string index, string mainClusterIndex)
        {
            CurrentCluster.Index ??= index;
            CurrentCluster.WorldJsonType = WorldData.GetWorldJsonTypeByIndex(index) ?? WorldData.GetWorldJsonTypeByIndex(mainClusterIndex) ?? string.Empty;
            CurrentCluster.File = WorldData.GetFileByIndex(index) ?? WorldData.GetFileByIndex(mainClusterIndex) ?? string.Empty;
            
            CurrentCluster.Tier = GetTier(CurrentCluster.File);
            CurrentCluster.ClusterMode = GetClusterType(CurrentCluster.WorldJsonType);
            CurrentCluster.AvalonTunnelType = GetTunnelType(CurrentCluster.WorldJsonType);

            if (CurrentCluster.ClusterMode is ClusterMode.Black or ClusterMode.Red or ClusterMode.Yellow or ClusterMode.SafeArea && CurrentCluster.MapType is MapType.Unknown)
            {
                CurrentCluster.ClusterHistoryString1 = ClusterModeString(CurrentCluster.ClusterMode);
                CurrentCluster.ClusterHistoryString2 = CurrentCluster.UniqueName;
                CurrentCluster.ClusterHistoryString3 = string.Empty;
            }
            
            if (CurrentCluster.MapType is MapType.Arena or MapType.CorruptedDungeon or MapType.RandomDungeon or MapType.Expedition or MapType.HellGate or MapType.Mists)
            {
                CurrentCluster.ClusterHistoryString1 = CurrentCluster.MapTypeString;
                CurrentCluster.ClusterHistoryString2 = string.Empty;
                CurrentCluster.ClusterHistoryString3 = string.Empty;
            }

            if (CurrentCluster.MapType is MapType.Island or MapType.Hideout)
            {
                CurrentCluster.ClusterHistoryString1 = ClusterModeString(CurrentCluster.ClusterMode);
                CurrentCluster.ClusterHistoryString2 = CurrentCluster.InstanceName;
                CurrentCluster.ClusterHistoryString3 = string.Empty;
            }

            if (CurrentCluster.ClusterMode is ClusterMode.AvalonTunnel)
            {
                CurrentCluster.ClusterHistoryString1 = ClusterModeString(CurrentCluster.ClusterMode);
                CurrentCluster.ClusterHistoryString2 = CurrentCluster.UniqueName;
                CurrentCluster.ClusterHistoryString3 = CurrentCluster.AvalonTunnelType.ToString();
            }
        }

        private static string ClusterModeString(ClusterMode clusterMode)
        {
            return clusterMode switch
            {
                ClusterMode.Island => "ISLAND",
                ClusterMode.AvalonTunnel => "AVALON_ROAD",
                ClusterMode.Black => "BLACK_ZONE",
                ClusterMode.Red => "RED_ZONE",
                ClusterMode.Yellow => "YELLOW_ZONE",
                ClusterMode.SafeArea => "SAFE_AREA",
                ClusterMode.Mists => "MISTS",
                ClusterMode.Unknown => "UNKNOWN_ZONE",
                _ => ""
            };
        }

        private static bool IsAvalonClusterTunnelByType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return false;
            }

            return type.ToUpper().Contains("TUNNEL_BLACK_LOW")
                   || type.ToUpper().Contains("TUNNEL_BLACK_MEDIUM")
                   || type.ToUpper().Contains("TUNNEL_BLACK_HIGH")
                   || type.ToUpper().Contains("TUNNEL_LOW")
                   || type.ToUpper().Contains("TUNNEL_MEDIUM")
                   || type.ToUpper().Contains("TUNNEL_HIGH")
                   || type.ToUpper().Contains("TUNNEL_DEEP")
                   || type.ToUpper().Contains("TUNNEL_ROYAL")
                   || type.ToUpper().Contains("TUNNEL_HIDEOUT");
        }

        private static Tier GetTier(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Tier.Unknown;
            }

            if (value.Contains("_T1_"))
            {
                return Tier.T1;
            }

            if (value.Contains("_T2_"))
            {
                return Tier.T2;
            }

            if (value.Contains("_T3_"))
            {
                return Tier.T3;
            }

            if (value.Contains("_T4_"))
            {
                return Tier.T4;
            }

            if (value.Contains("_T5_"))
            {
                return Tier.T5;
            }

            if (value.Contains("_T6_"))
            {
                return Tier.T6;
            }

            if (value.Contains("_T7_"))
            {
                return Tier.T7;
            }

            if (value.Contains("_T8_"))
            {
                return Tier.T8;
            }

            return Tier.Unknown;
        }

        private static ClusterMode GetClusterType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return ClusterMode.Unknown;
            }

            if (IsAvalonClusterTunnelByType(type))
            {
                return ClusterMode.AvalonTunnel;
            }

            if (type.ToUpper().Contains("SAFEAREA") || type.ToUpper().Equals("HIDEOUT") || type.ToUpper().Contains("PLAYERCITY"))
            {
                return ClusterMode.SafeArea;
            }

            if (type.ToUpper().Contains("ISLAND"))
            {
                return ClusterMode.Island;
            }

            if (type.ToUpper().Contains("YELLOW"))
            {
                return ClusterMode.Yellow;
            }

            if (type.ToUpper().Contains("RED"))
            {
                return ClusterMode.Red;
            }

            if (type.ToUpper().Contains("BLACK"))
            {
                return ClusterMode.Black;
            }

            if (type.ToUpper().Contains("MISTS"))
            {
                return ClusterMode.Mists;
            }

            return ClusterMode.Unknown;
        }

        private static AvalonTunnelType GetTunnelType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return AvalonTunnelType.Unknown;
            }

            if (type.ToUpper().Contains("TUNNEL_BLACK_LOW")) return AvalonTunnelType.TunnelBlackLow;

            if (type.ToUpper().Contains("TUNNEL_BLACK_MEDIUM")) return AvalonTunnelType.TunnelBlackMedium;

            if (type.ToUpper().Contains("TUNNEL_BLACK_HIGH")) return AvalonTunnelType.TunnelBlackHigh;

            if (type.ToUpper().Contains("TUNNEL_LOW")) return AvalonTunnelType.TunnelLow;

            if (type.ToUpper().Contains("TUNNEL_MEDIUM")) return AvalonTunnelType.TunnelMedium;

            if (type.ToUpper().Contains("TUNNEL_HIGH")) return AvalonTunnelType.TunnelHigh;

            if (type.ToUpper().Contains("TUNNEL_DEEP")) return AvalonTunnelType.TunnelDeep;

            if (type.ToUpper().Contains("TUNNEL_ROYAL")) return AvalonTunnelType.TunnelRoyal;

            if (type.ToUpper().Contains("TUNNEL_HIDEOUT")) return AvalonTunnelType.TunnelHideout;

            return AvalonTunnelType.Unknown;
        }

        private static string GetMapTypeName(MapType mapType)
        {
            return mapType switch
            {
                MapType.RandomDungeon => "DUNGEON",
                MapType.HellGate => "HELL_GATE",
                MapType.CorruptedDungeon => "CORRUPTED_DUNGEON",
                MapType.Island => "ISLAND",
                MapType.Hideout => "HIDEOUT",
                MapType.Expedition => "EXPEDITION",
                MapType.Arena => "ARENA",
                MapType.Mists => "MISTS",
                _ => ""
            };
        }
    }
}
