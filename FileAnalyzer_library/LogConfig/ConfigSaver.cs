using System.Text.Json;
using Nikolaev_RA_Project4_Var1_sideA_lib.ConsoleUI;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.LogConfig;

/// <summary>
/// Класс для сохранения текущей конфигурации в файл.
/// Использует данные из словаря с настройками для создания объекта <see cref="ConfigEntry"/>
/// и сериализует его в JSON формат.
/// </summary>
public class ConfigSaver : ConsoleUiBase
{
    /// <summary>
    /// Сохраняет текущую конфигурацию, полученную в виде словаря, в файл.
    /// </summary>
    /// <param name="currentConfiguration">
    /// Словарь, содержащий текущие настройки конфигурации.
    /// Ключи: "Порядок полей", "Разделитель", "Формат даты".
    /// Значения: соответствующие настройки.
    /// </param>
    public void SaveCurrentConfiguration(Dictionary<string, string> currentConfiguration)
    {
        // Создаем новый объект конфигурации.
        ConfigEntry config = new ConfigEntry();

        try
        {
            // Преобразуем строку с порядком полей в массив строк,
            // используя разделитель ", " для разбиения.
            config.FieldsOrder = currentConfiguration["Порядок полей"].Split(", ");
            // Получаем разделитель из словаря.
            config.Separator = currentConfiguration["Разделитель"];
            // Получаем формат даты из словаря.
            config.DateFormat = currentConfiguration["Формат даты"];
        }
        catch (Exception e)
        {
            // Если происходит ошибка при создании конфигурации, выводим сообщение об ошибке.
            Console.WriteLine();
            PrintErrorBox($"Ошибка в создании конфига. \n Ошибка: {e.Message}", ConsoleColor.Red);
        }
        
        // Формируем путь к файлу, в который будет сохранена кастомная конфигурация.
        string customConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../FileAnalyzer_library/Configs/customConfig.json");
        
        // Опции для форматированной сериализации (читаемый JSON с отступами).
        var options = new JsonSerializerOptions { WriteIndented = true };
        // Сериализуем объект конфигурации в JSON строку.
        string customConfigJson = JsonSerializer.Serialize(config, options);
        // Записываем JSON строку в файл.
        File.WriteAllText(customConfigPath, customConfigJson);
        
        // Выводим сообщение об успешной настройке формата конфигурации.
        PrintErrorBox("Формат успешно настроен!", ConsoleColor.Green);
    }
}
