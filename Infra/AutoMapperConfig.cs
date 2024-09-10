using AutoMapper;
using Todo_List_API.Commands.Requests.Todo;
using Todo_List_API.Commands.Responses.TodoItem;
using Todo_List_API.Data.Entity;

namespace Todo_List_API.Infra
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<CreateTodoRequest, TodoItem>();
            CreateMap<UpdateTodoRequest, TodoItem>();
            CreateMap<TodoItem, GetTodoItemResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AspNetUsersId));
            CreateMap<TodoItem, GetTodoItemResponse>();
        }
    }
}
