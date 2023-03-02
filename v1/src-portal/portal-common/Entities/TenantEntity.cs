using Azure;
using Azure.Data.Tables;

namespace MesToudoux.Portal.Common.Entities
{
    public class TenantEntity :  ITableEntity
    {
        /// <summary>
        /// Partitition is the tenant ID
        /// </summary>
        public string PartitionKey { get; set; }
        
        /// <summary>
        /// Rowkey (tenantID)
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Timestamp of last update
        /// </summary>
        public DateTimeOffset? Timestamp { get ; set ; }

        /// <summary>
        /// Etag
        /// </summary>
        public ETag ETag { get ; set ; }

        /// <summary>
        /// Tenant Name (human readable tenant name, for display)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date of creation request (for birthday billing date)
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// true if main infrastructure deployed
        /// </summary>
        public bool InfraDeployed { get; set; }

        /// <summary>
        /// true if entry dns zone for tenant is ready
        /// </summary>
        public bool DnsDeployed { get; set; }

        /// <summary>
        /// true if tenant is activated (redirect from tenantid.customers.mestoudoux.com works)
        /// </summary>
        public bool Activated { get; set; }

        /// <summary>
        /// true if tenant is suspended (deployed, activated, but easy url redirect to suspended homepage)
        /// </summary>
        public bool Suspended { get; set; }

        /// <summary>
        /// true if tenant has beed deleted. 
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// if true, all action for changin the tenant status are forbidden (deactivate, delete, ...)
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// root url for deployed tenant ( use for redirecting from tenantid.customers.mestoudoux.com )
        /// </summary>
        public string FQDN { get; set; }
    }
}