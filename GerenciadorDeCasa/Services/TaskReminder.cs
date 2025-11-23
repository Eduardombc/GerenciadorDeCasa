
namespace GerenciadorDeCasa.Services;

public class TaskReminder : BackgroundService
{
    private readonly ILogger<TaskReminder> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public TaskReminder(ILogger<TaskReminder> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("👷 Lembretes iniciado!");

        while (!stoppingToken.IsCancellationRequested)
        {
            // Visualiza a hora atual no log
            _logger.LogInformation($"⏰ Verificando tarefas às: {DateTime.Now:HH:mm:ss}");
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var taskService = scope.ServiceProvider.GetRequiredService<TaskService>();
                    var listaDeTask = await taskService.GetTasksByTime(stoppingToken);

                    // LOG NOVO: Ajuda a saber se o banco foi consultado com sucesso
                    if (listaDeTask.Any())
                    {
                        _logger.LogInformation($"✅ Encontradas {listaDeTask.Count()} tarefas para agora.");
                    }
                    else
                    {
                        // Log opcional para não poluir muito, mas útil agora
                        _logger.LogInformation("ℹ️ Nenhuma tarefa para este minuto.");
                    }

                    foreach (var task in listaDeTask)
                    {
                        _logger.LogWarning($"🔥 ALERTA! ENVIAR NOTIFICAÇÃO: '{task.Title}' para {task.ResponsiblePersonContact}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro ao verificar tarefas.");
            }


            // Aguarda 15 minutos antes de verificar novamente
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }
}
