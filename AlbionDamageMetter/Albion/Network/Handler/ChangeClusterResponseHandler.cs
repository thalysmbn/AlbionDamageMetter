using AlbionDamageMetter.Albion.Network.Response;

namespace AlbionDamageMetter.Albion.Network.Handler
{
    public class ChangeClusterResponseHandler
    {
        private readonly AlbionClusterData _albionClusterData;
        private readonly AlbionEntityData _albionEntityData;

        public ChangeClusterResponseHandler(AlbionClusterData albionClusterData, AlbionEntityData albionEntityData)
        {
            _albionClusterData = albionClusterData;
            _albionEntityData = albionEntityData;
        }

        public async Task OnActionAsync(ChangeClusterResponse value)
        {
            _albionClusterData.SaveHistory();
            _albionEntityData.ResetHistory();
            _albionClusterData.SetClusterInfo(value.MapType, value.Guid, value.Index, value.IslandName, value.WorldMapDataType, value.DungeonInformation, value.MainClusterIndex);
            await Task.CompletedTask;
        }
    }
}
