using System.Text;
using System.Text.Json;
using Nikolaev_RA_Project4_Var1_sideA_lib.CommandInterfaces;
using Nikolaev_RA_Project4_Var1_sideA_lib.ConsoleUI;
using Nikolaev_RA_Project4_Var1_sideA_lib.LogConfig;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.Commands;

/// <summary>
/// Команда для настройки конфигурации приложения.
/// Позволяет пользователю либо загрузить конфигурационный файл, либо настроить параметры вручную через консоль.
/// </summary>
public class Command2 : ConsoleUiBase, ISystemCommand
{
    /// <summary>
    /// Цвет, используемый для вывода сообщений об ошибках.
    /// </summary>
    private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;
    
    /// <summary>
    /// Выбранный тип команды (загрузка файла или ручная настройка).
    /// </summary>
    private int _selectedCommand;
    
    /// <summary>
    /// Выбранная категория настройки (например, порядок полей, разделитель, формат даты).
    /// </summary>
    private int _selectedCategory;
    
    /// <summary>
    /// Выбранный вариант внутри выбранной категории.
    /// </summary>
    private int _selectedVariant;
    
    /// <summary>
    /// Массив строк, представляющий категории настроек.
    /// Последний элемент отвечает за сохранение текущей конфигурации.
    /// </summary>
    private static readonly string[] Categories = ["Порядок полей", "Разделитель", "Формат даты", "Сохранить текущую конфигурацию"];

    /// <summary>
    /// Возможные варианты порядка полей.
    /// </summary>
    private static readonly string[] FieldsOrderVariants =
    [
        "Дата, Уровень, Сообщение",
        "Уровень, Дата, Сообщение",
        "Сообщение, Дата, Уровень"
    ];

    /// <summary>
    /// Возможные варианты разделителей между полями.
    /// </summary>
    private static readonly string[] SeparatorVariants =
    [
        " | ", " [] ", " {} ", " () ", " / "
    ];

    /// <summary>
    /// Возможные варианты форматов даты.
    /// </summary>
    private static readonly string[] DateFormatVariants =
    [
        "yyyy-MM-dd HH:mm:ss",
        "dd.MM.yyyy HH:mm",
        "MM/dd/yyyy HH:mm:ss"
    ];

    /// <summary>
    /// Текущая конфигурация, представленная в виде словаря.
    /// Ключ - название параметра, значение - выбранный вариант настройки.
    /// </summary>
    private readonly Dictionary<string, string> _currentConfiguration = new Dictionary<string, string>()
    {
        {"Порядок полей", "Дата, Уровень, Сообщение"},
        {"Разделитель", " [] "},
        {"Формат даты", "yyyy-MM-dd HH:mm:ss"}
    };
    
    /// <summary>
    /// Словарь, сопоставляющий категории с их возможными вариантами.
    /// Используется при ручной настройке конфигурации.
    /// </summary>
    private static readonly Dictionary<string, string[]?> Variants = new Dictionary<string, string[]?>
    {
        { "Порядок полей", FieldsOrderVariants },
        { "Разделитель", SeparatorVariants },
        { "Формат даты", DateFormatVariants }
    };
    
    /// <summary>
    /// Массив строк с названиями команд, доступных пользователю:
    /// 1. Загрузка конфигурационного файла
    /// 2. Ручная настройка конфигурации через консоль
    /// </summary>
    private static readonly string[] CommandsNames = new []
    {
        "1. Загрузить конфигурационный файл",
        "2. Найстроить вручную"
    };

    /// <summary>
    /// Сбрасывает выбранные параметры команд и настроек в исходное состояние.
    /// </summary>
    private void Reset()
    {
        // Сброс индексов выбора команд и настроек
        _selectedCommand = 0;
        _selectedCategory = 0;
        _selectedVariant = 0;
    }
    
    /// <summary>
    /// Основной метод выполнения команды.
    /// Сначала предлагает выбор способа настройки конфигурации, затем выполняет соответствующий метод.
    /// </summary>
    public void CommandProcess()
    {
        // Получение выбора пользователя из меню команд
        _selectedCommand = Run(CommandsNames);

        // Обработка выбранной команды
        switch (_selectedCommand)
        {
            case -1: // Если пользователь отменил выбор
                return;
            case 0:
                // Выбран вариант загрузки конфигурации из файла
                ReadConfigFromFile();
                break;
            case 1:
                // Выбран вариант ручной настройки через консоль
                ReadConfigFromConsole();
                break;
        }
        // Сброс настроек после выполнения команды
        Reset();
    }

