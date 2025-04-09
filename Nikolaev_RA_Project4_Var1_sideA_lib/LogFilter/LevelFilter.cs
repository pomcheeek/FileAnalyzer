using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.LogFilter;

/// <summary>
/// Фильтр логов по уровню важности.
/// Позволяет пользователю задать один или несколько уровней для фильтрации логов.
/// </summary>
public class LevelFilter : IFilter
{
    /// <summary>
    /// Список уровней важности, по которым будут отфильтрованы логи.
    /// </summary>
    private List<string>? _levels;
    
    /// <summary>
    /// Фильтрует список логов, оставляя только те записи, у которых уровень важности содержится в списке заданных уровней.
    /// </summary>
    /// <param name="logEntries">Список логов для фильтрации.</param>
    /// <returns>Отфильтрованный список логов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если logEntries равен null.</exception>
    public List<Log> Filter(List<Log> logEntries)
    {
        if (logEntries == null)
            throw new ArgumentNullException(nameof(logEntries));

        // Если уровни не заданы, возвращаем исходный список логов без фильтрации.
        if (_levels == null || _levels.Count == 0)
            return logEntries;

        // Фильтруем лог-записи, оставляя те, у которых уровень содержится в списке _levels.
        return logEntries.Where(entry => _levels.Contains(entry.Level)).ToList();
    }

    /// <summary>
    /// Запрашивает у пользователя ввод уровней важности для фильтрации.
    /// Пользователь вводит уровни через запятую, после чего они сохраняются в поле _levels.
    /// </summary>
    public void SetFilterField()
    {
        Console.WriteLine("Введите уровни важности через запятую (например, INFO,ERROR):");
        string? input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
        {
            // Разбиваем строку по запятым, убираем лишние пробелы и формируем список уровней.
            _levels = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(x => x.Trim())
                           .ToList();
        }
    }
}
