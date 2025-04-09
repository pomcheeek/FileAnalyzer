namespace Nikolaev_RA_Project4_Var1_sideA_lib.CommandInterfaces;

/// <summary>
/// Интерфейс для системных команд, предоставляющий общий метод для обработки команды.
/// </summary>
public interface ISystemCommand
{
    /// <summary>
    /// Выполняет обработку команды.
    /// </summary>
    void CommandProcess();
}