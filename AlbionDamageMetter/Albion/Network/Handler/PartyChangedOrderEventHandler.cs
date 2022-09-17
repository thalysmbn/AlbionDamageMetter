using AlbionDamageMetter.Albion.Network.Events;

namespace AlbionDamageMetter.Albion.Network.Handler
{
    public class PartyChangedOrderEventHandler
    {
        private readonly AlbionEntityData _albionEntityData;

        public PartyChangedOrderEventHandler(AlbionEntityData albionEntityData)
        {
            _albionEntityData = albionEntityData;
        }

        public async Task OnActionAsync(PartyChangedOrderEvent value)
        {
            _albionEntityData.SetParty(value.PartyUsersGuid, true);
            await Task.CompletedTask;
        }
    }
}
