using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class ListEntry
    {
        public int Id { get; set; }
        [Required]
		public string? Description { get; set; }
        public bool isDone { get; set; }
    }
}
