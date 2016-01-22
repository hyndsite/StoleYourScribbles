using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Newtonsoft.Json;
using StoleYourScribbles.Web.Entities;
using StoleYourScribbles.Web.Repositories;

namespace StoleYourScribbles.Web.Controllers
{
    [RoutePrefix("~/api/scribbles")]
    public class ScribblesController : ApiController
    {
        private readonly ScribblesRepository _scribblesRepository;

        public ScribblesController()
        {
            var accountName = ConfigurationManager.AppSettings["AccountName"];
            var accountKey = ConfigurationManager.AppSettings["AccountKey"];
            var tableName = ConfigurationManager.AppSettings["AzureTableName"];

            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            var tableFactory = new TableFactory(storageAccount);
            var cloudTableClient = tableFactory.GetCloudTableClient();

            _scribblesRepository = new ScribblesRepository(cloudTableClient(tableName));
        }

        [Route("~/api/scribbles/stolen")]
        [HttpGet]
        public async Task<IHttpActionResult> StolenScribble([FromUri] string data)
        {
            if (string.IsNullOrWhiteSpace(data)) Ok();

            var bytes = Encoding.UTF8.GetBytes(data);
            string[] scribble;
            using (var memoryStream = new MemoryStream(bytes))
            using (var streamReader = new StreamReader(memoryStream, Encoding.UTF8))
            using (var jsonStream = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                scribble = (string[])serializer.Deserialize(jsonStream, typeof(string[]));
            }

            string ipaddress = null;
            if (Request.Properties.ContainsKey("MS_OwinContext"))
            {
                var owinContext = Request.Properties["MS_OwinContext"] as OwinContext;
                if (owinContext != null)
                {
                    var address = IPAddress.Parse(owinContext.Request.RemoteIpAddress);
                    address = address.MapToIPv4();
                    ipaddress = address.ToString();
                }
            }

            var capturedScribble = new ScribbleEntity
            {
                PartitionKey = ipaddress,
                RowKey = Guid.NewGuid().ToString("N"),
                Scribbles = string.Join(",", scribble)
            };

            try
            {
                await _scribblesRepository.SaveAsync(capturedScribble);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
