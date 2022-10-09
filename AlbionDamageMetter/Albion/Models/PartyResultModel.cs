using AlbionDamageMetter.Albion.Models.NetworkModel;

namespace AlbionDamageMetter.Albion.Models
{
    public class PartyResultModel
    {
        public long HighestDamage { get; set; }
        public long HighestHeal { get; set; }
        public object[][] DamageMember { get; set; }
        public List<KeyValuePair<Guid, PlayerGameObject>> Members { get; set; }
    }
}
