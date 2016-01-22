using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using StoleYourScribbles.Web.Entities;

namespace StoleYourScribbles.Web.Repositories
{
    public class ScribblesRepository
    {
        private readonly CloudTable _cloutTable;

        public ScribblesRepository(CloudTable cloutTable)
        {
            _cloutTable = cloutTable;
        }

        public async Task<TableResult> SaveAsync(ScribbleEntity scribble)
        {
                var operation = TableOperation.Insert(scribble);
                return await _cloutTable.ExecuteAsync(operation);
        }
    }

}