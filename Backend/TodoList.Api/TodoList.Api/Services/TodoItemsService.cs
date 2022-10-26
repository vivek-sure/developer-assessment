using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Contexts.TodoContext;

namespace TodoList.Api.Services
{
    /// <summary>
    /// Service for interating with todo related services
    /// 
    /// The idea of having these operations inside a service class is
    /// to make the code reusable
    /// </summary>
    public class TodoService : ITodoService
    {
        private readonly TodoContext _context;

        /// <summary>
        /// Initializer for our todo context
        /// </summary>
        /// <param name="context"></param>
        public TodoService(TodoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of todo items which have not yet been complete
        /// </summary>
        public async Task<List<TodoItem>> GetTodoItems()
        {
            return await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
        }

        /// <summary>
        /// Retrieves a todo item using it's Id, null if not found
        /// </summary>
        /// <param name="id">Id of the todo item to retrieve</param>
        public async Task<TodoItem?> GetTodoItemById(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        /// <summary>
        /// Completely replaces a todo item with the values provided
        /// </summary>
        /// <param name="id">Id of the todo item we want to replace</param>
        /// <param name="todoItem">Values of the todo item we want to use for replacement</param>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        public async Task<TodoItem> PutTodoItem(Guid id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                throw new ValidationException("The Id provided does not match the record being replaced");
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemIdExists(id))
                {
                    throw new ValidationException("The Id specified could not be found");
                }
                else
                {
                    throw;
                }
            }

            return todoItem;
        }

        /// <summary>
        /// Creates a new todo item and returns the result
        /// </summary>
        /// <param name="todoItem"></param>
        /// <exception cref="ValidationException"></exception>
        public async Task<TodoItem> CreateTodoItem(TodoItem todoItem)
        {
            if (string.IsNullOrEmpty(todoItem.Description))
            {
                throw new ValidationException("Description is required");
            }
            else if (TodoItemDescriptionExists(todoItem.Description))
            {
                throw new ValidationException("Description already exists");
            }

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
             
            return todoItem;
        } 

        /// <summary>
        /// Checks to see if we have a todo item with the provided Id
        /// </summary>
        /// <param name="id">Id value to check</param>
        public bool TodoItemIdExists(Guid id)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }

        /// <summary>
        /// Checks to see if we have a non-completed todo item with the provided description
        /// </summary>
        /// <param name="description">Description value to check</param>
        public bool TodoItemDescriptionExists(string description)
        {
            return _context.TodoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }
        
    }
}
