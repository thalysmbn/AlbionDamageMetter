using AlbionDamageMetter.Albion.Network.Events;

namespace AlbionDamageMetter.Albion.Network.Handler
{
    public class PartyDisbandedEventHandler
    {
        private readonly AlbionEntityData _albionEntityData;

        public PartyDisbandedEventHandler(AlbionEntityData albionEntityData)
        {
            _albionEntityData = albionEntityData;
        }
        public async Task OnActionAsync(PartyDisbandedEvent value)
        {
            _albionEntityData.ResetPartyMember();
            _albionEntityData.AddLocalEntityToParty();
            await Task.CompletedTask;
        }
    }
}
