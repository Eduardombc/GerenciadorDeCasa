using GerenciadorDeCasa.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorDeCasa.Models;

public class HouseTask
{

    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    public DateTime? SpecificDate { get; set; }
    public TimeSpan? DueTime { get; set; }

    //Configuração de Recorrência

    public FrequencyType Frequency { get; set; }
    public List<DayOfWeek> RecurrenceDays { get; set; }

    public string ResponsiblePersonContact { get; set; }
    public bool IsCompleted { get; set; }
}