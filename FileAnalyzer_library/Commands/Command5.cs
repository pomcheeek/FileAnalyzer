using System.Text;
using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;
using Nikolaev_RA_Project4_Var1_sideA_lib.CommandInterfaces;
using Nikolaev_RA_Project4_Var1_sideA_lib.ConsoleUI;
using Nikolaev_RA_Project4_Var1_sideA_lib.LogParser;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.Commands;

/// <summary>
/// Команда для анализа логов, реализующая интерфейс <see cref="IWorkWithLogsCommand"/>.
/// Выполняет анализ логов и выводит статистику, такую как количество записей, распределение по уровням, 
/// временные метки и среднюю длину сообщения.
/// </summary>
public class Command5 : ConsoleUiBase, IWorkWithLogsCommand
{
    /// <summary>
    /// Цвет заголовков статистики.
    /// </summary>
    private static readonly ConsoleColor HeaderColor = ConsoleColor.White;

    /// <summary>
    /// Цвет ошибок.
    /// </summary>
    private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;

    /// <summary>
    /// Цвет успеха (например, при успешной записи в файл).
    /// </summary>
    private static readonly ConsoleColor SuccessColor = ConsoleColor.Green;

    /// <summary>
    /// Цвет информационных блоков.
    /// </summary>
    private static readonly ConsoleColor BoxColor = ConsoleColor.White;
    
    /// <summary>
    /// Выбранный вариант вывода статистики.
    /// </summary>
    private int _selectedWriteOption;
    
    /// <summary>
    /// Массив возможных вариантов вывода результата.
    /// </summary>
    private static readonly string[] WriteOptions = new[]
    {
        "1. Вывести результат в файл",
        "2. Вывести результат в консоль"
    };
    
    /// <summary>
    /// Выполняет анализ логов и выводит результаты в консоль или файл.
    /// </summary>
    /// <param name="logs">Список логов для анализа. Если список <c>null</c> или пустой, выводится сообщение об ошибке.</param>
    public void CommandProcess(List<Log>? logs)
    {
        // Устанавливаем кодировку UTF-8 для корректного отображения символов в консоли.
        Console.OutputEncoding = Encoding.UTF8;

        // Проверяем наличие данных для анализа
        if (logs == null || logs.Count == 0)
        {
            PrintErrorBox("Данные некорректны или отсутствуют", ErrorColor);
            return;
        }

        // Запрос у пользователя варианта вывода статистики (в консоль или файл)
        _selectedWriteOption = Run(WriteOptions);
        WriteResult(logs, _selectedWriteOption);
    }

    /// <summary>
    /// Определяет способ вывода статистики.
    /// </summary>
    /// <param name="logs">Список логов.</param>
    /// <param name="selectedOption">Выбранный вариант вывода.</param>
    private void WriteResult(List<Log> logs, int selectedOption)
    {
        switch (selectedOption + 1)
        {
            case 0:
                return;
            case 1:
                WriteInFile(logs);
                break;
            case 2:
                WriteInConsole(logs);
                break;
        }
    }

    /// <summary>
    /// Записывает статистику в файл.
    /// </summary>
    /// <param name="logs">Список логов.</param>
    private void WriteInFile(List<Log> logs)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Количество записей: {logs.Count}");
        sb.AppendLine();
        
        sb.AppendLine("Статистика по уровням важности: ");
        var levels = GetLevelsLogsCount(logs);
        foreach (var pair in levels)
        {
            sb.AppendLine($"\t{pair.Key}: {pair.Value}");
        }
        sb.AppendLine();
        
        sb.AppendLine($"Самая ранняя: \n{GetEarliestTime(logs)}");
        sb.AppendLine($"Самая поздняя: \n{GetLatestTime(logs)}");
        sb.AppendLine();
        
        sb.AppendLine($"Средняя длина сообщения: {GetAverageLength(logs)}");

        WriteStringInFile(sb.ToString());
    }

    /// <summary>
    /// Запрашивает у пользователя путь и записывает статистику в файл.
    /// </summary>
    /// <param name="output">Форматированный текст для записи.</param>
    private void WriteStringInFile(string output)
    {
        Console.Write("Введите полный путь до файла: ");
        string? outputPath = Console.ReadLine();

        if (string.IsNullOrEmpty(outputPath))
        {
            PrintErrorBox("Указан некорректный путь до файла", ErrorColor);
            return;
        }
            
        try
        {
            // Записываем данные в файл
            File.WriteAllText(outputPath, output, Encoding.UTF8);
            PrintErrorBox("Запись в файл выполнена успешно.", SuccessColor);
        }
        catch (Exception ex)
        {
            PrintErrorBox($"Ошибка записи в файл: {ex.Message}. Попробуйте еще раз", ErrorColor);
        }
    }

    /// <summary>
    /// Выводит статистику в консоль.
    /// </summary>
    /// <param name="logs">Список логов.</param>
    private void WriteInConsole(List<Log> logs)
    {
        PrintHeader("Анализ логов", HeaderColor);
        
        PrintInfoBox($"Количество записей: {logs.Count}", BoxColor);
        
        var levels = GetLevelsLogsCount(logs);
        PrintLevelsStats(levels, logs.Count, BoxColor);
        
        PrintTimestamps(GetEarliestTime(logs), GetLatestTime(logs), BoxColor);
        
        PrintAverageLength(GetAverageLength(logs), BoxColor);
        
        PrintFooter(HeaderColor);
    }
    
    /// <summary>
    /// Подсчитывает количество логов для каждого уровня значимости.
    /// </summary>
    /// <param name="logs">Список логов.</param>
    /// <returns>Словарь, где ключ – уровень логирования, а значение – количество записей.</returns>
    private Dictionary<string, int> GetLevelsLogsCount(List<Log> logs)
    {
        return logs.GroupBy(log => log.Level)
                   .ToDictionary(group => group.Key, group => group.Count());
    }

    /// <summary>
    /// Находит лог с самой ранней временной меткой.
    /// </summary>
    /// <param name="logs">Список логов.</param>
    /// <returns>Лог с самой ранней датой или <c>null</c>, если список пуст.</returns>
    private Log? GetEarliestTime(List<Log> logs)
    {
        return logs.OrderBy(log => log.Date).FirstOrDefault();
    }

    /// <summary>
    /// Находит лог с самой поздней временной меткой.
    /// </summary>
    /// <param name="logs">Список логов.</param>
    /// <returns>Лог с самой поздней датой или <c>null</c>, если список пуст.</returns>
    private Log? GetLatestTime(List<Log> logs)
    {
        return logs.OrderByDescending(log => log.Date).FirstOrDefault();
    }

    /// <summary>
    /// Вычисляет среднюю длину сообщения по всем логам.
    /// </summary>
    /// <param name="logs">Список логов.</param>
    /// <returns>Средняя длина сообщения.</returns>
    private double GetAverageLength(List<Log> logs)
    {
        if (!logs.Any()) return 0;
        
        return logs.Average(log => log.Message.Length);
    }

    /// <summary>
    /// Сохраняет отфильтрованные логи в файл или выводит их в консоль.
    /// </summary>
    /// <param name="logs">Список логов для сохранения.</param>
    /// <param name="selectedOption">Опция вывода: 1 - запись в файл, 2 - вывод в консоль.</param>
    public void SaveLogs(List<Log> logs, int selectedOption)
    {
        WriteLog writeLog = new WriteLog();
        switch (selectedOption + 1)
        {
            case 0:
                return;
            case 1:
                writeLog.Write(logs, true);
                break;
            case 2:
                writeLog.Write(logs, false);
                break;
        }
    }
}
