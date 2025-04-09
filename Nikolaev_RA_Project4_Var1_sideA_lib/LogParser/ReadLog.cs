using System.Globalization;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;
using Nikolaev_RA_Project4_Var1_sideA_lib.ConsoleUI;
using Nikolaev_RA_Project4_Var1_sideA_lib.LogConfig;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.LogParser
{
    /// <summary>
    /// Класс для чтения и парсинга логов из файлов.
    /// Считывает содержимое файлов, разбивает их на строки и извлекает лог-записи согласно конфигурации.
    /// </summary>
    public class ReadLog : ConsoleUiBase
    {
        /// <summary>
        /// Цвет для сообщений об ошибках.
        /// </summary>
        private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;
        /// <summary>
        /// Цвет для успешных сообщений.
        /// </summary>
        private static readonly ConsoleColor SuccessColor = ConsoleColor.Green;
        
        /// <summary>
        /// Читает логи из файлов, парсит их и возвращает список лог-записей.
        /// </summary>
        /// <returns>Список объектов <see cref="Log"/> или <c>null</c>, если не удалось прочитать логи.</returns>
        public List<Log>? Read()
        {
            // Считываем строки логов из файлов
            List<string>? inputLogs = ReadLogsFromFile();
            if (inputLogs == null || inputLogs.Count == 0)
            {
                return null;
            }
            
            // Парсим строки в объекты Log
            return ParseLog(inputLogs);
        }

        /// <summary>
        /// Считывает содержимое файлов, пути к которым вводятся пользователем.
        /// </summary>
        /// <returns>Список строк, каждая из которых содержит данные из одного файла, или <c>null</c> при ошибке.</returns>
        private List<string>? ReadLogsFromFile()
        {
            Console.Write("Введите полные пути до файлов через запятую без пробелов (например, File1.txt,File2.txt): ");
            string? filePathes = Console.ReadLine();

            if (string.IsNullOrEmpty(filePathes))
            {
                PrintErrorBox("Указан некорретный путь до файла", ErrorColor);
                return null;
            }
            
            // Разбиваем введенную строку по запятым для получения списка путей
            filePathes = filePathes.Trim();
            List<string> allPaths = filePathes.Split(",").ToList();
            List<string> allLogs = new List<string>();
            
            // Обрабатываем каждый путь
            foreach (var filePath in allPaths)
            {
                try
                {
                    string logs;
                    // Открываем файл для чтения в режиме FileAccess.Read
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        // Создаем StreamReader с указанием кодировки UTF-8
                        using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                        {
                            // Вывод сообщения об успешном чтении файла
                            PrintErrorBox("Чтение файла прошло успешно", SuccessColor);

                            // Сохраняем текущий поток ввода консоли
                            TextReader originalIn = Console.In;
                            // Устанавливаем поток ввода из файла, чтобы использовать Console.In.ReadToEnd()
                            Console.SetIn(sr);

                            // Читаем все содержимое файла
                            logs = Console.In.ReadToEnd();

                            // Восстанавливаем исходный поток ввода
                            Console.SetIn(originalIn);
                        }
                    }

                    // Добавляем путь к файлу в начало данных, чтобы сохранить имя файла
                    logs = filePath + "\n" + logs;
                    allLogs.Add(logs);
                }
                // Обработка исключений для различных ошибок при чтении файла
                catch (DirectoryNotFoundException)
                {
                    PrintErrorBox("Проблемы с открытием файла. Попробуйте еще раз", ErrorColor);
                    return null;
                }
                catch (FileNotFoundException)
                {
                    PrintErrorBox("Проблемы с открытием файла. Попробуйте еще раз", ErrorColor);
                    return null;
                }
                catch (IOException)
                {
                    PrintErrorBox("Проблемы с чтением данных из файла. Попробуйте еще раз", ErrorColor);
                    return null;
                }
                catch (UnauthorizedAccessException)
                {
                    PrintErrorBox("Проблемы с чтением данных из файла. Попробуйте еще раз", ErrorColor);
                    return null;
                }
                catch (SecurityException)
                {
                    PrintErrorBox("Проблемы с чтением данных из файла. Попробуйте еще раз", ErrorColor);
                    return null;
                }
                catch (Exception ex)
                {
                    PrintErrorBox($"Произошла ошибка: {ex.Message}", ErrorColor);
                    return null;
                }
            }
            return allLogs;
        }

        /// <summary>
        /// Парсит данные из строк, полученных из файлов, в список объектов <see cref="Log"/>.
        /// </summary>
        /// <param name="stringFileData">Список строк, каждая из которых содержит данные одного файла.</param>
        /// <returns>Список логов или <c>null</c> при ошибке в конфигурации.</returns>
        private List<Log>? ParseLog(List<string> stringFileData)
        {
            List<Log> logs = new List<Log>();
            // Обрабатываем каждый набор данных из файла
            foreach (var stringData in stringFileData)
            {
                // Разбиваем содержимое файла на строки
                string[] lines = stringData.Split(Environment.NewLine);
                // Первой строкой является имя файла
                string currentFileName = lines.First();
                // Остальные строки содержат лог-записи
                lines = lines.Skip(1).ToArray();            
            
                // Получаем конфигурацию для парсинга логов
                ConfigGetter getter = new ConfigGetter();
                ConfigEntry? config = getter.GetConfig();

                // Если конфигурация некорректна, выводим ошибку и завершаем парсинг
                if (config == null
                    || string.IsNullOrWhiteSpace(config.Separator)
                    || string.IsNullOrWhiteSpace(config.DateFormat)
                    || config.FieldsOrder.Length == 0)
                {
                    PrintErrorBox("Некорректный конфиг.", ErrorColor);
                    return null;
                }

                // Обрабатываем каждую строку с логом
                foreach (var line in lines)
                {
                    // Пропускаем пустые строки
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    // Извлекаем значения логов, используя разделитель и правила из конфигурации
                    string[] parts = GetWrappedValues(line, config);
                    // Преобразуем массив строк в объект Log
                    Log? log = SaveLogs(parts, config, currentFileName);

                    if (log != null)
                        logs.Add(log);
                }
            }
            
            return logs;
        }
        
        /// <summary>
        /// Извлекает значения из строки лога, используя разделитель, заданный в конфигурации.
        /// Если разделитель является скобками (например, [], {}, ()),
        /// используется регулярное выражение для извлечения текста между скобками.
        /// </summary>
        /// <param name="stringData">Строка с данными лога.</param>
        /// <param name="config">Конфигурация, содержащая параметры разделителя.</param>
        /// <returns>Массив строк с извлеченными значениями.</returns>
        private static string[] GetWrappedValues(string stringData, ConfigEntry config)
        {
            string[] result;
            var sep = config.Separator;
            // Если разделитель представляет собой пары скобок
            if (sep.Trim() == "[]" || sep.Trim() == "{}" || sep.Trim() == "()")
            {
                sep = sep.Trim();
                // Извлекаем символы открывающей и закрывающей скобок
                string openBracket = sep[0].ToString();
                string closeBracket = sep[1].ToString();
    
                // Экранируем символы для использования в регулярном выражении
                string openEscaped = Regex.Escape(openBracket);
                string closeEscaped = Regex.Escape(closeBracket);
    
                // Формируем шаблон для поиска текста между скобками
                string pattern = $@"{openEscaped}(.*?){closeEscaped}";
    
                var matches = Regex.Matches(stringData, pattern);
    
                // Создаем массив для хранения извлечённых значений + последний элемент для сообщения
                string[] parts = new string[matches.Count + 1];
    
                for (int i = 0; i < matches.Count; i++)
                {
                    parts[i] = matches[i].Groups[1].Value.Trim();
                }
    
                // Остаток строки после последней закрывающей скобки считается сообщением
                int lastCloseIndex = stringData.LastIndexOf(closeBracket, StringComparison.Ordinal);
                if (lastCloseIndex != -1 && lastCloseIndex < stringData.Length - 1)
                {
                    parts[matches.Count] = stringData.Substring(lastCloseIndex + 1).Trim();
                }
                else
                {
                    parts[matches.Count] = string.Empty;
                }
    
                return parts;
            }
            else
            {
                // Если разделитель не является скобками, просто делим строку по разделителю
                result = stringData.Split(sep);
            }
            
            return result;
        }

        /// <summary>
        /// Преобразует массив строк, полученных при парсинге, в объект <see cref="Log"/>.
        /// Использует конфигурацию для определения порядка полей.
        /// </summary>
        /// <param name="parts">Массив строк, содержащий данные лог-записи.</param>
        /// <param name="config">Конфигурация, определяющая порядок полей и формат даты.</param>
        /// <param name="fileName">Имя файла, из которого был прочитан лог.</param>
        /// <returns>Объект <see cref="Log"/>, если парсинг успешен, иначе <c>null</c>.</returns>
        private Log? SaveLogs(string[] parts, ConfigEntry config, string fileName)
        {
            Log log = new Log();
            var order = config.FieldsOrder;
            try
            {
                // Извлекаем и парсим дату согласно формату из конфигурации
                log.Date = DateTime.ParseExact(parts[Array.IndexOf(order, "Дата")], config.DateFormat,
                    CultureInfo.InvariantCulture);
                // Извлекаем уровень логирования
                log.Level = parts[Array.IndexOf(order, "Уровень")];
                // Извлекаем сообщение лога
                log.Message = parts[Array.IndexOf(order, "Сообщение")];
                // Сохраняем имя файла, из которого был прочитан лог
                log.FileName  = fileName;
                return log;
            }
            catch (Exception e)
            {
                // Если возникает ошибка при парсинге лог-записи, выводим сообщение об ошибке и возвращаем null
                PrintErrorBox($"Некорректный лог \n Ошибка: {e.Message}", ErrorColor);
                return null;
            }
        }
    }
}
