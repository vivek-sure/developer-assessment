using Microsoft.EntityFrameworkCore;
using TodoList.Api.Contexts.TodoContext;

namespace TodoList.Api.UnitTests
{
    /// <summary>
    /// Context for testing todo data
    /// </summary>
    public class TodoContextFixture
    {
        public TodoContext todoContext;

        /// <summary>
        /// Create our inmemory context for testing
        /// </summary>
        public TodoContextFixture()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<TodoContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase("TodoItemsDB");
            todoContext = new TodoContext(dbContextOptionsBuilder.Options);
        }
    }
}