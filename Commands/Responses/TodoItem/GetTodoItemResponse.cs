namespace Todo_List_API.Commands.Responses.TodoItem
{
    public class GetTodoItemResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime TotalTime { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
    }
}
