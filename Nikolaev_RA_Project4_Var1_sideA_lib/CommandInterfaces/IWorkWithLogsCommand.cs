using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.CommandInterfaces;

/// <summary>
/// Интерфейс для команд, осуществляющих работу с логами.
/// Предоставляет методы для обработки и сохранения списка логов.
/// </summary>
public interface IWorkWithLogsCommand
{
    /// <summary>
    /// Обрабатывает переданный список логов.
    /// </summary>
    /// <param name="logs">
    /// Список логов для обработки. Может быть <c>null</c> в случае отсутствия логов.
    /// </param>
    void CommandProcess(List<Log>? logs);
    
    /// <summary>
    /// Сохраняет список логов в соответствии с выбранным вариантом.
    /// </summary>
    /// <param name="logs">Список логов, который необходимо сохранить.</param>
    /// <param name="selectedOption">
    /// Опция сохранения, определяющая способ сохранения данных.
    /// Например, запись в файл или консольный вывод.
    /// </param>
    void SaveLogs(List<Log> logs, int selectedOption);
}