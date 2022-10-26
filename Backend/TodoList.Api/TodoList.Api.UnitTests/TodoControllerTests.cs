using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TodoList.Api.Contexts.TodoContext;
using TodoList.Api.Services;
using TodoList.Api.Controllers;
using Xunit;

namespace TodoList.Api.UnitTests;

public class TodoControllerTests
{
    readonly ITodoService _mockService;
    readonly IFixture _fixture;

        public TodoControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            _mockService = _fixture.Freeze<ITodoService>();
        }

        [Fact]
        public async Task GetTodo_ValidRequest_ShouldReturnOk()
        {
            // ARRANGE
            var todo = new TodoItem
            {
                Description = "something"
            };

            _mockService.GetTodoItemById(Arg.Any<Guid>())
                .Returns( todo );


            var sut = new TodoItemsController(_mockService);

            // ACT
            var result = (OkObjectResult)await sut.GetTodoItem(Guid.NewGuid());

            // ASSERT
            Assert.Equal(result.StatusCode, StatusCodes.Status200OK);;
            Assert.Equal(result.Value,todo);
        }

        [Fact]
        public async Task GetTodo_NoTodoFound_ShouldReturnNotFound()
        {
            // ARRANGE

            var sut = new TodoItemsController(_mockService);
        
            // ACT
            var result = (NotFoundObjectResult)await sut.GetTodoItem(Guid.NewGuid());
        
            // ASSERT
            Assert.Equal(result.StatusCode, StatusCodes.Status404NotFound);;
        }
        
        // POST
        [Fact]
        public async Task PostTodo_ExistingDescription_ShouldReturnBadRequest()
        {
            // ARRANGE
            var exception = new ValidationException("Description already exists");
            var todo = new TodoItem
            {
                Description = "something",
            };
        
            _mockService.CreateTodoItem( Arg.Any<TodoItem>()).Throws(exception);
        
        
            var sut = new TodoItemsController(_mockService);
        
            // ACT
            var result = (BadRequestObjectResult) await sut.PostTodoItem(new TodoItem { Description = "test", IsCompleted = false});
        
            // ASSERT
            Assert.Equal(result.StatusCode, StatusCodes.Status400BadRequest);
            Assert.Equal(result.Value, exception.Message);;
        }
        
        [Fact]
        public async Task PostTodo_NewItem_ShouldReturnOk_WithNewlyCreatedItem()
        {
            // ARRANGE
            var description = "new description";
            var isCompleted = false;
            var newId = Guid.NewGuid();
        
            var request = new TodoItem()
            {
                Description = description,
            };
        
            var mockResponse = new TodoItem
            {
                Description = description,
                IsCompleted = isCompleted,
                Id = newId
            };

            _mockService.CreateTodoItem(Arg.Any<TodoItem>())
                .Returns(mockResponse);
        
        
            var sut = new TodoItemsController(_mockService);
        
            // ACT
            var result = (CreatedAtActionResult)await sut.PostTodoItem(new TodoItem() { Description = description, IsCompleted = isCompleted });
        
            // ASSERT
            Assert.Equal(result.StatusCode, StatusCodes.Status201Created);
            Assert.Equal(result.Value.As<TodoItem>().Description, mockResponse.Description);
            Assert.Equal(result.Value.As<TodoItem>().IsCompleted, mockResponse.IsCompleted);
            Assert.Equal(result.Value.As<TodoItem>().Id, mockResponse.Id);

            await _mockService.Received(1).CreateTodoItem(Arg.Any<TodoItem>());
        }
        
}
