using System.ComponentModel.DataAnnotations;

namespace Todo_List_API.Commands.Requests.Todo
{
    public class UpdateTodoRequest
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Done flag is required")]
        public bool IsDone { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Total time is required")]
        public DateTime TotalTime { get; set; }
    }
}
