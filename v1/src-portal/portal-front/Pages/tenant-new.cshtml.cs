using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using portal_front.Models;
using System.Drawing;
using System.Security.Cryptography;

namespace portal_front.Pages
{
    public class tenant_newModel : PageModel
    {

        private IConfiguration _configuration;
        private readonly ILogger<IndexModel> _logger;
        public tenant_newModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [BindProperty]
        public string? TenantId { get; set; }

        [BindProperty]
        public string? SecurityCode { get; set; }

        [BindProperty]
        public string? SkuCode { get; set; }


        public string? ErrorMessage { get; set; } = "";
        public string? SuccessMessage { get; set; } = "";

        public IEnumerable<TenantSku> AvailableSkus = new List<TenantSku> {
                    new TenantSku { SkuCode ="-", SkuName = "-" },
                    new TenantSku { SkuCode ="F0", SkuName = "Free" },
                    new TenantSku { SkuCode ="B0", SkuName = "Basic" },
                    new TenantSku { SkuCode ="S0", SkuName = "Standard" },
                    new TenantSku { SkuCode ="P0", SkuName = "Pro" }
        };

        public void OnGet()
        {
        }

        public void OnPost()
        {
            
            if (string.IsNullOrEmpty(SecurityCode))
            {
                ErrorMessage = "Le code de sécurité est obligatoire";
                return;
            }
            
            // Si si, ca c'est de la sécurité :D
            var sha512sc = Convert.ToHexString(SHA512.HashData(System.Text.Encoding.UTF8.GetBytes(SecurityCode)));
            
            if (sha512sc != _configuration.GetValue<string>("SecurityCodeHash"))
            {
                ErrorMessage = "Code de sécurité incorrect.";
                return;
            }
            if (string.IsNullOrEmpty(TenantId) || TenantId?.Length>4)
            {
                ErrorMessage = "L'ID de tenant est obligatoire et doit faire 4 caracteres maximum (lettre minuscule ou chiffres)";
                return;
            }
            TenantId = TenantId.ToLower();

            if (string.IsNullOrEmpty(SkuCode) || SkuCode=="-")
            {
                ErrorMessage = "Vous devez sélectionner un tiers supportés";
                return;
            }


            // TODO : check if tenant already exist
            if (TenantId=="0000")
            {
                ErrorMessage = "Cet ID de tenant est déjà alloué. Merci de corriger votre saisie";
                return;
            }

            // TODO : push message to queue to trigger infrastructure creation
            // TODO : log msg ID

            this.SuccessMessage = $"La demande de création du tenant [{TenantId}] est prise en compte.";
            this.SecurityCode = string.Empty;
            
        
        }
    }
}
