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
            foreach (var member in members)
            {
                list.Add(new DamageResultModel
                {
                    Name = member.Value.Name,
                    Data = member.Value.CombatDamageList.Where(x => x.Key >= DateTime.UtcNow.AddMinutes(-3)).ToDictionary(x => x.Key, x => x.Value)
                });
            }
            return list.ToArray();
        }

        [Route("totalDamage")]
        public object[][] TotalDamage()
        {
            var list = new List<Object[]>();
            var members = _albionEntityData.GetAllEntities(true);
            var totalDamage = members.Sum(x => x.Value.Damage);
            var selectOrderedMembers = members.Select(i => new { i.Value.Name, i.Value.Damage }).OrderByDescending(x => x.Damage);
            foreach (var member in selectOrderedMembers)
            {
                var damagePercentage = member.Damage == 0 ? 0 : (int)(0.5f + ((100f * member.Damage) / totalDamage));
                list.Add(new object[] { $"{member.Name} ( {damagePercentage}% )", member.Damage });

            }
            return list.ToArray();
        }

        [Route("highestDps")]
        public object[][] HighestDps()
        {
            var list = new List<Object[]>();
            var members = _albionEntityData.GetAllEntities(true);
            var selectOrderedMembers = members.Select(i => new { i.Value.Name, i.Value.Dps }).OrderByDescending(x => x.Dps);
            foreach (var member in members)
            {
                list.Add(new object[] { member.Value.Name, member.Value.Dps });

            }

            return list.ToArray();
        }

        [Route("totalHealing")]
        public object[][] TotalHealing()
        {
            var list = new List<Object[]>();
            var members = _albionEntityData.GetAllEntities(true);
            var selectOrderedMembers = members.Select(i => new { i.Value.Name, i.Value.Damage }).OrderByDescending(x => x.Damage);
            foreach (var member in members)
            {
                list.Add(new object[] { member.Value.Name, member.Value.Damage });
            }
            return list.ToArray();
        }
    }
}
