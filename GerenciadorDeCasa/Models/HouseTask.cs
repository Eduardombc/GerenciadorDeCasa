using GerenciadorDeCasa.Models.Enums;

namespace GerenciadorDeCasa.Models;

public class HouseTask
{
    public HouseTask(int id, string title, string description, DateTime? specificDate, TimeSpan? dueTime, FrequencyType frequency, List<DayOfWeek> recurrenceDays, string responsiblePersonContact, bool isCompleted)
    {
        Id = id;
        Title = title;
        Description = description;
        SpecificDate = specificDate;
        DueTime = dueTime;
        Frequency = frequency;
        RecurrenceDays = recurrenceDays;
        ResponsiblePersonContact = responsiblePersonContact;
        IsCompleted = isCompleted;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? SpecificDate { get; set; }
    public TimeSpan? DueTime { get; set; }

    //Configuração de Recorrência
    public FrequencyType Frequency { get; set; }
    public List<DayOfWeek> RecurrenceDays { get; set; }

    public string ResponsiblePersonContact { get; set; }
    public bool IsCompleted { get; set; }

}