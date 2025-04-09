using System.Text;
using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.ConsoleUI
{
    /// <summary>
    /// Абстрактный базовый класс для реализации консольного пользовательского интерфейса.
    /// Содержит методы для отображения меню, заголовков, подвалов, информационных и ошибокых сообщений.
    /// </summary>
    public abstract class ConsoleUiBase
    {
        // Вспомогательные методы оформления консольного интерфейса

        /// <summary>
        /// Отображает меню с вариантами команд, выделяя выбранный пункт и выводя дополнительную информацию.
        /// </summary>
        /// <param name="commandsNames">Массив строк с названиями команд.</param>
        /// <param name="selectedCommandIndex">Индекс выбранной команды для выделения.</param>
        /// <param name="extraInformation">Дополнительная информация, которая выводится под меню.</param>
        private void DisplayMenu(string[] commandsNames, int selectedCommandIndex, string? extraInformation)
        {
            // Устанавливаем кодировку консоли для поддержки символов UTF-8.
            Console.OutputEncoding = Encoding.UTF8;
            const int padding = 2; // Отступ для форматирования вывода

            // Вычисляем максимальную ширину строки с учетом отступа
            int maxWidth = commandsNames.Max(c => c.Length) + padding;
            // Формируем горизонтальную линию для оформления
            string horizontalLine = new string('─', maxWidth);

            // Вывод заголовка меню
            Console.WriteLine("\n══════ Выберите команду ══════\n");

            // Перебор всех вариантов команд для их отображения
            for (int i = 0; i < commandsNames.Length; i++)
            {
                // Если текущий элемент является выбранным, применяем выделенный стиль
                if (i == selectedCommandIndex)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("┌─");
                    Console.Write(horizontalLine);
                    Console.WriteLine("─┐");

                    Console.Write("│ ");
                    // Выравнивание строки команды по заданной ширине
                    Console.Write(commandsNames[i].PadRight(maxWidth));
                    Console.WriteLine(" │");

                    Console.Write("└─");
                    Console.Write(horizontalLine);
                    Console.WriteLine("─┘");
                }
                else // Обычное отображение невыбранных пунктов
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"├ {commandsNames[i].PadRight(maxWidth)} ┤");
                }
            }

            // Вывод дополнительной информации (если таковая имеется)
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(extraInformation);

            // Вывод подсказок для навигации по меню
            Console.WriteLine("\n[ ↑/↓ ] - Навигация   [ Enter ] - Выбор  [ Escape ] - Назад");
            Console.ResetColor();
        }

        /// <summary>
        /// Запускает меню для выбора команды пользователем.
        /// Пользователь может навигировать с помощью стрелок, выбрать пункт клавишей Enter или выйти с Escape.
        /// </summary>
        /// <param name="commandsNames">Массив строк с названиями команд.</param>
        /// <param name="extraInformation">Дополнительная информация для вывода в меню.</param>
        /// <returns>Индекс выбранной команды, или -1, если выбор отменен.</returns>
        protected int Run(string[] commandsNames, string? extraInformation = null)
        {
            int selectedCommandIndex = 0;
            ConsoleKey key;
            // Бесконечный цикл для обработки ввода с клавиатуры
            while (true)
            {
                // Очищаем консоль перед каждым отображением меню
                Console.Clear();
                // Отображаем меню с текущим выбором
                DisplayMenu(commandsNames, selectedCommandIndex, extraInformation);

                var keyInfo = Console.ReadKey(true);
                key = keyInfo.Key;

                // Обработка нажатия стрелки вверх: перемещаем выбор вверх
                if (key == ConsoleKey.UpArrow)
                {
                    selectedCommandIndex = (selectedCommandIndex == 0) ? commandsNames.Length - 1 : selectedCommandIndex - 1;
                }
                // Обработка нажатия стрелки вниз: перемещаем выбор вниз
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedCommandIndex = (selectedCommandIndex == commandsNames.Length - 1) ? 0 : selectedCommandIndex + 1;
                }
                // Если нажата клавиша Enter, возвращаем индекс выбранной команды
                else if (key == ConsoleKey.Enter)
                {
                    return selectedCommandIndex;
                }
                // Если нажата клавиша Escape, возвращаем -1 для обозначения отмены выбора
                else if (key == ConsoleKey.Escape)
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Выводит заголовок с заданным текстом и цветом.
        /// </summary>
        /// <param name="title">Текст заголовка.</param>
        /// <param name="color">Цвет заголовка.</param>
        protected void PrintHeader(string title, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            // Преобразуем заголовок к верхнему регистру для акцента
            Console.WriteLine($"\n══════════ {title.ToUpper()} ══════════\n");
            Console.ResetColor();
        }

        /// <summary>
        /// Выводит подвал с заданным цветом.
        /// </summary>
        /// <param name="color">Цвет подвала.</param>
        protected void PrintFooter(ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine("\n════════════════════════════════════\n");
            Console.ResetColor();
        }

        /// <summary>
        /// Выводит сообщение об ошибке в виде оформленного блока.
        /// </summary>
        /// <param name="message">Текст сообщения об ошибке.</param>
        /// <param name="color">Цвет для сообщения об ошибке.</param>
        protected void PrintErrorBox(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            // Оформление блока сообщения с рамкой
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine($"║ {"! " + message.PadRight(28)}   ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.ResetColor();
        }

        /// <summary>
        /// Выводит информационный блок с заданным содержимым и цветом.
        /// </summary>
        /// <param name="content">Содержимое информационного блока.</param>
        /// <param name="color">Цвет текста информационного блока.</param>
        protected void PrintInfoBox(string content, ConsoleColor color)
        {
            const int boxWidth = 34;
            // Выравнивание текста с учетом ширины блока
            string paddedContent = content.PadRight(boxWidth - 4);
            Console.ForegroundColor = color;
            Console.WriteLine($"╭─{new string('─', boxWidth - 4)}─╮");
            Console.WriteLine($"│ {paddedContent} │");
            Console.WriteLine($"╰─{new string('─', boxWidth - 4)}─╯");
            Console.ResetColor();
        }

        /// <summary>
        /// Выводит статистику по уровням логирования в виде оформленного блока.
        /// </summary>
        /// <param name="levels">Словарь, содержащий уровни логирования и количество записей для каждого.</param>
        /// <param name="total">Общее количество логов для вычисления процентного соотношения.</param>
        /// <param name="color">Цвет для вывода статистики.</param>
        protected void PrintLevelsStats(Dictionary<string, int> levels, int total, ConsoleColor color)
        {
            const int boxWidth = 40;
            const int contentWidth = boxWidth - 4;
            string horizontalLine = new string('─', boxWidth - 2);
            Console.ForegroundColor = color;
            Console.WriteLine($"╭{horizontalLine}╮");

            // Заголовок блока статистики
            string header = "Количество по группам";
            int headerPadding = (contentWidth - header.Length) / 2;
            Console.WriteLine($"│ {header.PadLeft(headerPadding + header.Length),-contentWidth} │");
            Console.WriteLine($"├{new string('─', boxWidth - 2)}┤");

            // Вывод статистических данных для каждого уровня
            foreach (var level in levels)
            {
                // Если имя уровня длинное, обрезаем его до 12 символов
                string name = level.Key.Length > 12 ? level.Key.Substring(0, 12) : level.Key.PadRight(12);
                string count = level.Value.ToString().PadLeft(5);
                int percent = (level.Value * 100) / total;
                // Формируем строку с информацией о уровне, количестве и процентном соотношении
                string line = $"▸ {name} : {count} ({percent.ToString().PadLeft(3)}%)";

                // Корректируем длину строки для соответствия ширине блока
                if (line.Length > contentWidth)
                    line = line.Substring(0, contentWidth);
                else
                    line = line.PadRight(contentWidth);
                Console.WriteLine($"│ {line} │");
            }
            Console.WriteLine($"╰{horizontalLine}╯");
            Console.ResetColor();
        }

        /// <summary>
        /// Выводит блок с информацией о временных метках: самой ранней и самой поздней.
        /// </summary>
        /// <param name="earliest">Лог с самой ранней датой.</param>
        /// <param name="latest">Лог с самой поздней датой.</param>
        /// <param name="color">Цвет для оформления блока временных меток.</param>
        protected void PrintTimestamps(Log? earliest, Log? latest, ConsoleColor color)
        {
            // Форматируем временные метки или выводим "N/A" если значение отсутствует
            string earliestStr = earliest?.Date.ToString("yyyy-MM-dd HH:mm") ?? "N/A";
            string latestStr = latest?.Date.ToString("yyyy-MM-dd HH:mm") ?? "N/A";
            string paddedEarliest = earliestStr.PadRight(24);
            string paddedLatest = latestStr.PadRight(23);
            Console.ForegroundColor = color;
            Console.WriteLine("╭───────── Временные метки ────────╮");
            Console.WriteLine($"│ Ранняя: {paddedEarliest} │");
            Console.WriteLine($"│ Поздняя: {paddedLatest} │");
            Console.WriteLine("╰──────────────────────────────────╯");
            Console.ResetColor();
        }

        /// <summary>
        /// Выводит блок с информацией о средней длине сообщения.
        /// </summary>
        /// <param name="avg">Средняя длина сообщения.</param>
        /// <param name="color">Цвет для оформления блока.</param>
        protected void PrintAverageLength(double avg, ConsoleColor color)
        {
            // Форматируем значение средней длины до двух знаков после запятой
            string avgStr = avg.ToString("F2").PadLeft(23);
            Console.ForegroundColor = color;
            Console.WriteLine("╭───── Средняя длина сообщения ────╮");
            Console.WriteLine($"│ {avgStr} символов │");
            Console.WriteLine("╰──────────────────────────────────╯");
            Console.ResetColor();
        }
    }
}
