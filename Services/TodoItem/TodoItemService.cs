using AutoMapper;
using System.Net;
using Todo_List_API.Commands.Requests.Todo;
using Todo_List_API.Commands.Responses.TodoItem;
using Todo_List_API.Data.Contexts;
using Todo_List_API.Data.Entity;
using Todo_List_API.UoW;

namespace Todo_List_API.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly UnityOfWork _unityOfWork;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public TodoItemService(UnityOfWork unityOfWork, IMapper mapper, AppDbContext appDbContext)
        {
            _unityOfWork = unityOfWork;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public Task<ServiceResponse> Create(CreateTodoRequest request, string userId)
        {

            var todo= _mapper.Map<CreateTodoRequest, TodoItem>(request);

            todo.AspNetUsersId = userId;
            todo.CreatedAt = DateTime.UtcNow;
            todo.IsDone = false;
            todo.Status = "Backlog";
            todo.TotalTime = DateTime.MinValue;

            _appDbContext.Set<TodoItem>().Add(todo);

            _unityOfWork.Commit();

            return Task.FromResult(ServiceResponse.Factory(true, "Todo created!", HttpStatusCode.Created, null));
        }

        public Task<ServiceResponse> Delete(Guid id)
        {
            var todo = _appDbContext.Set<TodoItem>().FirstOrDefault(x => x.Id == id);

            if (todo == null)
                return Task.FromResult(ServiceResponse.Factory(false, "Todo not found!", HttpStatusCode.NotFound, null));

            _appDbContext.Set<TodoItem>().Remove(todo);

            _unityOfWork.Commit();

            return Task.FromResult(ServiceResponse.Factory(true, "Todo deleted!", HttpStatusCode.OK, null));
        }
        public Task<ServiceResponse> Update(UpdateTodoRequest request, Guid id)
        {
            var todo = _appDbContext.Set<TodoItem>().FirstOrDefault(x => x.Id == id);

            if (todo == null)
                return Task.FromResult(ServiceResponse.Factory(false, "Todo not found!", HttpStatusCode.NotFound, null));

            todo.Title = request.Title;
            todo.Description = request.Description;
            todo.IsDone = request.IsDone;
            todo.Status = request.Status;
            todo.UpdatedAt = DateTime.UtcNow;
            todo.TotalTime = request.TotalTime;

            _appDbContext.Set<TodoItem>().Update(todo);

            _unityOfWork.Commit();

            return Task.FromResult(ServiceResponse.Factory(true, "Todo updated!", HttpStatusCode.OK, null));
        }

        public Task<ServiceResponse> Get(Guid id)
        {
            var todo = _appDbContext.Set<TodoItem>().FirstOrDefault(x => x.Id == id);

            if (todo == null)
                return Task.FromResult(ServiceResponse.Factory(false, "Todo not found!", HttpStatusCode.NotFound, null));

            var result = _mapper.Map<TodoItem, GetTodoItemResponse>(todo);

            return Task.FromResult(ServiceResponse.Factory(true, "Todo found!", HttpStatusCode.OK, todo));

        }

        public Task<ServiceResponse> GetAll(int take, int skip, string? search)
        {
            var todos = _appDbContext.Set<TodoItem>().AsQueryable();

            if (!string.IsNullOrEmpty(search))
                todos = todos.Where(x => x.Title.Contains(search) || x.Description.Contains(search));

            var result = todos.Skip(skip).Take(take).ToList();

            var resultPaginated = new GetListTodoItemResponse
            {
                Items = _mapper.Map<List<TodoItem>, List<GetTodoItemResponse>>(result),
                Skip = skip,
                Take = take,
                Total = todos.Count()
            };

            return Task.FromResult(ServiceResponse.Factory(true, "Todos found!", HttpStatusCode.OK, resultPaginated));
        }
    }
}
