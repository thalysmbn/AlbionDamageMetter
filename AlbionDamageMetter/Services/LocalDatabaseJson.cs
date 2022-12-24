using AlbionDamageMetter.Albion.Models.Database;
using JsonFlatFileDataStore;

namespace AlbionDamageMetter.Services
{
    public class LocalDatabaseJson
    {
        private DataStore DataStore { get; }

        public LocalDatabaseJson()
        {
            DataStore = new DataStore("data.json");
        }

        public IDocumentCollection<ClusterPartyHistoryDatabaseModel> GetClusterPartyHistory()
        {
            return DataStore.GetCollection<ClusterPartyHistoryDatabaseModel>();
        }
    }
}
