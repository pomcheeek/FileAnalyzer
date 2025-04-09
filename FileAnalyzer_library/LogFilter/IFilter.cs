using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.LogFilter;

/// <summary>
/// Интерфейс для фильтров логов.
/// Обеспечивает методы для фильтрации списка логов и установки параметров фильтрации.
/// </summary>
public interface IFilter
{
    /// <summary>
    /// Фильтрует список логов по заданному критерию.
    /// </summary>
    /// <param name="logs">Список логов для фильтрации.</param>
    /// <returns>Список логов, удовлетворяющих условиям фильтра.</returns>
    List<Log> Filter(List<Log> logs);

    /// <summary>
    /// Устанавливает параметры фильтрации, например, запрашивает у пользователя необходимые значения.
    /// </summary>
    void SetFilterField();
}