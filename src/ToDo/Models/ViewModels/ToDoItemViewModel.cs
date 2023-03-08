using Microsoft.AspNetCore.Http;
namespace ToDoProject.Models.ViewModels
{
    public class ToDoItemViewModel
    {
        public int Id { get; set; }
        public string Tittle { get; set; }
        public bool isActive { get; set; }
        public IFormFile Image { get; set; }
    }
}
