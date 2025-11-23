using System.Net;
using System.Net.Mail;

namespace GerenciadorDeCasa.Services.Notifications;

public class EmailNotificationService : INotificationService
{
    private readonly ILogger<EmailNotificationService> _logger;
    private readonly IConfiguration _configuration;

    public EmailNotificationService(ILogger<EmailNotificationService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task SendNotificationAsync(string contact, string title, string message)
    {
        // Recupera configurações do appsettings.json
        var myEmail = _configuration["EmailSettings:Email"];
        var myPassword = _configuration["EmailSettings:Password"]; // Senha de App, não a do login!

        if (string.IsNullOrEmpty(myEmail) || string.IsNullOrEmpty(myPassword))
        {
            _logger.LogError("❌ Configurações de E-mail não encontradas!");
            return;
        }

        try
        {
            // Configura o cliente SMTP (O carteiro)
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(myEmail, myPassword);

                // Cria a carta
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(myEmail),
                    Subject = $"🏠 Lembrete: {title}",
                    Body = $"Olá! \n\nLembrete da tarefa: {title}\nDetalhes: {message}\n\nAtenciosamente,\nGerenciador de Casa",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(contact); // O email do Eduardo que veio do banco

                // Envia
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"📧 E-mail enviado com sucesso para {contact}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Falha ao enviar e-mail.");
        }
    }
}