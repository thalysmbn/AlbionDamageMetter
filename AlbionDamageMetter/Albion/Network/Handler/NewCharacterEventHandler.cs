using AlbionDamageMetter.Albion.Enums;
using AlbionDamageMetter.Albion.Network.Events;

namespace AlbionDamageMetter.Albion.Network.Handler
{
    public class NewCharacterEventHandler
    {
        private readonly AlbionEntityData _albionEntityData;

        public NewCharacterEventHandler(AlbionEntityData albionEntityData)
        {
            _albionEntityData = albionEntityData;
        }

        public async Task OnActionAsync(NewCharacterEvent value)
        {
            if (value.Guid != null && value.ObjectId != null)
            {
                _albionEntityData.AddEntity((long)value.ObjectId, (Guid)value.Guid, null,
                    value.Name, value.GuildName, string.Empty, value.CharacterEquipment, GameObjectType.Player, GameObjectSubType.Player);
            }

            await Task.CompletedTask;
        }
    }
}
