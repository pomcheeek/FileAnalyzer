using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.LogFilter;

/// <summary>
/// Фильтр логов по содержимому сообщения.
/// Позволяет фильтровать записи, содержащие заданное ключевое слово.
/// </summary>
public class MessageFilter : IFilter
{
    /// <summary>
    /// Ключевое слово для фильтрации сообщений.
    /// </summary>
    private string _word;
    
    /// <summary>
    /// Фильтрует список логов, возвращая те записи, в которых сообщение содержит указанное слово.
    /// </summary>
    /// <param name="logEntries">Список логов для фильтрации.</param>
    /// <returns>Отфильтрованный список логов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если logEntries равен null.</exception>
    public List<Log> Filter(List<Log> logEntries)
    {
        if (logEntries == null)
            throw new ArgumentNullException(nameof(logEntries));
        
        // Если слово для фильтрации не задано, возвращаем исходный список логов без изменений.
        if (string.IsNullOrWhiteSpace(_word))
            return logEntries;
        
        // Фильтруем записи, оставляя те, в которых сообщение содержит ключевое слово.
        return logEntries.Where(entry => entry.Message.Contains(_word)).ToList();
    }

    /// <summary>
    /// Запрашивает у пользователя ключевое слово для фильтрации сообщений.
    /// </summary>
    public void SetFilterField()
    {
        Console.WriteLine("Введите слово для фильтрации сообщений:");
        // Считываем слово из консоли. Если ввод пустой, устанавливаем пустую строку.
        _word = Console.ReadLine() ?? string.Empty;
    }
}