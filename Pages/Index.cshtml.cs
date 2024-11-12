using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Data.Tables;
using Azure;


namespace IBAS_kantine.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public record MenuItem
    {
        public string PartitionKey { get; init; } = "Menu"; // Default value for consistency
        public string RowKey { get; init; } // Represents the day of the week
        public string KoldRet { get; init; } // Represents the cold dish
        public string VarmRet { get; init; } // Represents the hot dish
    }

    public List<MenuItem> MenuItems { get; init; } = new List<MenuItem>();


    public void OnGet()
    {
        var tablename = "ibaskantine";
        var connectionString = "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=ibasstorage;AccountKey=RBoOmlWMPeBB7TqCjr5ydqhnGlOBe2QIoS1HtKou9hBos3wrL4ls/eQtKvAPukPFK975z21Lfpp7+ASt5BaLIw==;BlobEndpoint=https://ibasstorage.blob.core.windows.net/;FileEndpoint=https://ibasstorage.file.core.windows.net/;QueueEndpoint=https://ibasstorage.queue.core.windows.net/;TableEndpoint=https://ibasstorage.table.core.windows.net/";


        TableClient tableClient = new TableClient(connectionString, tablename);
        Pageable<TableEntity> entities = tableClient.Query<TableEntity>();


        var _productionRepo = new List<MenuItem>();
        foreach (TableEntity entity in entities)
        {
            var menuitem = new MenuItem
            {
                RowKey = entity.RowKey,
                PartitionKey = entity.PartitionKey,
                KoldRet = entity.GetString("KoldRet"),
                VarmRet = entity.GetString("VarmRet")

            };
            _productionRepo.Add(menuitem);
            MenuItems.Add(menuitem);
        }



        foreach (MenuItem menuItem in MenuItems)
        {
            Console.WriteLine($"{menuItem.RowKey} {menuItem.KoldRet} {menuItem.VarmRet}");
        }

    }
}
