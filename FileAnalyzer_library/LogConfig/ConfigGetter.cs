using System.Text;
using System.Text.Json;
using Nikolaev_RA_Project4_Var1_sideA_lib.ConsoleUI;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.LogConfig;

/// <summary>
/// Класс для получения конфигурации из файлов JSON.
/// Если основной конфигурационный файл некорректен или отсутствует, используется конфигурация по умолчанию.
/// </summary>
public class ConfigGetter : ConsoleUiBase 
{
    /// <summary>
    /// Цвет для вывода сообщений об ошибках.
    /// </summary>
    private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;
    
    /// <summary>
    /// Считывает конфигурацию из файла и возвращает объект <see cref="ConfigEntry"/>.
    /// При ошибке чтения или десериализации выводится сообщение об ошибке и возвращается конфигурация по умолчанию.
    /// </summary>
    /// <returns>Объект <see cref="ConfigEntry"/> с параметрами конфигурации или <c>null</c>, если конфигурацию получить не удалось.</returns>
    public ConfigEntry? GetConfig()
    {
        // Формирование пути к кастомному конфигурационному файлу.
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../FileAnalyzer_library/Configs/customConfig.json");
        string json;
        try
        {
            // Чтение содержимого файла с конфигурацией в кодировке UTF8.
            json = File.ReadAllText(path, Encoding.UTF8).Trim();
        }
        catch (Exception e)
        {
            // Вывод сообщения об ошибке в случае проблем с чтением файла.
            PrintErrorBox($"Ошибка чтения конфигурацонного файла \n Ошибка: {e}", ErrorColor);
            return null;
        }

        ConfigEntry? config = null;
        try
        {
            // Попытка десериализации JSON в объект ConfigEntry.
            config = JsonSerializer.Deserialize<ConfigEntry>(json);
        }
        catch (Exception ex)
        {
            // Вывод ошибки при неудачной десериализации.
            PrintErrorBox($"Указан некорректный конфиг. \n Ошибка: {ex.Message}", ErrorColor);
        }
        
        // Проверка валидности полученной конфигурации.
        if (config == null 
            || string.IsNullOrWhiteSpace(config.Separator) 
            || string.IsNullOrWhiteSpace(config.DateFormat) 
            || config.FieldsOrder.Length == 0)
        {
            // Если конфигурация некорректна, выводим сообщение и используем конфигурацию по умолчанию.
            PrintErrorBox("Указан некорректный конфиг. Задействован конфиг по умолчанию. ", ErrorColor);
            
            // Формирование пути к файлу конфигурации по умолчанию.
            string defaultConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../FileAnalyzer_library/Configs/defaultConfig.json");
            string defaultConfigJson = File.ReadAllText(defaultConfigPath, Encoding.UTF8).Trim();

            try
            {
                // Попытка десериализации дефолтного конфигурационного файла.
                config = JsonSerializer.Deserialize<ConfigEntry>(defaultConfigJson);
            }
            catch (Exception ex)
            {
                // Вывод ошибки при неудачной десериализации дефолтного файла.
                PrintErrorBox($"Некорректный конфиг. \n Ошибка: {ex.Message}", ErrorColor);
                return null;
            }
        }
        
        // Возвращаем корректный объект конфигурации.
        return config;
    }
}
