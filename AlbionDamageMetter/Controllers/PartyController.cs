using AlbionDamageMetter.Albion;
using AlbionDamageMetter.Albion.Models.NetworkModel;
using Microsoft.AspNetCore.Mvc;

namespace AlbionDamageMetter.Controllers
{
    [Route("api/party", Name = "Party")]
    public class PartyController : Controller
    {
        private readonly AlbionEntityData _albionEntityData;

        public PartyController(AlbionEntityData albionEntityData)
        {
            _albionEntityData = albionEntityData;
        }

        public List<KeyValuePair<Guid, PlayerGameObject>> Index()
        {
            return _albionEntityData.GetAllEntities(true);
        }
    }
}
