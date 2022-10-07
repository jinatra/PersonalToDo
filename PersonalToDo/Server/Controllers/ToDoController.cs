using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalToDo.Server.Services;

namespace PersonalToDo.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoService toDoService;

        public ToDoController(ToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        // ToDoItem 생성
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> CreateToDo(ToDoItem toDoItem)
        {
            await toDoService.CreateToDo(toDoItem);

            return Ok(toDoItem);
        }

        // ToDoItem List 가져오기
        [HttpGet]
        public async Task<List<ToDoItem>> GetToDos()
        {
            return await toDoService.GetToDos();
        }
    }
}
