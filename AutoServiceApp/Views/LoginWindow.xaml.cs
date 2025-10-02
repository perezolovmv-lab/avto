using AutoServiceApp.Domain.Entities;
using AutoServiceApp.Domain.Enums;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic; 

namespace AutoServiceApp
{
    public partial class LoginWindow : Window
    {
        
        public User CurrentUser { get; private set; }

        private readonly List<User> users = new List<User>
        {
            new User {
                Username = "admin",
                PasswordHash = "admin123",
                Role = UserRole.Administrator,
                FullName = "Администратор"
            },
            new User {
                Username = "mechanic",
                PasswordHash = "mech123",
                Role = UserRole.Mechanic,
                FullName = "Механик"
            }
        };

        private TextBlock errorText;

        public LoginWindow()
        {
            InitializeComponent(); 
            UsernameTextBox.Focus(); 
            errorText = (TextBlock)FindName("ErrorText");
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim(); 
            string password = PasswordBox.Password;       

            // Валидация ввода: проверка на пустые логин или пароль.
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                errorText.Text = "Введите логин и пароль"; 
                return;
            }

            // Поиск пользователя в списке по логину и паролю.
            var user = users.FirstOrDefault(u =>
                u.Username == username && u.PasswordHash == password);

            if (user != null)
            {
                CurrentUser = user; 
                this.DialogResult = true; 
                this.Close(); 
            }
            else
            {
                errorText.Text = "Неверный логин или пароль"; // Выводим сообщение об ошибке
                PasswordBox.Password = ""; // Очищаем поле пароля
                PasswordBox.Focus(); 
            }
        }

        // Обработчик нажатия клавиш в поле логина.
        private void UsernameTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                PasswordBox.Focus(); 
            }
        }

        // Обработчик нажатия клавиш в поле пароля.
        private void PasswordBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Login_Click(sender, e); 
            }
        }
    }
}