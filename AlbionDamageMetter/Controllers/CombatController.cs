using AlbionDamageMetter.Albion;
using AlbionDamageMetter.Albion.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlbionDamageMetter.Controllers
{
    [ApiController]
    [Route("api/combat", Name = "Combat")]
    public class CombatController : ControllerBase
    {
        private readonly AlbionEntityData _albionEntityData;

        public CombatController(AlbionEntityData albionEntityData)
        {
            _albionEntityData = albionEntityData;
        }

        [Route("damage")]
        public IList<DamageResultModel> Damage()
        {
            var list = new List<DamageResultModel>();
            var members = _albionEntityData.GetAllEntities(true);
            var selectOrderedMembers = members.Select(i => new { i.Value.Name, i.Value.Damage });
            foreach (var member in members)
            {
                list.Add(new DamageResultModel
                {
                    Name = member.Value.Name,
                    Data = member.Value.DamageList.Where(x => x.Key >= DateTime.UtcNow.AddMinutes(-3)).ToDictionary(x => x.Key, x => x.Value)
                });
            }
            return list.ToArray();
        }

        [Route("totalDamage")]
        public object[][] TotalDamage()
        {
            var list = new List<Object[]>();
            var members = _albionEntityData.GetAllEntities(true);
            var selectOrderedMembers = members.Select(i => new { i.Value.Name, i.Value.Damage });
            foreach (var member in members)
            {
                Object[] arrayUserData = { member.Value.Name, member.Value.Damage };
                list.Add(arrayUserData);

            }
            return list.ToArray();
        }

        [Route("totalHealing")]
        public Array TotalHealing()
        {
            var members = _albionEntityData.GetAllEntities(true);
            foreach (var member in members.Select(i => new { i.Value.Name, i.Value.Heal }))
            {

            }
            return members.Select(i => new { i.Value.Name, i.Value.Heal }).Distinct().OrderByDescending(x => x.Heal).ToArray();
        }
    }
}
