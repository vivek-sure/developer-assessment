using Microsoft.EntityFrameworkCore;

namespace TodoList.Api.Contexts.TodoContext
{
    /// <summary>
    /// DB Context for Todo data
    /// </summary>
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options) {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
