using GerenciadorDeCasa.Data;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorDeCasa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HouseTasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HouseTasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
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
