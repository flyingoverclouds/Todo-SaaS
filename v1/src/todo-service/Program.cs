using Microsoft.Azure.Cosmos;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Configuration.AddEnvironmentVariables();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string EndpointUri = builder.Configuration.GetValue<string>("EndPointUri");
string PrimaryKey = builder.Configuration.GetValue<string>("PrimaryKey");
string databaseId = builder.Configuration.GetValue<string>("DatabaseId");
string containerId = builder.Configuration.GetValue<string>("ContainerId");


Trace.TraceInformation($"Settings: EndpointURI = {EndpointUri}");
Trace.TraceInformation($"Settings: PrimaryKey = {PrimaryKey.Substring(0,5)}...REDACTED");
Trace.TraceInformation($"Settings: EndpointURI = {databaseId}");
Trace.TraceInformation($"Settings: EndpointURI = {containerId}");

var cosmosClient = new CosmosClient(EndpointUri, PrimaryKey,
               new CosmosClientOptions()
               {
                   ApplicationName = "TodoSaas-toto-service"
               });
var database = cosmosClient.GetDatabase(databaseId);
var container = database.GetContainer(containerId);

builder.Services.AddSingleton(container);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
