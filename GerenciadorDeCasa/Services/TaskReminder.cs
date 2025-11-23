
using GerenciadorDeCasa.Services.Notifications;

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
                    var notifier = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    foreach (var task in listaDeTask)
                    {
                        _logger.LogWarning($"🔥 Processando tarefa: {task.Title}");

                        // 2. Enviamos de verdade!
                        await notifier.SendNotificationAsync(
                            task.ResponsiblePersonContact, // O email que está no banco
                            task.Title,
                            task.Description
                        );
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
