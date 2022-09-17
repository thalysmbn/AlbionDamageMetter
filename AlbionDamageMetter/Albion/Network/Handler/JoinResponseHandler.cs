using AlbionDamageMetter.Albion.Enums;
using AlbionDamageMetter.Albion.Network.Response;

namespace AlbionDamageMetter.Albion.Network.Handler
{
    public class JoinResponseHandler
    {
        private readonly AlbionClusterData _albionClusterData;
        private readonly AlbionEntityData _albionEntityData;

        public JoinResponseHandler(AlbionClusterData _albionClusterData, AlbionEntityData albionEntityData)
        {
            _albionClusterData = _albionClusterData;
            _albionEntityData = albionEntityData;
        }

        public async Task OnActionAsync(JoinResponse value)
        {
            _albionClusterData.SetJoinClusterInfo(value.MapIndex, value.MainMapIndex);

            AddEntity(value.UserObjectId, value.UserGuid, value.InteractGuid,
                value.Username, value.GuildName, value.AllianceName, GameObjectType.Player, GameObjectSubType.LocalPlayer);

            await Task.CompletedTask;
        }

        private void AddEntity(long? userObjectId, Guid? guid, Guid? interactGuid, string name, string guild, string alliance, GameObjectType gameObjectType, GameObjectSubType gameObjectSubType)
        {
            if (guid == null || interactGuid == null || userObjectId == null)
            {
                return;
            }

            _albionEntityData.AddEntity((long)userObjectId, (Guid)guid, interactGuid, name, guild, alliance, null, GameObjectType.Player, GameObjectSubType.LocalPlayer);
            _albionEntityData.AddToParty((Guid)guid, name);
        }
    }
}
