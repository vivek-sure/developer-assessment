using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TodoList.Api.Contexts.TodoContext;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoService _service;

        public TodoItemsController(ITodoService service)
        {
            _service = service;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var results = await _service.GetTodoItems();
            return Ok(results);
        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            var result = await _service.GetTodoItemById(id);

            if (result == null)
            {
                return NotFound("No record was found with the provided Id");
            }

            return Ok(result);
        }

        // PUT: api/TodoItems/... 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            try
            {
                var result = await _service.PutTodoItem(id, todoItem);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                if (ex.Message == "The Id specified could not be found")
                {
                    return NotFound("No record was found with the provided Id to replace");
                }

                return BadRequest(ex.Message);
            }
        } 

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItem(TodoItem todoItem)
        {
            try
            {
                var result = await _service.CreateTodoItem(todoItem);
                return CreatedAtAction(nameof(GetTodoItem), new { id = result.Id }, result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