    /// <summary>
    /// Загружает конфигурацию из файла.
    /// Запрашивает у пользователя путь к файлу, читает и десериализует JSON, сохраняет конфигурацию в файл.
    /// В случае ошибок выводит сообщение об ошибке.
    /// </summary>
    private void ReadConfigFromFile()
    {
        // Запрос пути к файлу конфигурации
        Console.WriteLine("Введите полный путь до файла: ");
        string? configPath = Console.ReadLine();

        // Если пользователь не ввёл путь, прекращаем выполнение метода
        if (string.IsNullOrEmpty(configPath))
        {
            return;
        }
        
        // Формирование полного пути к файлу конфигурации
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configPath);
        // Чтение содержимого файла в формате UTF8
        string json = File.ReadAllText(path, Encoding.UTF8).Trim();
        ConfigEntry? config = null;
        try
        {
            // Попытка десериализации JSON в объект ConfigEntry
            config = JsonSerializer.Deserialize<ConfigEntry>(json);
        }
        catch (Exception e)
        {
            // Вывод сообщения об ошибке в случае неудачи десериализации
            PrintErrorBox($"Указан некорректный конфиг. \n Ошибка: {e.Message}", ErrorColor);
        }
        
        // Проверка валидности полученной конфигурации
        if (config == null 
            || string.IsNullOrWhiteSpace(config.Separator) 
            || string.IsNullOrWhiteSpace(config.DateFormat) 
            || config.FieldsOrder.Length == 0)
        {
            // Вывод ошибки и прекращение выполнения метода, если конфигурация некорректна
            PrintErrorBox("Указан некорректный конфиг.", ErrorColor);
            return;
        }
        
        // Формирование пути для сохранения кастомной конфигурации
        string customConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Nikolaev_RA_Project4_Var1_sideA_lib/Configs/customConfig.json");
        
        // Опции для сериализации с форматированием отступов
        var options = new JsonSerializerOptions { WriteIndented = true };

        // Сериализация объекта конфигурации в форматированный JSON
        string customConfigJson = JsonSerializer.Serialize(config, options);
        
        // Запись нового JSON в файл по указанному пути
        File.WriteAllText(customConfigPath, customConfigJson);
    }

    /// <summary>
    /// Обеспечивает ручную настройку конфигурации через консоль.
    /// Позволяет пользователю выбрать категорию настройки и соответствующий вариант, обновляя текущую конфигурацию.
    /// </summary>
    private void ReadConfigFromConsole()
    {
        // Цикл продолжается, пока пользователь не выберет сохранение конфигурации
        while (_selectedCategory != 3)
        {
            // Отображение меню категорий с текущей конфигурацией
            _selectedCategory = Run(Categories, PrintCurrentConfiguration());
            // Определение действия в зависимости от выбранной категории
            switch (_selectedCategory + 1)
            {
                case 0:
                    // Пользователь отменил выбор, выходим из метода
                    return;
                case 1:
                    // Выбран порядок полей - отображаем варианты порядка полей
                    _selectedVariant = Run(FieldsOrderVariants);
                    break;
                case 2:
                    // Выбран разделитель - отображаем варианты разделителей
                    _selectedVariant = Run(SeparatorVariants);
                    break;
                case 3:
                    // Выбран формат даты - отображаем варианты форматов даты
                    _selectedVariant = Run(DateFormatVariants);
                    break;
                case 4:
                    // Выбран пункт для сохранения текущей конфигурации
                    ConfigSaver saver = new ConfigSaver();
                    saver.SaveCurrentConfiguration(_currentConfiguration);
                    return;
            }

            // Если пользователь отменил выбор варианта, переходим к следующей итерации
            if (_selectedVariant == -1)
            {
                continue;
            }

            try
            {
                // Обновление текущей конфигурации выбранным вариантом из словаря вариантов
                _currentConfiguration[Categories[_selectedCategory]] =
                    Variants[Categories[_selectedCategory]]![_selectedVariant];
            }
            catch (Exception e)
            {
                // Обработка исключений при обновлении конфигурации и вывод сообщения об ошибке
                PrintErrorBox($"Ошибка настройки конфигурации. \n Ошибка: {e.Message}", ErrorColor);
            }
        }
    }
    
    /// <summary>
    /// Формирует строку, содержащую текущую конфигурацию.
    /// Используется для отображения настроек в консольном меню.
    /// </summary>
    /// <returns>Строка с перечнем параметров конфигурации и их значениями.</returns>
    private string PrintCurrentConfiguration()
    {
        // Используем StringBuilder для эффективной конкатенации строк
        StringBuilder sb = new StringBuilder();
        // Перебираем все элементы текущей конфигурации
        foreach (var pair in _currentConfiguration)
        {
            // Добавляем строку с названием параметра и его значением
            sb.AppendLine(pair.Key + ": " + pair.Value);
        }

        // Возвращаем сформированную строку
        return sb.ToString();
    }
}
