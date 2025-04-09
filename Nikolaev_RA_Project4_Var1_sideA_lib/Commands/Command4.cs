using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;
using Nikolaev_RA_Project4_Var1_sideA_lib.CommandInterfaces;
using Nikolaev_RA_Project4_Var1_sideA_lib.ConsoleUI;
using Nikolaev_RA_Project4_Var1_sideA_lib.LogParser;
using Nikolaev_RA_Project4_Var1_sideA_lib.LogSorter;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.Commands;

/// <summary>
/// Команда для сортировки логов, реализующая интерфейс <see cref="IWorkWithLogsCommand"/>.
/// Позволяет пользователю отсортировать логи по дате и времени, уровню важности или длине сообщения
/// в порядке возрастания или убывания.
/// </summary>
public class Command4 : ConsoleUiBase, IWorkWithLogsCommand
{
    /// <summary>
    /// Цвет, используемый для вывода сообщений об ошибках.
    /// </summary>
    private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;
    
    /// <summary>
    /// Массив вариантов выбора типа сортировки логов.
    /// </summary>
    private static readonly string[] CommandsNames = new[]
    {
        "1. Сортировка по дате и времени",
        "2. Сортировка по уровню важности",
        "3. Сортировка по длине сообщения"
    };

    /// <summary>
    /// Варианты вывода отсортированных логов.
    /// </summary>
    private static readonly string[] WriteOptions = new[]
    {
        "1. Вывести результат в файл",
        "2. Вывести результат в консоль"
    };
    
    /// <summary>
    /// Массив вариантов порядка сортировки.
    /// </summary>
    private static readonly string[] Options = new[]
    {
        "По возрастанию ( ↑ )",
        "По убыванию ( ↓ )"
    };

    /// <summary>
    /// Выбранный тип сортировки (индекс в массиве <see cref="CommandsNames"/>).
    /// </summary>
    private int _selectedCommand;
    
    /// <summary>
    /// Выбранный порядок сортировки (индекс в массиве <see cref="Options"/>).
    /// </summary>
    private int _selectedOption;
    
    /// <summary>
    /// Выбранный вариант вывода результата сортировки.
    /// </summary>
    private int _selectedWriteOption;

    /// <summary>
    /// Выполняет сортировку логов в зависимости от выбора пользователя.
    /// Если список логов <c>null</c>, выводит сообщение об ошибке.
    /// </summary>
    /// <param name="logs">Список логов для сортировки.</param>
    public void CommandProcess(List<Log>? logs)
    {
        // Проверяем, что список логов не пуст
        if (logs == null)
        {
            PrintErrorBox("Данные некорректны или их нет", ErrorColor);
            return;
        }
        
        // Получаем от пользователя параметры сортировки
        GetCurrentOption();
        
        // Создаём экземпляр класса сортировки
        Sorter sorter = new Sorter();

        // Определяем, по какому полю будет производиться сортировка
        string logField = string.Empty;
        switch (_selectedCommand + 1)
        {
            case 1:
                logField = "date"; // Сортировка по дате и времени
                break;
            case 2:
                logField = "level"; // Сортировка по уровню важности
                break;
            case 3:
                logField = "message"; // Сортировка по длине сообщения
                break;
            case 0:
                return; // Выход, если выбор не был сделан
        }

        // Список для хранения отсортированных логов
        List<Log> sortedLogs = new List<Log>();

        // Определяем порядок сортировки (по возрастанию или убыванию)
        switch (_selectedOption + 1)
        {
            case 1:
                sortedLogs = sorter.AscendingSort(logs, logField); // Сортировка по возрастанию
                break;
            case 2:
                sortedLogs = sorter.DescendingSort(logs, logField); // Сортировка по убыванию
                break;
            case -1:
                return; // Выход, если выбор не был сделан
        }

        // Запрашиваем у пользователя, куда вывести результат
        _selectedWriteOption = Run(WriteOptions);
        SaveLogs(sortedLogs, _selectedWriteOption);
    }
    
    /// <summary>
    /// Получает выбор пользователя для типа и порядка сортировки.
    /// Сначала запрашивает тип сортировки, затем порядок сортировки.
    /// </summary>
    private void GetCurrentOption()
    {
        // Запрос выбора типа сортировки (по дате, уровню или длине сообщения)
        _selectedCommand = Run(CommandsNames);
        
        // Если выбор был отменён, выходим
        if (_selectedCommand == -1)
        {
            return;
        }
        
        // Запрос выбора порядка сортировки (по возрастанию или убыванию)
        _selectedOption = Run(Options);
    }
    
    /// <summary>
    /// Сохраняет отсортированные логи в файл или выводит их в консоль.
    /// </summary>
    /// <param name="logs">Список отсортированных логов.</param>
    /// <param name="selectedOption">Опция вывода: 1 - в файл, 2 - в консоль.</param>
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
