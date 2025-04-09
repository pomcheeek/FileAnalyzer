using System.Security;
using System.Text;
using System.Text.Json;
using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;
using Nikolaev_RA_Project4_Var1_sideA_lib.ConsoleUI;
using Nikolaev_RA_Project4_Var1_sideA_lib.LogConfig;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.LogParser
{
    /// <summary>
    /// Класс для записи логов в файл или консоль.
    /// Считывает конфигурацию и форматирует логи согласно заданным настройкам.
    /// </summary>
    public class WriteLog : ConsoleUiBase
    {
        /// <summary>
        /// Цвет для сообщений об ошибках.
        /// </summary>
        private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;
        /// <summary>
        /// Цвет для сообщений об успешном выполнении операций.
        /// </summary>
        private static readonly ConsoleColor SuccessColor = ConsoleColor.Green;
        
        /// <summary>
        /// Записывает логи либо в файл, либо в консоль, либо выполняет оба действия.
        /// </summary>
        /// <param name="logs">Список логов для записи.</param>
        /// <param name="isFile">Если <c>true</c>, данные будут записаны в файл; если <c>false</c> – выведены в консоль.</param>
        public void Write(List<Log>? logs, bool isFile)
        {
            if (logs is null)
            {
                PrintErrorBox("Данные некорректны или их нет", ErrorColor);
                return;
            }
            
            // Получаем форматированный вывод логов в виде строки
            string? output = ParseLog(isFile, logs);
            if (output == null)
            {
                PrintErrorBox("Данные некорректны или их нет", ErrorColor);
                return;
            }
            
            // Если режим файла активен, сначала выполняется запись в файл
            if (isFile) 
                WriteInFile(output);
            // Затем всегда выполняется вывод в консоль
            WriteInConsole(output);
        }

        /// <summary>
        /// Записывает отформатированные логи в файл по указанному пользователем пути.
        /// </summary>
        /// <param name="outputData">Строка с отформатированными логами.</param>
        private void WriteInFile(string? outputData)
        {
            Console.Write("Введите полный путь до файла: ");
            string? outputPath = Console.ReadLine();

            if (string.IsNullOrEmpty(outputPath))
            {
                PrintErrorBox("Указан некорректный путь до файла", ErrorColor);
                return;
            }
            
            try
            {
                // Открываем или создаём файл для записи с кодировкой UTF-8
                using (FileStream fs = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        // Записываем отформатированные данные в файл
                        sw.Write(outputData);
                    }
                }
                PrintErrorBox("Запись в файл выполнена успешно.", SuccessColor);
            }
            catch (DirectoryNotFoundException)
            {
                PrintErrorBox("Не удается найти часть файла или каталога. Попробуйте еще раз", ErrorColor);
            }
            catch (IOException)
            {
                PrintErrorBox("Проблемы с сохранением файла. Попробуйте еще раз", ErrorColor);
            }
            catch (UnauthorizedAccessException)
            {
                PrintErrorBox("Проблемы с записью данных в файл. Попробуйте еще раз", ErrorColor);
            }
            catch (SecurityException)
            {
                PrintErrorBox("Проблемы с записью данных в файл. Попробуйте еще раз", ErrorColor);
            }
            catch (Exception ex)
            {
                PrintErrorBox($"Произошла ошибка: {ex.Message}. Попробуйте еще раз", ErrorColor);
            }
        }

        /// <summary>
        /// Выводит отформатированные логи в консоль.
        /// </summary>
        /// <param name="outputData">Строка с отформатированными логами.</param>
        private void WriteInConsole(string? outputData)
        {
            Console.WriteLine(outputData);
        }

        /// <summary>
        /// Форматирует логи в строку согласно конфигурации.
        /// Если конфигурация некорректна, используется конфигурация по умолчанию.
        /// </summary>
        /// <param name="isFileOutput">Режим вывода: <c>true</c> для файла, <c>false</c> для консоли.</param>
        /// <param name="logs">Список логов для форматирования.</param>
        /// <returns>Отформатированная строка с логами или <c>null</c>, если конфигурация не получена.</returns>
        private string? ParseLog(bool isFileOutput, List<Log> logs)
        {
            // Чтение кастомного конфигурационного файла
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../FileAnalyzer_library/Configs/customConfig.json");
            string json = File.ReadAllText(path, Encoding.UTF8).Trim();

            ConfigEntry? config = null;
            try
            {
                // Попытка десериализовать кастомный конфиг
                config = JsonSerializer.Deserialize<ConfigEntry>(json);
            }
            catch (Exception ex)
            {
                PrintErrorBox($"Указан некорректный конфиг. \n Ошибка: {ex.Message}", ErrorColor);
            }
            
            // Если кастомный конфиг некорректен, пытаемся использовать конфиг по умолчанию
            if (config == null 
                || string.IsNullOrWhiteSpace(config.Separator) 
                || string.IsNullOrWhiteSpace(config.DateFormat) 
                || config.FieldsOrder.Length == 0)
            {
                PrintErrorBox("Указан некорректный конфиг. Задействован конфиг по умолчанию.", ErrorColor);
                
                string defaultConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../FileAnalyzer_library/Configs/defaultConfig.json");
                string defaultConfigJson = File.ReadAllText(defaultConfigPath, Encoding.UTF8).Trim();

                try
                {
                    config = JsonSerializer.Deserialize<ConfigEntry>(defaultConfigJson);
                }
                catch (Exception ex)
                {
                    PrintErrorBox($"Некорректный конфиг. \n Ошибка: {ex.Message}", ErrorColor);
                }

                if (config == null
                    || string.IsNullOrWhiteSpace(config.Separator)
                    || string.IsNullOrWhiteSpace(config.DateFormat)
                    || config.FieldsOrder.Length == 0)
                {
                    PrintErrorBox("Некорректный конфиг.", ErrorColor);
                    return null;
                }
            }
            
            // Выбираем форматирование для файла или консоли
            string outputData = isFileOutput ? ParseForFile(logs, config) : ParseForConsole(logs, config);
            return outputData;
        }

        /// <summary>
        /// Форматирует логи для вывода в файл.
        /// Использует конфигурацию для определения порядка полей и разделителя.
        /// </summary>
        /// <param name="logs">Список логов.</param>
        /// <param name="config">Конфигурация форматирования.</param>
        /// <returns>Отформатированная строка для записи в файл.</returns>
        private string ParseForFile(List<Log> logs, ConfigEntry config)
        {
            StringBuilder outputData = new StringBuilder();
            
            // Вычисляем индексы для доступа к элементам массива согласно конфигурации
            int indexLevel = Array.IndexOf(config.FieldsOrder, "Уровень");
            int indexDate = Array.IndexOf(config.FieldsOrder, "Дата");
            int indexMessage = Array.IndexOf(config.FieldsOrder, "Сообщение");
            
            foreach (var log in logs)
            {
                // Массив для хранения отдельных частей лог-записи
                string[] parts = new string[3];
                parts[indexLevel] = log.Level;
                parts[indexDate] = log.Date.ToString(config.DateFormat);
                parts[indexMessage] = log.Message;
    
                string line;
                // Если разделитель представляет собой специальные символы-скобки
                var sep = config.Separator.Trim();
                if (sep is "[]" or "{}" or "()")
                {
                    char openBracket = sep[0];
                    char closeBracket = sep[1];
                        
                    // Оборачиваем каждое значение (кроме последнего, если требуется)
                    for (int i = 0; i < parts.Length - 1; i++)
                    {
                        parts[i] = $"{openBracket}{parts[i]}{closeBracket}";
                    }
                        
                    line = string.Join(" ", parts); 
                }
                else
                {
                    // Обычное объединение частей строки с указанным разделителем
                    line = string.Join(sep, parts);
                }
    
                outputData.AppendLine(line);
            }
            
            return outputData.ToString();
        }

        /// <summary>
        /// Форматирует логи для вывода в консоль.
        /// Добавляет разделительные линии и оборачивает отдельные части логов согласно конфигурации.
        /// </summary>
        /// <param name="logs">Список логов.</param>
        /// <param name="config">Конфигурация форматирования.</param>
        /// <returns>Отформатированная строка для вывода в консоль.</returns>
        private string ParseForConsole(List<Log> logs, ConfigEntry config)
        {
            StringBuilder outputData = new StringBuilder();
            outputData.AppendLine(new string('─', 50));
            var sep = config.Separator.Trim();
            
            foreach (var log in logs)
            {
                string line;
                if (sep is "[]" or "{}" or "()")
                {
                    char openBracket = sep[0];
                    char closeBracket = sep[1];
                        
                    // Форматируем строку для консольного вывода, оборачивая значения в скобки
                    line = $"{openBracket}{log.FileName}{closeBracket} {openBracket}{log.Date}{closeBracket} {openBracket}{log.Level}{closeBracket} \n {log.Message}";
                }
                else
                {
                    line = $"{log.FileName} {sep} {log.Date} {sep} {log.Level} {sep} \n {log.Message}";
                }
                    
                outputData.AppendLine(line);
                outputData.AppendLine(new string('─', 50));
            }
            
            return outputData.ToString();
        }
    }
}
