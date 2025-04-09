using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;
using Nikolaev_RA_Project4_Var1_sideA_lib.CommandInterfaces;
using Nikolaev_RA_Project4_Var1_sideA_lib.Commands;
using Nikolaev_RA_Project4_Var1_sideA_lib.ConsoleUI;
using Nikolaev_RA_Project4_Var1_sideA_lib.MenuClasses.Commands;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.MenuClasses
{
    /// <summary>
    /// Класс Menu реализует консольное меню для взаимодействия с приложением.
    /// Предоставляет пользователю набор команд для ввода логов, настройки формата,
    /// фильтрации, сортировки, статистики, вывода данных и завершения работы приложения.
    /// </summary>
    public class Menu : ConsoleUiBase
    {
        /// <summary>
        /// Хранит список логов, с которыми работает приложение.
        /// </summary>
        private List<Log>? _logs = new List<Log>();

        /// <summary>
        /// Индекс выбранной команды в меню.
        /// </summary>
        public int SelectedCommandIndex;

        /// <summary>
        /// Массив названий команд, отображаемых в главном меню.
        /// </summary>
        private static readonly string[] CommandsNames = new[]
        {
            "1. Ввести данные из файла",
            "2. Настроить формата ввода/вывода",
            "3. Отфильтровать логи",
            "4. Отсортрровать логи",
            "5. Показать статистику",
            "6. Вывести данные в консоль/файл",
            "7. Выйти из консольного приложения"
        };

        /// <summary>
        /// Словарь команд для сохранения логов.
        /// Ключ – индекс команды, значение – объект, реализующий интерфейс <see cref="ISaveLogsCommand"/>.
        /// </summary>
        private static readonly Dictionary<int, ISaveLogsCommand> SaveLogsCommands = new()
        {
            { 0, new Command1() }
        };

        /// <summary>
        /// Словарь системных команд.
        /// Ключ – индекс команды, значение – объект, реализующий интерфейс <see cref="ISystemCommand"/>.
        /// </summary>
        private static readonly Dictionary<int, ISystemCommand> SystemCommands = new()
        {
            { 1, new Command2() },
            { 6, new Command7() }
        };

        /// <summary>
        /// Словарь команд для работы с логами.
        /// Ключ – индекс команды, значение – объект, реализующий интерфейс <see cref="IWorkWithLogsCommand"/>.
        /// </summary>
        private static readonly Dictionary<int, IWorkWithLogsCommand?> WorkWithLogsCommands = new()
        {
            { 2, new Command3() },
            { 3, new Command4() },
            { 4, new Command5() },
            { 5, new Command6() }
        };

        /// <summary>
        /// Запускает меню и выполняет выбранное действие.
        /// Отображает меню, обрабатывает выбор и запускает соответствующую команду.
        /// </summary>
        public void MenuAction()
        {
            // Отображение меню и получение выбранного индекса команды
            SelectedCommandIndex = Run(CommandsNames);
            Console.Clear();
            CommandProcess();
        }

        /// <summary>
        /// Обрабатывает выполнение команды, выбранной пользователем.
        /// В зависимости от выбранного индекса вызывается соответствующая команда для работы с логами,
        /// системная команда или команда сохранения логов.
        /// </summary>
        private void CommandProcess()
        {
            // Если выбранная команда принадлежит к командам работы с логами
            if (WorkWithLogsCommands.TryGetValue(SelectedCommandIndex, out IWorkWithLogsCommand? workWithLogsCommand))
            {
                // Если команда отсутствует, выводим сообщение и продолжаем
                if (workWithLogsCommand == null)
                {
                    ContinueMessage();
                    return;
                }
                // Выполняем команду обработки логов, передавая текущий список логов
                workWithLogsCommand.CommandProcess(_logs);
            }
            // Если выбранная команда является системной командой
            else if (SystemCommands.TryGetValue(SelectedCommandIndex, out ISystemCommand? systemCommand))
            {
                // Выполняем системную команду
                systemCommand.CommandProcess();
            }
            // Если выбранная команда предназначена для сохранения логов
            else if (SaveLogsCommands.TryGetValue(SelectedCommandIndex, out ISaveLogsCommand? saveLogsCommand))
            {
                // Получаем новые логи с помощью команды сохранения логов
                List<Log>? newLogs = saveLogsCommand.SaveLogs();
                if (newLogs == null)
                {
                    ContinueMessage();
                    return;
                }
                // Добавляем полученные логи к существующему списку
                _logs?.AddRange(newLogs);
            }
            // Если пользователь отменил выбор (SelectedCommandIndex равен -1)
            else if (SelectedCommandIndex == -1)
            {
                ContinueMessage();
                return;
            }
            
            // После выполнения команды выводим сообщение о продолжении работы
            ContinueMessage();
        }

        /// <summary>
        /// Выводит сообщение о необходимости нажать Enter для продолжения работы.
        /// Очищает консоль после нажатия Enter.
        /// </summary>
        private void ContinueMessage()
        {
            Console.WriteLine("Нажмите [ Enter ], чтобы продолжить");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
