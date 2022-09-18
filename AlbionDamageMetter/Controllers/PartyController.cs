using AlbionDamageMetter.Albion;
using AlbionDamageMetter.Albion.Models;
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

        public PartyResultModel Index()
        {
            var members = _albionEntityData.GetAllEntities(true);
            return new PartyResultModel
            {
                HighestDamage = members.Count > 0 ? members.Max(x => x.Value.Damage) : 0,
                HighestHeal = members.Count > 0 ? members.Max(x => x.Value.Heal) : 0,
                Members = members
            };
        }
    }
}
