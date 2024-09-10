using Todo_List_API.Commands.Requests.Todo;

namespace Todo_List_API.Services
{
    public interface ITodoItemService
    {
        Task<ServiceResponse> Create(CreateTodoRequest request, string userId);
        Task<ServiceResponse> Update(UpdateTodoRequest request, Guid id);
        Task<ServiceResponse> Delete(Guid id);
        Task<ServiceResponse> Get(Guid id);
        Task<ServiceResponse> GetAll(int take, int skip, string? search);
    }
}
