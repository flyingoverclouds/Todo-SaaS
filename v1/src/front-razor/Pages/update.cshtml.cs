using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front_razor.Pages
{
    public class UpdateModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public UpdateModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}