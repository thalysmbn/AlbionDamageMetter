using AlbionDamageMetter.Albion.Network.Response;

namespace AlbionDamageMetter.Albion.Network.Handler
{
    public class ChangeClusterResponseHandler
    {
        private readonly AlbionClusterData _albionClusterData;

        public ChangeClusterResponseHandler(AlbionClusterData clusterDataController)
        {
            _albionClusterData = clusterDataController;
        }

        public async Task OnActionAsync(ChangeClusterResponse value)
        {
            _albionClusterData.SetClusterInfo(value.MapType, value.Guid, value.Index, value.IslandName, value.WorldMapDataType, value.DungeonInformation, value.MainClusterIndex);
            await Task.CompletedTask;
        }
    }
}
