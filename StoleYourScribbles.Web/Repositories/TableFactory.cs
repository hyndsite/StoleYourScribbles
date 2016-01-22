using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace StoleYourScribbles.Web.Repositories
{
    public class TableFactory
    {
        private readonly CloudStorageAccount _account;

        public TableFactory(CloudStorageAccount account)
        {
            _account = account;
        }

        public Func<string, CloudTable> GetCloudTableClient()
        {
            Func<string, CloudTable> cloudTableFunc = (tableName) =>
            {
                var cloudTableClient = _account.CreateCloudTableClient();
                var cloudTable = cloudTableClient.GetTableReference(tableName);
                return cloudTable;
            };
            return cloudTableFunc;
        }
    }
}