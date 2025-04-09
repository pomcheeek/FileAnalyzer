using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;
using Nikolaev_RA_Project4_Var1_sideA_lib.CommandInterfaces;
using Nikolaev_RA_Project4_Var1_sideA_lib.LogParser;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.Commands;

/// <summary>
/// Команда для сохранения логов, реализующая интерфейс <see cref="ISaveLogsCommand"/>.
/// Использует класс <see cref="ReadLog"/> для чтения логов.
/// </summary>
public class Command1 : ISaveLogsCommand
{
    // Метод CommandProcess закомментирован, так как ранее мог использоваться для альтернативного подхода обработки логов.
    // public void CommandProcess(ref List<LogEntry>? logs)
    // {
    //     logs = ReadLog.Read();
    // }

    /// <summary>
    /// Сохраняет логи, считывая их с помощью экземпляра класса <see cref="ReadLog"/>.
    /// </summary>
    /// <returns>
    /// Список логов, полученных с помощью <see cref="ReadLog"/>, или <c>null</c>, если логи не удалось получить.
    /// </returns>
    public List<Log>? SaveLogs()
    {
        // Создаем экземпляр класса для чтения логов.
        ReadLog reader = new ReadLog();
        // Считываем и возвращаем список логов.
        return reader.Read();
    }
}