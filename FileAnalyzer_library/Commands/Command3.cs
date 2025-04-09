using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;
using Nikolaev_RA_Project4_Var1_sideA_lib.CommandInterfaces;
using Nikolaev_RA_Project4_Var1_sideA_lib.ConsoleUI;
using Nikolaev_RA_Project4_Var1_sideA_lib.LogFilter;
using Nikolaev_RA_Project4_Var1_sideA_lib.LogParser;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.Commands;

/// <summary>
/// Команда для работы с логами, реализующая интерфейс <see cref="IWorkWithLogsCommand"/>.
/// Предоставляет функционал фильтрации логов по уровню значимости, диапазону дат и времени или поиску по ключевому слову.
/// </summary>
public class Command3 : ConsoleUiBase, IWorkWithLogsCommand
{
    /// <summary>
    /// Цвет для вывода сообщений об ошибках.
    /// </summary>
    private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;
    
    /// <summary>
    /// Массив строк, представляющий имена команд для выбора типа фильтрации логов.
    /// </summary>
    private static readonly string[] CommandsNames = new[]
    {
        "1. По уровню значимости",
        "2. Диапазон дат и времени",
        "3. Поиск по ключевому слову в сообщении"
    };

    /// <summary>
    /// Варианты вывода отфильтрованных логов.
    /// </summary>
    private static readonly string[] WriteOptions = new[]
    {
        "1. Вывести результат в файл",
        "2. Вывести результат в консоль"
    };
    
    /// <summary>
    /// Выбранная команда фильтрации.
    /// </summary>
    private int _selectedCommand;
    
    /// <summary>
    /// Выбранная команда вывода результата.
    /// </summary>
    private int _selectedWriteOption;
    
    /// <summary>
    /// Обрабатывает список логов, предоставляя пользователю возможность выбрать тип фильтрации и выводит отфильтрованные логи.
    /// </summary>
    /// <param name="logs">
    /// Список логов, которые необходимо отфильтровать.
    /// Если список <c>null</c> или пустой, выводится сообщение об ошибке.
    /// </param>
    public void CommandProcess(List<Log>? logs)
    {
        // Проверка наличия данных для фильтрации
        if (logs == null || logs.Count == 0)
        {
            PrintErrorBox("Данные некорректны или их нет", ErrorColor);
            return;
        }
        
        // Вывод меню с вариантами фильтрации и получение выбора пользователя
        _selectedCommand = Run(CommandsNames);

        // Словарь, сопоставляющий номер выбора с соответствующим фильтром
        Dictionary<int, IFilter> filters = new Dictionary<int, IFilter>()
        {
            { 0, new LevelFilter() },       // Фильтр по уровню значимости
            { 1, new DateFilter() },        // Фильтр по диапазону дат и времени
            { 2, new MessageFilter() },     // Фильтр по ключевому слову в сообщении
        };

        List<Log> filteredLogs;
        // Проверяем, существует ли фильтр для выбранного пользователем варианта
        if (filters.TryGetValue(_selectedCommand, out var filter))
        {
            // Устанавливаем параметры фильтра (например, запрашиваем у пользователя значения для фильтрации)
            filter.SetFilterField();
            // Применяем фильтр к списку логов
            filteredLogs = filter.Filter(logs);
        }
        else
        {
            // Если выбран некорректный вариант, выходим из метода
            return;
        }

        // Запрашиваем у пользователя, куда выводить результат
        _selectedWriteOption = Run(WriteOptions);
        SaveLogs(filteredLogs, _selectedWriteOption);
    }

    /// <summary>
    /// Сохраняет отфильтрованные логи в файл или выводит их в консоль.
    /// </summary>
    /// <param name="logs">Список логов для сохранения.</param>
    /// <param name="selectedOption">
    /// Опция вывода: 1 - запись в файл, 2 - вывод в консоль.
    /// </param>
    public void SaveLogs(List<Log> logs, int selectedOption)
    {
        WriteLog writeLog = new WriteLog();
        switch (selectedOption + 1)
        {
            case 0:
                return;
            case 1:
                writeLog.Write(logs, true); // Запись в файл
                break;
            case 2:
                writeLog.Write(logs, false); // Вывод в консоль
                break;
        }
    }
}
