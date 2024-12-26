using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ResourceUsage.AOTDemo
{
    public class IndexModel : PageModel
    {
        public string ApiUrl { get; set; } = "https://your-api-url.com/calculate-primes"; // Defina a URL padrão aqui
        public string Limit { get; set; } = "10000"; // Valor padrão para upperLimit

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndexModel(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public void OnGet()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            ApiUrl = $"{request.Scheme}://{request.Host.Value}/benchmark/run";
        }

    }
}
