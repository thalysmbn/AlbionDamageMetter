using AlbionDamageMetter.Albion.Network.Events;

namespace AlbionDamageMetter.Albion.Network.Handler
{
    public class PartyPlayerLeftEventHandler
    {
        private readonly AlbionEntityData _albionEntityData;

        public PartyPlayerLeftEventHandler(AlbionEntityData albionEntityData)
        {
            _albionEntityData = albionEntityData;
        }

        public async Task OnActionAsync(PartyPlayerLeftEvent value)
        {
            _albionEntityData.RemoveFromParty(value.UserGuid);
            await Task.CompletedTask;
        }
    }
}
