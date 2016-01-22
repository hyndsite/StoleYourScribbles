using Microsoft.WindowsAzure.Storage.Table;

namespace StoleYourScribbles.Web.Entities
{
    public class ScribbleEntity : TableEntity
    {
        public string Scribbles { get; set; }
    }
}