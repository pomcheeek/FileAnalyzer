namespace Nikolaev_RA_Project4_Var1_sideA_lib.CLog
{
    /// <summary>
    /// Класс, представляющий лог-запись.
    /// Содержит информацию о дате, уровне важности, сообщении и имени файла, откуда был получен лог.
    /// </summary>
    public class Log
    {
        // Приватные поля для хранения данных лога (не используются напрямую, а доступны через свойства)
        private DateTime _date;
        private string _level;
        private string _message;
        private string _fileName;

        /// <summary>
        /// Дата и время создания лог-записи.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Уровень важности лог-записи (например, Info, Warning, Error).
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Сообщение, содержащее подробности лог-записи.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Имя файла, из которого была считана лог-запись.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Возвращает строковое представление лог-записи, содержащее информацию о файле, дате, уровне и сообщении.
        /// </summary>
        /// <returns>Строка с форматированным представлением лог-записи.</returns>
        public override string ToString()
        {
            // Форматируем строку для удобного чтения информации о логе
            return $"\tФайл: {FileName}\n" +
                   $"\tДата: {Date}\n" +
                   $"\tУровень важности: {Level} \n" +
                   $"\tСообщение: {Message}";
        }
    }
}