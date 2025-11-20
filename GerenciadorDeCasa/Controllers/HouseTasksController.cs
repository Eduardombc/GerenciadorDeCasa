using Microsoft.AspNetCore.Mvc;

namespace GerenciadorDeCasa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HouseTasksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetTasks()
        {
            // Lógica para obter as tarefas
            return Ok(new { Message = "Lista de tarefas" });
        }

        [HttpPost]
        public IActionResult CreateTask()
        {
            // Lógica para criar uma nova tarefa
            return Ok(new { Message = "Tarefa criada" });
        }
    }
}
