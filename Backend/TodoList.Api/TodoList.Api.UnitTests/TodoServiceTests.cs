using System;
using TodoList.Api.Contexts.TodoContext;
using TodoList.Api.Services;
using Xunit;

namespace TodoList.Api.UnitTests
{
    public class TodoServiceTests : IClassFixture<TodoContextFixture>
    {
        private readonly TodoService _service;

        private readonly TodoItem _testItem;
        


        /// <summary>
        /// Initialize our test data and servies
        /// </summary>
        /// <param name="fixture">Test fixture</param>
        public TodoServiceTests(TodoContextFixture fixture)
        {
            _service = new TodoService(fixture.todoContext);
            // Create our test data
            _testItem = _service.CreateTodoItem(new TodoItem()
            {
                Description = Guid.NewGuid().ToString(),
                IsCompleted = false,
                
            }).Result;
        }

        [Fact]
        public void GetTodoItemsTest()
        {
            var result = _service.GetTodoItems().Result;
            Assert.Contains(_testItem, result);
        }

        [Fact]
        public void GetTodoItemByIdTest()
        {
            var result = _service.GetTodoItemById(_testItem.Id).Result;
            Assert.Equal(_testItem, result);
        }

        [Fact]
        public void PutTodoItemTest()
        {
            _testItem.IsCompleted = true;
            var result = _service.PutTodoItem(_testItem.Id, _testItem).Result;
            Assert.True(result.IsCompleted);
        }
    }
}
