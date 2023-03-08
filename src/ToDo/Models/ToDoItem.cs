using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class ToDoItem
    {
        [Key]
        public int Id { get; set; }
        public string Tittle { get; set; }
        public bool isActive { get; set; }
        public byte[] Image { get; set; }
    }
}
