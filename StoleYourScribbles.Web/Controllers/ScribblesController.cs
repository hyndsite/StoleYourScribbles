using System.Web.Http;

namespace StoleYourScribbles.Web.Controllers
{
    [RoutePrefix("~/api/scribbles")]
    public class ScribblesController : ApiController
    {
        [Route("~/api/scribbles/stolen")]
        public void Stolen([FromUri] string data)
        {
            
        }
    }
}
