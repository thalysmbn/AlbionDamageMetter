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
            var list = new List<object[]>();
            var members = _albionEntityData.GetAllEntities(true);
            var selectOrderedMembers = members.Select(i => new { i.Value.Name, i.Value.Damage, i.Value.Heal });
            foreach (var member in selectOrderedMembers)
            {
                list.Add(new object[] { member.Name, member.Damage });

            }
            return new PartyResultModel
            {
                HighestDamage = members.Count > 0 ? selectOrderedMembers.Max(x => x.Damage) : 0,
                HighestHeal = members.Count > 0 ? selectOrderedMembers.Max(x => x.Heal) : 0,
                DamageMember = list.ToArray(),
                Members = members
            };
        }
    }
}
