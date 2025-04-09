using System.Text.Json.Serialization;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.LogConfig;

/// <summary>
/// Класс, представляющий конфигурацию логирования, считываемую из JSON файла.
/// Содержит порядок полей, разделитель и формат даты.
/// </summary>
public class ConfigEntry
{
    /// <summary>
    /// Порядок полей в логе.
    /// </summary>
    /// <remarks>
    /// Например: "Дата, Уровень, Сообщение".
    /// </remarks>
    [JsonPropertyName("fieldsOrder")]
    public string[] FieldsOrder { get; set; }

    /// <summary>
    /// Строка-разделитель, используемая для разделения полей лога.
    /// </summary>
    [JsonPropertyName("separator")]
    public string Separator { get; set; }

    /// <summary>
    /// Формат даты, используемый при выводе временных меток.
    /// </summary>
    [JsonPropertyName("dateFormat")] 
    public string DateFormat { get; set; }
}