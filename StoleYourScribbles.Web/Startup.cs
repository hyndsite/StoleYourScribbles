using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Owin;

namespace StoleYourScribbles.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
         var httpConfiguration = new HttpConfiguration();

            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(httpConfiguration);

            var task = InitializeStorage();
            task.Wait();
            httpConfiguration.MapHttpAttributeRoutes();
        }

        private async Task InitializeStorage()
        {
            var accountName = ConfigurationManager.AppSettings["AccountName"];
            var accountKey = ConfigurationManager.AppSettings["AccountKey"];

            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);

            await ValidateTableStorage(storageAccount);
        }

        private async Task ValidateTableStorage(CloudStorageAccount storageAccount)
        {
            var tableName = ConfigurationManager.AppSettings["AzureTableName"];
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);

            try
            {
                if (!await table.ExistsAsync())
                {
                    await table.CreateAsync();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}