using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.CommandInterfaces;

/// <summary>
/// Интерфейс для команды сохранения логов.
/// Предназначен для реализации функционала сохранения логов в проекте.
/// </summary>
public interface ISaveLogsCommand
{
    /// <summary>
    /// Сохраняет логи.
    /// </summary>
    /// <returns>
    /// Список логов, которые были сохранены. Если сохранение не выполнено, возвращается <c>null</c>.
    /// </returns>
    List<Log>? SaveLogs();
}