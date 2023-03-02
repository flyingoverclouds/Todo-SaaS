using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


var serviceSettings = new front_razor.ServiceSettings();


//var CONTAINER_APP_ENV_DNS_SUFFIX = System.Environment.GetEnvironmentVariable("CONTAINER_APP_ENV_DNS_SUFFIX");
//if (!string.IsNullOrEmpty(CONTAINER_APP_ENV_DNS_SUFFIX))
//{
//    // HACK we found the CONTAINER_APP_ENV_DNS_SUFFIX en variable -> app is runnnin in container app --> Patch service URL ....
//    Trace.TraceWarning($"Running in CONTAINER APP : patching service URL suffix with .internal.{CONTAINER_APP_ENV_DNS_SUFFIX}");
//    serviceSettings.TodoServiceUri = $"{builder.Configuration.GetValue<string>("TodoServiceUri")}.internal.{CONTAINER_APP_ENV_DNS_SUFFIX}";
//}
//else

    serviceSettings.TodoServiceUri = builder.Configuration.GetValue<string>("TodoServiceUri");

builder.Services.AddSingleton<front_razor.ServiceSettings>(serviceSettings);
Trace.TraceInformation($"Settings: TodoServiceUri= {serviceSettings.TodoServiceUri}");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();


app.Run();
