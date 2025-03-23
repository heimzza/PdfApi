using Microsoft.AspNetCore.Mvc;
using PdfApi.Models;

namespace PdfApi.Controllers;

public class OrdersController : Controller
{
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> DownloadInvoice()
    {
        var orderDetails = new List<OrderDetailDto>();

        for (int i = 0; i < 12; i++)
        {
            var purchaseOrder = new OrderDetailDto
            {
                Content = null,
                Title = "Product " + i,
                UserName = i + ". customer",
                AssetId = i + "-XA49MG",
                Format = "Media",
                Size = "N/A",
                Licensing = "Commercial"
            };
            
            orderDetails.Add(purchaseOrder);
        }
        
        
    }

    private MemoryStream GeneratePdf(List<OrderDetailDto> orderDetails, decimal price)
    {
        
    }
}