namespace Todo_List_API.Commands.Responses.TodoItem
{
    public class GetListTodoItemResponse
    {
        public int Total { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public List<GetTodoItemResponse> Items { get; set; }
    }
}
