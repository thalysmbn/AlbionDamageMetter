using AlbionDamageMetter.Albion.Network.Events;

namespace AlbionDamageMetter.Albion.Network.Handler
{
    public class PartyPlayerJoinedEventHandler
    {
        private readonly AlbionEntityData _albionEntityData;

        public PartyPlayerJoinedEventHandler(AlbionEntityData albionEntityData)
        {
            _albionEntityData = albionEntityData;
        }

        public async Task OnActionAsync(PartyPlayerJoinedEvent value)
        {
            if (value?.UserGuid != null)
            {
                _albionEntityData.AddToParty((Guid)value.UserGuid, value.Username);
            }
            await Task.CompletedTask;
        }
    }
}
