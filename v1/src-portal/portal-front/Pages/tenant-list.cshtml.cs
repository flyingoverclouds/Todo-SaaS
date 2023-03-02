using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MesToudoux.Portal.Common.Entities;
using Azure.Data.Tables;
using Azure;
using System.Collections.Concurrent;
using Microsoft.Extensions.Azure;

namespace portal_front.Pages
{
    public class tenant_listModel : PageModel
    {

        public ILogger<tenant_listModel> _Logger { get; set; }
        public IConfiguration _Configuration { get; set; }

        public tenant_listModel(ILogger<tenant_listModel> logger, IConfiguration configuration)
        {
            _Logger = logger;
            _Configuration = configuration;
        }

        public IEnumerable<TenantEntity> Tenants { get; set; } = null;


        public async Task OnGet()
        {
            var l =new List<TenantEntity>();

            var tc = new TableClient(_Configuration.GetValue<string>("TenantDbConnectionString"), "TodoTenants");
            AsyncPageable<TenantEntity> queryResultsMaxPerPage = tc.QueryAsync<TenantEntity>(filter: $"", maxPerPage: 10);

            await foreach (Page<TenantEntity> page in queryResultsMaxPerPage.AsPages())
            {
                foreach (TenantEntity qEntity in page.Values)
                {
                    l.Add(qEntity);
                }
            }
            this.Tenants = l;
        }
    }
}
