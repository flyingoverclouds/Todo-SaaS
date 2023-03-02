using front_common.Models;

namespace todo_service
{
    public interface IItemDataService
    {
        Task<TodoItem> GetItemAsync(Guid todoId);
        Task<IEnumerable<TodoItem>> GetItemsAsync();
        Task CreateItemAsync(TodoItem item);
        Task DeleteItemAsync(Guid todoId);
        Task SetDoneAsync(Guid todoId, bool done);
        Task UpdateItemAsync(TodoItem item);
    }
}
