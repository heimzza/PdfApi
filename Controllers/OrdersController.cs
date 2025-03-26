using Microsoft.AspNetCore.Mvc;
using PdfApi.Models;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrdersController : Controller
{
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult DownloadInvoice()
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

        var pdfStream = GeneratePdf(orderDetails, 100);
        
        Response.Headers.ContentDisposition = "attachment: filename=invoice.pdf";
        
        pdfStream.Seek(0, SeekOrigin.Begin);
        
        return File(pdfStream, "application/pdf", "invoice.pdf");
    }

    private MemoryStream GeneratePdf(List<OrderDetailDto> orderDetails, decimal price)
    {
        Settings.License = LicenseType.Community;
        
        var pdfStream = new MemoryStream();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.Content()
                    .PaddingHorizontal(20)
                    .PaddingVertical(20)
                    .DefaultTextStyle(x => x.FontSize(12))
                    .Column(main =>
                    {
                        BuildDetails(main, orderDetails);
                    });
            });
        }).GeneratePdf(pdfStream);
        
        return pdfStream;
    }

    private void BuildDetails(ColumnDescriptor main, List<OrderDetailDto> orderDetails)
    {
        main.Item()
            .Container()
            .Text("Purchase Details")
            .Bold();
        
        main.Item()
            .Container()
            .Table(table =>
            {
                table.ColumnsDefinition(c =>
                {
                    c.ConstantColumn(60);
                    c.RelativeColumn(3);
                    c.RelativeColumn(3);
                    c.RelativeColumn(3);
                    c.RelativeColumn(3);
                });

                foreach (var detail in orderDetails)
                {
                    table.Cell().Element(c =>
                    {
                        c.Container()
                            .Text(detail.Title)
                            .FontSize(14);
                    });
                    
                    table.Cell().Element(c =>
                    {
                        c.Container().Text(detail.Price);
                    });

                    table.Cell().Text(detail.AssetId);
                    table.Cell().Text(detail.Format);
                    table.Cell().Text(detail.Licensing);
                }
            });
    }
}