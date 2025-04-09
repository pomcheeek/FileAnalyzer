using System.Globalization;
using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;
using Nikolaev_RA_Project4_Var1_sideA_lib.ConsoleUI;
using Nikolaev_RA_Project4_Var1_sideA_lib.LogConfig;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.LogFilter;

/// <summary>
/// Фильтр для логов по диапазону дат.
/// Позволяет задать начальную и конечную дату, и возвращает лог-записи, попадающие в указанный диапазон.
/// </summary>
public class DateFilter : ConsoleUiBase, IFilter
{
    /// <summary>
    /// Начальная дата для фильтрации.
    /// </summary>
    private DateTime _startTime;

    /// <summary>
    /// Конечная дата для фильтрации.
    /// </summary>
    private DateTime _endTime;

    /// <summary>
    /// Цвет для вывода сообщений об ошибках.
    /// </summary>
    private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;
    
    /// <summary>
    /// Фильтрует список логов, возвращая только те записи, дата которых находится в заданном диапазоне.
    /// </summary>
    /// <param name="logEntries">Список логов для фильтрации.</param>
    /// <returns>Список логов, удовлетворяющих условию фильтрации по дате.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если logEntries равен null.</exception>
    public List<Log> Filter(List<Log> logEntries)
    {
        if (logEntries == null) 
            throw new ArgumentNullException(nameof(logEntries));
        
        // Если начальная и конечная даты равны, фильтрация не применяется и возвращается исходный список
        if (_startTime == _endTime) 
            return logEntries;
        
        // Фильтруем записи, оставляя только те, что попадают в указанный диапазон
        return logEntries.Where(entry => entry.Date >= _startTime && entry.Date <= _endTime).ToList();
    }

    /// <summary>
    /// Устанавливает параметры фильтрации, запрашивая у пользователя начальную и конечную дату.
    /// Использует формат даты из конфигурационного файла.
    /// </summary>
    public void SetFilterField()
    {
        // Получаем текущую конфигурацию для определения формата даты
        ConfigGetter getter = new ConfigGetter();
        ConfigEntry? config = getter.GetConfig();
        if (config == null) return;

        string dateFormat = config.DateFormat;
        
        // Запрос начальной даты у пользователя
        Console.WriteLine($"Введите начальную дату (формат: {dateFormat}):");
        string? startInput = Console.ReadLine();
        // Запрос конечной даты у пользователя
        Console.WriteLine($"Введите конечную дату (формат: {dateFormat}):");
        string? endInput = Console.ReadLine();

        // Попытка распарсить начальную дату по заданному формату
        if (DateTime.TryParseExact(startInput, dateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime start))
        {
            _startTime = start;
        }
        else
        {
            // Если парсинг неудачен, выводим сообщение об ошибке и оставляем значение по умолчанию (DateTime.MinValue)
            PrintErrorBox("Неверный формат начальной даты. Используем значение по умолчанию.", ErrorColor);
        }

        // Попытка распарсить конечную дату по заданному формату
        if (DateTime.TryParseExact(endInput, dateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime end))
        {
            _endTime = end;
        }
        else
        {
            // Если парсинг неудачен, выводим сообщение об ошибке и оставляем значение по умолчанию (DateTime.MinValue)
            PrintErrorBox("Неверный формат конечной даты. Используем значение по умолчанию.", ErrorColor);
        }
    }
}
