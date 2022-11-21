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

            if (string.IsNullOrEmpty(PrimaryKey))
            {
                Console.WriteLine("ERREUR : Vérifier la présence et les valeurs dans le fichier App.Config");
                return;
            }


            try
            {
                var p = new Program();
                await p.MainAsync(args);
            }
            catch (CosmosException ce)
            {
                Console.WriteLine($"Erreur COSMOSDB: {ce.Message}");
                //Console.WriteLine(ce.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception non gérée : {ex.Message}");
                //Console.WriteLine(ex.ToString());
            }

        }

        public async Task MainAsync(string[] args)
        {
            cosmosClient = new CosmosClient(EndpointUri, PrimaryKey,
                new CosmosClientOptions() { 
                    ApplicationName="TodoItemDevDbFiller"
                });
            // Database and Container should already exist and configured
            database = cosmosClient.GetDatabase(databaseId);
            container = database.GetContainer(containerId);

            var items = GetTestItems();

            foreach(var i in items)
            {
                Console.Write($"ITM #{i.id} ...");
                var result = await container.CreateItemAsync<TodoItem>(i);
                Console.WriteLine($"ID={result.Resource.id}      RUs={result.RequestCharge} \n");
            }
            
        }

        ICollection<TodoItem> GetTestItems()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            const int nbTenant = 3;
            const int nbItemPerTenant = 10;
            var d = new List<TodoItem>();
            int nb = 1;
            for (int t = 1; t <= nbTenant; t++)
            {
                for (int i = 1; i <= nbItemPerTenant; i++)
                {
                    d.Add(new TodoItem()
                    {
                        id = Guid.NewGuid(),
                        timestamp = DateTime.Now.AddDays(-rnd.NextInt64(120)),
                        tenant = $"T{t}",
                        title = $"Todo #{nb}",
                        content = $"Ceci est le todo {nb} : T{t}#{i}",
                        done = rnd.NextInt64() % 1 == 1
                    });
                    nb++;
                }
            }

            return d;
        }
    }
}




