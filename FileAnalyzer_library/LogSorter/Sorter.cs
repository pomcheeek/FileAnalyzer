using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.LogSorter
{
    /// <summary>
    /// Класс для сортировки логов.
    /// Предоставляет методы для сортировки по возрастанию и убыванию по заданному полю.
    /// </summary>
    public class Sorter
    {
        /// <summary>
        /// Сортирует список логов по заданному полю в порядке возрастания.
        /// </summary>
        /// <param name="logs">Список логов для сортировки.</param>
        /// <param name="logField">
        /// Поле для сортировки: "date" - по дате, "level" - по уровню важности, "message" - по длине сообщения.
        /// Регистр игнорируется.
        /// </param>
        /// <returns>Отсортированный список логов.</returns>
        public List<Log> AscendingSort(List<Log> logs, string logField)
        {
            // Используем switch expression для выбора способа сортировки в зависимости от logField
            return logField.ToLower() switch
            {
                "date" => logs.OrderBy(log => log.Date).ToList(), // Сортировка по дате
                "level" => logs.OrderBy(log => log.Level).ToList(), // Сортировка по уровню важности
                "message" => logs.OrderBy(log => log.Message.Length).ToList(), // Сортировка по длине сообщения
                _ => logs // Если поле не найдено, возвращаем список без изменений
            };
        }

        /// <summary>
        /// Сортирует список логов по заданному полю в порядке убывания.
        /// </summary>
        /// <param name="logs">Список логов для сортировки.</param>
        /// <param name="logField">
        /// Поле для сортировки: "date" - по дате, "level" - по уровню важности, "message" - по длине сообщения.
        /// Регистр игнорируется.
        /// </param>
        /// <returns>Отсортированный список логов.</returns>
        public List<Log> DescendingSort(List<Log> logs, string logField)
        {
            // Используем switch expression для выбора способа сортировки в зависимости от logField
            return logField.ToLower() switch
            {
                "date" => logs.OrderByDescending(log => log.Date).ToList(), // Сортировка по дате в обратном порядке
                "level" => logs.OrderByDescending(log => log.Level).ToList(), // Сортировка по уровню важности в обратном порядке
                "message" => logs.OrderByDescending(log => log.Message.Length).ToList(), // Сортировка по длине сообщения в обратном порядке
                _ => logs // Если поле не найдено, возвращаем список без изменений
            };
        }
    }
}
