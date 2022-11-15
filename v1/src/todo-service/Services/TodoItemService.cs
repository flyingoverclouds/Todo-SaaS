using todo_service.Models;

namespace todo_service.Services
{
    public class TodoItemService : IItemDataService
    {
        public void CheckItem(string Tenant, Guid todoId, bool done)
        {
            //throw new NotImplementedException();
        }

        public void CreateItem(TodoItem item)
        {
            //throw new NotImplementedException();
        }

        public void DeleteItem(string Tenant, Guid todoId)
        {
            //throw new NotImplementedException();
        }

        public TodoItem GetItem(string Tenant, Guid todoId)
        {
            return new TodoItem()
            {
                Id = todoId,
                Tenant = Tenant,
                Title = $"GET ITEM {todoId}",
                Content = $"no content for item {todoId}",
                Done = ((todoId.ToByteArray()[0] & 1) == 1) ? true : false

            };
            
        }
    }
}
