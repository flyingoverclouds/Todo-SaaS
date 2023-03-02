using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace portal_functions
{
    public class  CreateTenantRequestFunction
    {
        [FunctionName("CreateTenantRequestFunction")]
        public void Run([QueueTrigger("tenant-creation-request", Connection = "TenantCreationRequestQueue")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
