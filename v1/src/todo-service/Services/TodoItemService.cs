using front_common.Models;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;

namespace todo_service.Services
{
    public class TodoItemService : IItemDataService
    {
        private readonly string tenant;
        private readonly Microsoft.Azure.Cosmos.Container dbContainer;
        public TodoItemService(Microsoft.Azure.Cosmos.Container cosmosContainer,string tenant)
        {
            this.dbContainer = cosmosContainer;
            this.tenant = tenant;
        }

        public async Task SetDoneAsync(Guid todoId, bool done)
        {
            // TODO : implement a partial update of TodoItems
            throw new NotImplementedException();
        }

        public async Task CreateItemAsync(TodoItem item)
        {
            var result = await dbContainer.CreateItemAsync<TodoItem>(item);
            // TODO : check item response to ensure write is correct
        }


        public async Task UpdateItemAsync(TodoItem item)
        {
            var result = await dbContainer.UpsertItemAsync<TodoItem>(item);
            // TODO : check item response to ensure write is correct
        }


        public async Task DeleteItemAsync(Guid todoId)
        {
            // TODO : v2: implement deletion of a TodoItem 
            throw new NotImplementedException();
        }

        

        public async Task<TodoItem> GetItemAsync( Guid todoId)
        {
            TodoItem item = null; ;
            
            QueryDefinition query = new QueryDefinition("SELECT * from Items i where i.tenant = @Tenant AND i.id=@TodoId")
                .WithParameter("@Tenant",tenant)
                .WithParameter("@TodoId", todoId);
            using (FeedIterator<TodoItem> resultset = dbContainer.GetItemQueryIterator<TodoItem>(query))
            {
                if (resultset.HasMoreResults)
                {
                    FeedResponse<TodoItem> response = await resultset.ReadNextAsync();
                    item = response.FirstOrDefault();
                }
            }
            return item;
        }

        public async  Task<IEnumerable<TodoItem>> GetItemsAsync()
        {

            var items = new List<TodoItem>();
            List<TodoItem> list = new List<TodoItem>();
            QueryDefinition query = new QueryDefinition("SELECT * from Items i where i.tenant = @Tenant")
                .WithParameter("@Tenant", tenant);
            using (FeedIterator<TodoItem> resultset = dbContainer.GetItemQueryIterator<TodoItem>(query))
            {
                while (resultset.HasMoreResults)
                {
                    FeedResponse<TodoItem> response = await resultset.ReadNextAsync();
                    //Console.WriteLine("Q1 took {0} ms. RU consumed: {1}, Number of items : {2}", response.Diagnostics.GetClientElapsedTime().TotalMilliseconds, response.RequestCharge, response.Count);
                    foreach (var item in response)
                    {
                        items.Add(item);
                    }
                }
            }
            return items;
        }
    }
}
