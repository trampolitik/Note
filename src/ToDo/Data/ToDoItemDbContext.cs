using Microsoft.EntityFrameworkCore;
using ToDo.Models;

namespace ToDo.Data
{
    public class ToDoItemDbContext: DbContext
    {
        public ToDoItemDbContext(DbContextOptions<ToDoItemDbContext> option): base(option)
        {

        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
