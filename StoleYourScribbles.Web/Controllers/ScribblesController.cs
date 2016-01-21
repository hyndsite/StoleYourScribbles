using System.IO;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;

namespace StoleYourScribbles.Web.Controllers
{
    [RoutePrefix("~/api/scribbles")]
    public class ScribblesController : ApiController
    {
        [Route("~/api/scribbles/stolen")]
        [HttpGet]
        public IHttpActionResult Stolen([FromUri] string data)
        {
            if (string.IsNullOrWhiteSpace(data)) Ok();

            var bytes = Encoding.UTF8.GetBytes(data);
            string[] entity;
            using (var memoryStream = new MemoryStream(bytes))
            using (var streamReader = new StreamReader(memoryStream, Encoding.UTF8))
            using (var jsonStream = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                entity = (string[]) serializer.Deserialize(jsonStream, typeof (string[]));
            }


            return Ok();
        }
    }
}
