using Nikolaev_RA_Project4_Var1_sideA_lib.MenuClasses;

namespace Nikolaev_RA_Project4_Var1_sideA_lib
{
    /// <summary>
    /// Класс, реализующий основной процесс выполнения программы.
    /// Запускает и управляет циклом работы консольного меню.
    /// </summary>
    public class ProgramProcess
    {
        /// <summary>
        /// Запускает приложение и выполняет цикл меню до выхода пользователя.
        /// </summary>
        public void Run()
        {
            // Создаем объект меню для взаимодействия с пользователем
            Menu menu = new Menu();
            // Цикл работы приложения, выполняющий действия, выбранные пользователем
            do
            {
                // Отображаем меню и выполняем выбранное действие
                menu.MenuAction();
            }
            // Продолжаем работу, пока пользователь не выберет команду выхода (индекс 6) или не отменит выбор (индекс -1)
            while (menu.SelectedCommandIndex != 6 && menu.SelectedCommandIndex != -1);
        }
    }
}