using AlbionDamageMetter.Albion.Network.Events;

namespace AlbionDamageMetter.Albion.Network.Handler
{
    public class PartyPlayerJoinedEventHandler
    {
        public async Task OnActionAsync(PartyPlayerJoinedEvent value)
        {
            if (value?.UserGuid == null) return;
        }
    }
}
