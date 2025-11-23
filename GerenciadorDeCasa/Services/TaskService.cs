using GerenciadorDeCasa.Data;
using GerenciadorDeCasa.Models;
using GerenciadorDeCasa.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace GerenciadorDeCasa.Services;

public class TaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    // Lista todas as tarefas
    public async Task<IEnumerable<HouseTask>> GetTasks()
    {
        return await _context.HouseTasks.ToListAsync();
    }

    //Cria uma nova tarefa
    public async Task<HouseTask> CreateTask(HouseTask task)
    {
        _context.HouseTasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    //Atualiza uma tarefa existente
    public async Task<HouseTask?> UpdateTask(int id, HouseTask updatedTask)
    {
        var task = await _context.HouseTasks.FindAsync(id);
        if (task == null) return null;
        task.Title = updatedTask.Title;
        task.Description = updatedTask.Description;
        task.SpecificDate = updatedTask.SpecificDate;
        task.DueTime = updatedTask.DueTime;
        task.Frequency = updatedTask.Frequency;
        task.RecurrenceDays = updatedTask.RecurrenceDays;
        task.ResponsiblePersonContact = updatedTask.ResponsiblePersonContact;
        task.IsCompleted = updatedTask.IsCompleted;
        await _context.SaveChangesAsync();
        return task;
    }

    //Deleta uma tarefa
    public async Task<bool> DeleteTask(int id)
    {
        var task = await _context.HouseTasks.FindAsync(id);
        if (task == null) return false;
        _context.HouseTasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }

    //Busca por task por dia
    public async Task<IEnumerable<HouseTask>> GetTaskToday()
    {
        var today = DateTime.Today;
        var currentDayOfWeek = today.DayOfWeek;
        var allTasks = await _context.HouseTasks.ToListAsync();

        var tasksForToday = allTasks.Where(t =>
        {
            // Caso 1: É tarefa diária?
            if (t.Frequency == FrequencyType.Daily) return true;

            // Caso 2: É tarefa semanal E hoje é um dos dias agendados?
            if (t.Frequency == FrequencyType.Weekly && t.RecurrenceDays.Contains(currentDayOfWeek)) return true;

            // Caso 3: É tarefa única E a data é hoje?
            if (t.Frequency == FrequencyType.OneTime && t.SpecificDate.HasValue && t.SpecificDate.Value.Date == today) return true;

            return false;
        }).ToList();
        return tasksForToday;
    }

    //Busca tarefas de hoje por hora pro TaskReminder

    public async Task<IEnumerable<HouseTask>> GetTasksByTime(CancellationToken stoppingToken)
    {
        // 1. Obtemos a hora atual (Hora e Minuto apenas)
        var now = DateTime.Now;
        var currentTime = new TimeSpan(now.Hour, now.Minute, 0);

        // 2. Buscamos as tarefas (Filtragem em Memória pela simplicidade)
        var allTasks = await _context.HouseTasks.ToListAsync(stoppingToken);

        var tasksDueNow = allTasks.Where(t =>
        {
            // A) Verifica se o horário bate (ignorando segundos)
            if (t.DueTime == null || t.DueTime.Value.Hours != now.Hour || t.DueTime.Value.Minutes != now.Minute)
                return false;

            // B) Verifica se é para HOJE (Mesma lógica do Controller)
            if (t.Frequency == FrequencyType.Daily) return true;
            if (t.Frequency == FrequencyType.Weekly && t.RecurrenceDays.Contains(now.DayOfWeek)) return true;
            if (t.Frequency == FrequencyType.OneTime && t.SpecificDate.HasValue && t.SpecificDate.Value.Date == now.Date) return true;

            return false;
        }).ToList();

        return tasksDueNow;
    }
}
