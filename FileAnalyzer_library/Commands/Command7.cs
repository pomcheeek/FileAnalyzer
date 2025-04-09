using Nikolaev_RA_Project4_Var1_sideA_lib.CLog;
using Nikolaev_RA_Project4_Var1_sideA_lib.CommandInterfaces;
using System.IO;

namespace Nikolaev_RA_Project4_Var1_sideA_lib.MenuClasses.Commands;

/// <summary>
/// Системная команда завершения работы программы.
/// При выполнении выводит сообщение о завершении работы и очищает файл кастомной конфигурации.
/// </summary>
public class Command7 : ISystemCommand
{
    /// <summary>
    /// Выполняет команду завершения работы программы.
    /// Выводит сообщение пользователю и очищает содержимое файла с кастомной конфигурацией.
    /// </summary>
    public void CommandProcess()
    {
        // Вывод сообщения о завершении работы программы в консоль.
        Console.WriteLine("Выполнение программы завершено");

        // Определение пути к файлу кастомной конфигурации.
        string filePath = "../../../../FileAnalyzer_library/Configs/customConfig.json";

        // Очищаем содержимое файла, записывая в него пустую строку.
        File.WriteAllText(filePath, string.Empty);
    }
}