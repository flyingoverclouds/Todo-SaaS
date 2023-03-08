using System;
using System.Threading.Tasks;
using Azure.Data.Tables;
using MesToudoux.Portal.Common.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace portal_functions
{
    public class DeployWorkflowStatusFunction
    {
        [FunctionName("DeployWorkflowStatusFunction")]
        public async Task Run([QueueTrigger("deploy-workflow-status", Connection = "QueueStorageCnxStr")]string workflowStatus, ILogger log)
        {
            log.LogInformation($"DeployWorkflowStatusFunction: new workflow status : {workflowStatus}");

            var parts = workflowStatus.Split('#'); // format is : TENANTID#FQDN
            if (parts.Length!=2)
            {
                log.LogError($"DeployWorkflowStatusFunction: Invalid message format [{workflowStatus}]");
                return;
            }
            string tenantId = parts[0];
            string fqdn = parts[1];

            // retrieve TenantEntity from tenantdb
            var tableClient = new TableClient(Environment.GetEnvironmentVariable("TenantDbCnxStr"), "TodoTenants");
            TenantEntity tenant = null;
            try
            {
                tenant = await tableClient.GetEntityAsync<TenantEntity>(tenantId, tenantId);
            }
            catch (Exception ex)
            {
                log.LogCritical(ex, $"Unable to retrieve tenant entity for TenantId={tenantId} !! Exception: {ex.Message}");
                return;
            }
            if (tenant == null)
            {
                log.LogCritical($"Unable to find tenant entity for TenantId={tenantId} (no exception)");
                return;
            }

            tenant.InfraDeployed = true;
            tenant.Activated = true;  
            tenant.FQDN= fqdn;

            await tableClient.UpdateEntityAsync<TenantEntity>(tenant, Azure.ETag.All);
            log.LogInformation($"Tenant updated : tenantId={tenantId}  fqdn={fqdn}");
        }
    }
}
