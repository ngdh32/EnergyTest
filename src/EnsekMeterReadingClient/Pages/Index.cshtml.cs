using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EnsekMeterReadingClient.Pages;

public class IndexModel : PageModel
{
    public string? MeterUploadUrl { get; set; }

    public IndexModel(IConfiguration configuration)
    {
        MeterUploadUrl = configuration["MeterUploadUrl"];
    }

    public void OnGet()
    {
    }
}