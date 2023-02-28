using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace portal_front.Pages
{
    public class tenant_statusModel : PageModel
    {


        private IConfiguration _configuration;
        private readonly ILogger<IndexModel> _logger;
        public tenant_statusModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [BindProperty]
        public string? TenantId { get; set; }



        public string? ErrorMessage { get; set; } = "";
        
        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (string.IsNullOrEmpty(TenantId) || TenantId?.Length > 4)
            {
                ErrorMessage = "L'ID de tenant est obligatoire et doit faire 4 caracteres maximum (lettre minuscule ou chiffres)";
                return;
            }
            TenantId = TenantId.ToLower();

            // TODO : get tenant status from table DB

            this.ErrorMessage = $"Le tenant [{TenantId}] est inconnu.";
        }
    }
}
