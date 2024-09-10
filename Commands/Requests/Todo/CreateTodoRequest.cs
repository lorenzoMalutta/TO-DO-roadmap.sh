using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Todo_List_API.Commands.Requests.Todo
{
    public class CreateTodoRequest
    {
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status is required!")]
        public bool Status { get; set; }

    }
}
