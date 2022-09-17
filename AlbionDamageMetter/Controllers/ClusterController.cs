using AlbionDamageMetter.Albion;
using Microsoft.AspNetCore.Mvc;

namespace AlbionDamageMetter.Controllers
{
    [Route("api/cluster", Name = "Cluster")]
    public class ClusterController : Controller
    {
        private readonly AlbionClusterData _albionClusterData;

        public ClusterController(AlbionClusterData albionClusterData)
        {
            _albionClusterData = albionClusterData;
        }

        public AlbionClusterData Data()
        {
            return _albionClusterData;
        }
    }
}
