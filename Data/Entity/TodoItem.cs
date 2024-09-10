using System.ComponentModel.DataAnnotations.Schema;

namespace Todo_List_API.Data.Entity
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime TotalTime { get; set; }
        public string Status { get; set; }

        [ForeignKey("AspNetUsersId")]
        public string AspNetUsersId { get; set; }
    }
}
