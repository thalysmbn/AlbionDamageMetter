using AlbionDamageMetter.Albion.Network.Response;

namespace AlbionDamageMetter.Albion.Network.Handler
{
    public class ChangeClusterResponseHandler
    {
        private readonly AlbionClusterData _clusterDataController;

        public ChangeClusterResponseHandler(AlbionClusterData clusterDataController)
        {
            _clusterDataController = clusterDataController;
        }

        public async Task OnActionAsync(ChangeClusterResponse value)
        {
            _clusterDataController.SetClusterInfo(value.MapType, value.Guid, value.Index, value.IslandName, value.WorldMapDataType, value.DungeonInformation, value.MainClusterIndex);
            await Task.CompletedTask;
        }
    }
}
