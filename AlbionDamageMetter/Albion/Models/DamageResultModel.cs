namespace AlbionDamageMetter.Albion.Models
{
    public class DamageResultModel
    {
        public string Name { get; set; }
        public Dictionary<DateTime, long> Data { get; set; }
    }
}
