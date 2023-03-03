using Azure.Data.Tables;
using Azure.Storage.Queues; 
using MesToudoux.Portal.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using portal_front.Models;
using System.Diagnostics;
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

        public async Task OnPost()
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
                ErrorMessage = "Code de s&eacute;curit&eacute; incorrect.";
                return;
            }
            if (string.IsNullOrEmpty(TenantId) || TenantId?.Length>5)
            {
                ErrorMessage = "L'ID de tenant est obligatoire et doit faire 5 caract&egrave;res MAX (lettre minuscule ou chiffres)";
                return;
            }
            TenantId = TenantId.ToLower();

            if (string.IsNullOrEmpty(SkuCode) || SkuCode=="-")
            {
                ErrorMessage = "Vous devez sélectionner un tiers supportés";
                return;
            }


            // Check if tenant already existe in tenant db
            var tableClient = new TableClient(_configuration.GetValue<string>("TenantDbConnectionString"), "TodoTenants");
            TenantEntity tenant = null;
            try {
                tenant = await tableClient.GetEntityAsync<TenantEntity>(TenantId, TenantId);

                // if no error , an existing tenant has been found ... 
                ErrorMessage = "Cet ID de tenant est déjà alloué. Merci de corriger votre saisie";
                return;
            }
            catch (Exception ex) {
                // Error -> no tenant found, that's good :D
            }

            tenant = new TenantEntity
            {
                PartitionKey = TenantId,
                RowKey = TenantId,
                Name = $"Tenant {TenantId}",
                CreationDate = DateTime.UtcNow,
                FQDN=string.Empty,
                InfraDeployed =false,
                DnsDeployed=false,
                Activated = false,
                Suspended=false,
                Deleted=false,
                Locked=false                
            };
            await tableClient.AddEntityAsync<TenantEntity>(tenant);

            // TODO : push message to queue to trigger infrastructure creation
            QueueClient qc = new QueueClient(new Uri(_configuration.GetValue<string>("TenantCreationRequestQueueConnectionString")));
            var creationRequestMsg = $"{TenantId}";
            var receipt = await qc.SendMessageAsync(creationRequestMsg);

            Trace.TraceInformation($"Tenant creation requested : TenantId={TenantId}   MessageId={receipt.Value.MessageId}"); 

            this.SuccessMessage = $"La demande de création du tenant [{TenantId}] est prise en compte. RequestId={receipt.Value.MessageId}";
            this.SecurityCode = string.Empty;
            
        
        }
    }
}
