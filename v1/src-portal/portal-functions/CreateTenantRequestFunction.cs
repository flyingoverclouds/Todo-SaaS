using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using MesToudoux.Portal.Common.Entities;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Azure.Documents.SystemFunctions;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace portal_functions
{
    public class CreateTenantRequestFunction
    {
        [FunctionName("CreateTenantRequestFunction")]
        public async Task Run(
            [QueueTrigger("tenant-creation-request", Connection = "QueueStorageCnxStr")]string tenantId,
            //[Table("TodoTenants","{queueTrigger}", "{queueTrigger}", Connection="TenantDbCnxStr")]TenantEntity tenant,
            ILogger log)
        {
            log.LogInformation($"CreateTenantRequestFunction: creation request for tenant : [{tenantId}]");

            
            // retrieve TenantEntity from tenantdb
            var tableClient = new TableClient(Environment.GetEnvironmentVariable("TenantDbCnxStr"), "TodoTenants");
            TenantEntity tenant = null;
            try
            {
                tenant = await tableClient.GetEntityAsync<TenantEntity>(tenantId, tenantId);
            }
            catch (Exception ex)
            {
                log.LogCritical(ex, $"Unable to retrieve tenant entity for TenanId={tenantId} !! Exception: {ex.Message}");
                return;
            }
            if (tenant== null)
            {
                log.LogCritical($"Unable to find tenant entity for TenanId={tenantId} (no exception) ==> Github workflow NOT started.");
                return;
            }

            // start GithubWorkflow 
            string githubRepo = Environment.GetEnvironmentVariable("GithubRepo");
            log.LogInformation($"Starting github workflow for tenantId={tenantId} in repo {githubRepo} ");
            string githubApiUrl = $"https://api.github.com/repos/{githubRepo}/dispatches"; 
            string githubBearer = Environment.GetEnvironmentVariable("GithubBearer");
            string body = "{\"event_type\":\"" + tenantId + "\"}";
            using (var hc = new HttpClient())
            {
                hc.DefaultRequestHeaders.Clear();
                hc.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("curl", "1")); // set your own values here
                hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",githubBearer);
                hc.DefaultRequestHeaders.Accept.Clear();
                hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new StringContent(JsonConvert.SerializeObject(new
                {
                    event_type = tenantId
                }), Encoding.UTF8, "application/json");
                try
                {
                    var resp = await hc.PostAsync(githubApiUrl, jsonContent);
                    resp.EnsureSuccessStatusCode();
                    if (resp.StatusCode != System.Net.HttpStatusCode.NoContent)
                    {
                        log.LogError($"Github api {githubApiUrl} return unexpected status code [{resp.StatusCode}]");
                    }
                    else
                    {
                        log.LogInformation($"Github workflow STARTED for tenantId={tenantId}");
                    }
                }
                catch (Exception ex)
                {
                    log.LogCritical(ex, $"Exception while calling github api {githubApiUrl} !! Exception: {ex.Message}");
                }
            }
        }
    }
}
