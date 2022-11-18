using System;
using Microsoft.Azure.Cosmos;

using front_common.Models;
using System.Configuration;

namespace dbfiller_dev
{
    /// <summary>
    /// This program will insert test data in cosmos db.
    /// </summary>
    internal class Program
    {
        const string VERSION = "20221117-1";

        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = ConfigurationManager.AppSettings["EndPointUri"];

        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = ConfigurationManager.AppSettings["DatabaseId"];
        private string containerId = ConfigurationManager.AppSettings["ContainerId"];

        public static async Task Main(string[] args)
        {
            Console.WriteLine($"###> DbFiller -DEV- v{VERSION}");


            try
            {
                var p = new Program();
                await p.MainAsync(args);
            }
            catch (CosmosException ce)
            {
                Console.WriteLine($"Erreur COSMOSDB: {ce.Message}");
                Console.WriteLine(ce.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception non gérée : {ex.Message}");
                Console.WriteLine(ex.ToString());
            }

        }

        public async Task MainAsync(string[] args)
        {
            cosmosClient = new CosmosClient(EndpointUri, PrimaryKey,
                new CosmosClientOptions() { 
                    ApplicationName="TodoItemDevDbFiller"
                });
            // Database , Container should already exist and configured
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            


        }

        ICollection<TodoItem> GetTestItems()
        {
            var nbTenant = 3;
            var d = new List<TodoItem>();
            int nb = 1;
            for (int t = 1; t <= nbTenant; t++)
            {
                d.Add(new TodoItem()
                {
                    Id = Guid.NewGuid(),
                    Timestamp = DateTime.Now.AddDays(-nb),
                    Tenant = $"T{t}",
                    Title = $"Todo #{nb}",
                    Content = $"Il y a plein de chose a faire ici T{t}#{nb}",
                    Done = (nb % 1 == 1) ? true : false
                }); ;
                nb++;
            }

            return d;
        }
    }
}




