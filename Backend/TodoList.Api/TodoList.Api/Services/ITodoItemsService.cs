using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Contexts.TodoContext;

namespace TodoList.Api.Services;
public interface ITodoService
{
        Task<List<TodoItem>> GetTodoItems();
        Task<TodoItem?> GetTodoItemById(Guid id);
        Task<TodoItem> PutTodoItem(Guid id, TodoItem todoItem);
        Task<TodoItem> CreateTodoItem(TodoItem todoItem);
        bool TodoItemIdExists(Guid id);
        bool TodoItemDescriptionExists(string description);
}