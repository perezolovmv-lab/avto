using System.Windows;
using AutoServiceApp.Domain.Entities; // Добавлено для доступа к User

namespace AutoServiceApp
{
    public partial class App : Application
    {
        // --- Жизненный цикл приложения ---
        // Метод, вызываемый при запуске приложения.
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Убедитесь, что приложение не закрывается автоматически после закрытия первого окна.
            // Это позволяет управлять закрытием приложения вручную.
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Создаем и показываем окно входа (LoginWindow).
            var loginWindow = new LoginWindow();

            // ShowDialog() блокирует выполнение до закрытия окна и возвращает DialogResult.
            if (loginWindow.ShowDialog() == true && loginWindow.CurrentUser != null)
            {
                // Если авторизация успешна (DialogResult == true и CurrentUser установлен),
                // меняем режим завершения приложения на закрытие при закрытии главного окна.
                this.ShutdownMode = ShutdownMode.OnMainWindowClose;

                // Создаем главное окно (MainWindow), передавая текущего пользователя.
                var mainWindow = new MainWindow(loginWindow.CurrentUser);
                mainWindow.Show(); // Показываем главное окно.
            }
            else
            {
                // Если авторизация не удалась (DialogResult == false или CurrentUser не установлен),
                // закрываем приложение.
                this.Shutdown();
            }
        }
    }
}