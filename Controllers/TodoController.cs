using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Todo_List_API.Commands.Requests.Todo;
using Todo_List_API.Services;

namespace Todo_List_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private string _userId { get { return User.FindFirstValue(ClaimTypes.NameIdentifier); } }
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTodoRequest request)
        {
            var response = await _todoItemService.Create(request, _userId);

            if (response.HttpResponse != HttpStatusCode.Created)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateTodoRequest request, Guid id)
        {
            var response = await _todoItemService.Update(request, id);

            if (response.HttpResponse != HttpStatusCode.OK)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _todoItemService.Delete(id);

            if (response.HttpResponse != HttpStatusCode.OK)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _todoItemService.Get(id);

            if (response.HttpResponse != HttpStatusCode.OK)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? search, int take = 10, int skip = 0)
        {
            var response = await _todoItemService.GetAll(take, skip, search);

            if (response.HttpResponse != HttpStatusCode.OK)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
