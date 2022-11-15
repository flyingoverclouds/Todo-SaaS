using todo_service.Models;

namespace todo_service
{
    public interface IItemDataService
    {
        TodoItem GetItem(string Tenant, Guid todoId);
        void CreateItem(TodoItem item);
        void DeleteItem(string Tenant, Guid todoId);
        void CheckItem(string Tenant, Guid todoId, bool done);

    }
}
