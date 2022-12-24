using AlbionDamageMetter.Albion.Enums;
using AlbionDamageMetter.Albion.Network.Events;

namespace AlbionDamageMetter.Albion.Network.Handler
{
    public class HealthUpdateEventHandler
    {
        private readonly AlbionEntityData _albionEntityData;

        public HealthUpdateEventHandler(AlbionEntityData albionEntityData)
        {
            _albionEntityData = albionEntityData;
        }

        public async Task OnActionAsync(HealthUpdateEvent value)
        {
            _albionEntityData.AddHistory(value);
            _albionEntityData.AddDamage(value.ObjectId, value.CauserId, value.HealthChange, value.NewHealthValue);
            await Task.CompletedTask;
        }
    }
}
