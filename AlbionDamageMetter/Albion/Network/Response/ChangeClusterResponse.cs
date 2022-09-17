using AlbionDamageMetter.Albion.Enums;

namespace AlbionDamageMetter.Albion.Network.Response
{
    public class ChangeClusterResponse
    {
        public string Index { get; }
        public Guid? Guid { get; }
        public MapType MapType = MapType.Unknown;
        public string WorldMapDataType { get; }
        public string IslandName { get; }
        public byte[] DungeonInformation { get; }
        public string MainClusterIndex { get; }

        public ChangeClusterResponse(Dictionary<byte, object> parameters)
        {
            if (parameters.ContainsKey(0))
            {
                var clusterString = string.IsNullOrEmpty(parameters[0].ToString()) ? string.Empty : parameters[0].ToString();
                var splitName = clusterString?.Split(new[] { "@" }, StringSplitOptions.RemoveEmptyEntries);

                if (splitName?.Length > 1 && clusterString.ToLower().Contains('@'))
                {
                    Guid = AlbionWorldData.GetMapGuid(clusterString);
                    MapType = AlbionWorldData.GetMapType(clusterString);

                    if (MapType is MapType.Hideout && splitName.Length >= 3)
                    {
                        MainClusterIndex = string.IsNullOrEmpty(splitName[1]) ? string.Empty : splitName[1];
                    }
                }
                else
                {
                    Index = clusterString;
                }
            }

            if (parameters.ContainsKey(1))
            {
                WorldMapDataType = string.IsNullOrEmpty(parameters[1].ToString()) ? string.Empty : parameters[1].ToString();
            }

            if (parameters.ContainsKey(2))
            {
                IslandName = string.IsNullOrEmpty(parameters[2].ToString()) ? string.Empty : parameters[2].ToString();
            }

            if (parameters.ContainsKey(3))
            {
                DungeonInformation = ((byte[])parameters[3]).ToArray();
            }
        }
    }
}
