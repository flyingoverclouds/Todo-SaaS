using Azure.Data.Tables;
using MesToudoux.Portal.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using static System.Net.WebRequestMethods;


var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


app.MapGet("/" , (IConfiguration configuration) => 
{ 
    return Microsoft.AspNetCore.Http.Results.Redirect(configuration.GetValue<string>("portalUrl"), false); 
});

app.MapGet("/{tenantId}", async (string tenantId, HttpContext context, IConfiguration configuration) =>
{
    Trace.WriteLine($"tenant resquested : '{tenantId}'");

    var tableClient = new TableClient(configuration.GetValue<string>("TenantDbCnxStr"), "TodoTenants");
    try
    {
        TenantEntity tenant = await tableClient.GetEntityAsync<TenantEntity>(tenantId, tenantId);

        if (tenant != null 
            && tenant.InfraDeployed == true   
            && tenant.Activated == true 
            && tenant.Suspended==false
            && tenant.Deleted==false 
            && !string.IsNullOrEmpty(tenant.FQDN))
        {
            context.Response.Redirect(tenant.FQDN, false); // redirecting to fqdn for tenant
            return;
        }
    }
    catch (Exception ex)
    {
        Trace.TraceError($"Exception while retrieving tenant {tenantId} : {ex.Message}");
        Trace.TraceError(ex.ToString());
    }

    context.Response.Redirect($"/UnavailableTenant/{tenantId}", false);
});

app.MapGet("/UnavailableTenant/{tenantId}", (string tenantId) => $"Le tenant '{tenantId}' n'est pas disponible (ou en erreur).");

app.Run();

