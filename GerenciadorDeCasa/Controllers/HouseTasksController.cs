using GerenciadorDeCasa.Data;
using GerenciadorDeCasa.Models;
using GerenciadorDeCasa.Services;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorDeCasa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HouseTasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public HouseTasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult GetTasks()
        {
            return Ok(_taskService.GetTasks());
        }

        [HttpGet("today")]
        public ActionResult GetTodayTasks()
        {
            if (_taskService.GetTaskToday() is null)
            {
                return NotFound("Nenhuma tarefa encontrada para hoje.");
            }
            return Ok(_taskService.GetTaskToday());
        }

        [HttpPost]
        public async Task<ActionResult<HouseTask>> CreateTasks(HouseTask task)
        {
            return task == null ? BadRequest("Tarefa inválida.") : Ok(await _taskService.CreateTask(task));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTasks(int id, HouseTask updatedTask)
        {
            var result = await _taskService.UpdateTask(id, updatedTask);
            return result is null ? NotFound("Tarefa não encontrada.") : Ok(result);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTask(id);
            return result ? Ok("Tarefa deletada com sucesso.") : NotFound("Tarefa não encontrada.");
        }
    }
}
