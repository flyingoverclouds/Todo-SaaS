using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front_razor.Pages
{
    public class ChangeDoneModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public ChangeDoneModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}