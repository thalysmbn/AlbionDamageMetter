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
            Console.WriteLine("ChangeClusterResponseHandler");

            await Task.Run(() =>
            {
                if (_albionClusterData.ClusterInfoFullyAvailable)
                {
                    if (_albionEntityData.GetCombatHistory().Count > 0)
                        _albionClusterData.SaveHistory(_albionClusterData.CurrentCluster);
                    _albionEntityData.ResetHistory();
                }
            });

            _albionClusterData.ClusterInfoFullyAvailable = false;

            _albionClusterData.SetClusterInfo(value.MapType, value.Guid, value.Index, value.IslandName, value.WorldMapDataType, value.DungeonInformation, value.MainClusterIndex);
            
            await Task.CompletedTask;
        }
    }
}
