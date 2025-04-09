using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;
using Nikolaev_RA_Project4_Var1_sideA_lib.CommandInterfaces;
using Nikolaev_RA_Project4_Var1_sideA_lib.ConsoleUI;
using Nikolaev_RA_Project4_Var1_sideA_lib.LogParser;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.Commands;

/// <summary>
/// Команда для вывода или сохранения логов, реализующая интерфейс <see cref="IWorkWithLogsCommand"/>.
/// Позволяет пользователю выбрать между выводом логов в консоль или записью их в файл.
/// </summary>
public class Command6 : ConsoleUiBase, IWorkWithLogsCommand
{
    /// <summary>
    /// Цвет для вывода сообщений об ошибках.
    /// </summary>
    private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;
    
    /// <summary>
    /// Выбранный пользователем вариант (индекс выбора).
    /// </summary>
    private int _selectedOption;
    
    /// <summary>
    /// Массив строк с вариантами команд:
    /// 1. Вывести данные в консоль
    /// 2. Записать данные в файл
    /// </summary>
    private static readonly string[] CommandsNames = new[]
    {
        "1. Вывести данные в консоль",
        "2. Записать данные в файл"
    };
    
    /// <summary>
    /// Выполняет обработку логов в зависимости от выбора пользователя.
    /// Если логи недоступны или список пустой, выводит сообщение об ошибке.
    /// </summary>
    /// <param name="logs">Список логов для обработки.</param>
    public void CommandProcess(List<Log>? logs)
    {
        // Проверка наличия данных для обработки
        if (logs == null || logs.Count == 0)
        {
            PrintErrorBox("Данные некорректны или их нет", ErrorColor);
            return;
        }
        
        // Получение выбора пользователя (вывод в консоль или запись в файл)
        _selectedOption = Run(CommandsNames);

        // Если пользователь отменяет выбор, выходим из метода
        if (_selectedOption == -1)
        {
            return;
        }
        
        // Очистка консоли перед выводом результатов
        Console.Clear();
        
        // Создаем экземпляр класса для вывода логов
        WriteLog writer = new();
        
        // Определяем, куда сохранить логи:
        // Если _selectedOption равен 1, записываем в файл, иначе выводим в консоль.
        writer.Write(logs, _selectedOption == 1);
    }

    /// <summary>
    /// Сохраняет список логов в консоль или в файл в зависимости от выбранного варианта.
    /// </summary>
    /// <param name="logs">Список логов для сохранения.</param>
    /// <param name="selectedOption">Опция вывода: 1 - консоль, 2 - файл.</param>
    public void SaveLogs(List<Log> logs, int selectedOption)
    {
        WriteLog writer = new();
        switch (selectedOption + 1)
        {
            case 0:
                return;
            case 1:
                writer.Write(logs, false); // Вывод в консоль
                break;
            case 2:
                writer.Write(logs, true); // Запись в файл
                break;
        }
    }
}
