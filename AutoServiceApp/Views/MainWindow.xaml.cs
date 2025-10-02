using AutoServiceApp.Domain.Entities;
using AutoServiceApp.Domain.Enums;
using Data.InMemory.Repositories;
using Data.Interfaces;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AutoServiceApp
{
    public partial class MainWindow : Window
    {
        private User _currentUser;
        private DispatcherTimer _clockTimer;
        private readonly IRepairRequestRepository _repository;

        public MainWindow(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _repository = new RepairRequestRepository();
            InitializeComponent();
            _currentUser = user;
            InitializeClock();
            InitializeUserInfo();
            ApplyRoleRestrictions();
            InitializeFilters();
            LoadRequests();
        }

        private void InitializeFilters()
        {
            var statusItems = new List<object> { "Все статусы" };
            statusItems.AddRange(Enum.GetValues(typeof(RequestStatus)).Cast<object>());
            StatusFilter.ItemsSource = statusItems;
            StatusFilter.SelectedIndex = 0;
        }

        private void ApplyRoleRestrictions()
        {
            // Логика ограничений по ролям
        }

        private void InitializeClock()
        {
            _clockTimer = new DispatcherTimer();
            _clockTimer.Interval = TimeSpan.FromSeconds(1);
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();
        }

        private void InitializeUserInfo()
        {
            UserInfoText.Text = $"{_currentUser.FullName} ({_currentUser.Role})";
            Title = $"Автосервис 'АвтоТранс' - {_currentUser.FullName}";
        }

        private void LoadRequests()
        {
            try
            {
                LoadingProgress.Visibility = Visibility.Visible;
                StatusText.Text = "Загрузка данных...";

                var searchText = SearchTextBox.Text.Trim();
                var selectedItem = StatusFilter.SelectedItem;
                RequestStatus? status = selectedItem is RequestStatus statusEnum ? statusEnum : null;

                var requests = _repository.GetAll(searchText, status);
                RequestsGrid.ItemsSource = requests;
                UpdateItemsCount(requests.Count);

                StatusText.Text = "Данные успешно загружены";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                StatusText.Text = "Ошибка загрузки данных";
            }
            finally
            {
                LoadingProgress.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateItemsCount(int count)
        {
            ItemsCountText.Text = $"Заявок: {count}";
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            DateTimeText.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        private void AddRequest_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditRequestWindow();
            if (editWindow.ShowDialog() == true)
            {
                _repository.Add(editWindow.Request);
                LoadRequests();
                StatusText.Text = "Заявка добавлена успешно";
            }
        }

        private void EditRequest_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = RequestsGrid.SelectedItem as RepairRequest;
            if (selectedRequest != null)
            {
                var editWindow = new EditRequestWindow(selectedRequest);
                if (editWindow.ShowDialog() == true)
                {
                    if (_repository.Update(editWindow.Request))
                    {
                        LoadRequests();
                        StatusText.Text = "Заявка обновлена успешно";
                    }
                    else
                    {
                        MessageBox.Show("Ошибка обновления заявки", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteRequest_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = RequestsGrid.SelectedItem as RepairRequest;
            if (selectedRequest != null)
            {
                var result = MessageBox.Show($"Удалить заявку №{selectedRequest.Id}?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (_repository.Delete(selectedRequest.Id))
                    {
                        LoadRequests();
                        StatusText.Text = "Заявка успешно удалена";
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления заявки", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRequests();
            StatusText.Text = "Данные обновлены";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadRequests();
        }

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadRequests();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;
            StatusFilter.SelectedIndex = 0;
        }

        private void RequestsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusText.Text = RequestsGrid.SelectedItem != null ?
                $"Выбрана заявка №{(RequestsGrid.SelectedItem as RepairRequest)?.Id}" :
                "Заявка не выбрана";
        }

        private void RequestsGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditRequest_Click(sender, e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "Приложение загружено и готово к работе";
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _clockTimer.Stop();

            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UsersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал управления пользователями в разработке", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ReportsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var allRequests = _repository.GetAll();
            var reportWindow = new ReportWindow(allRequests);
            reportWindow.ShowDialog();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Автосервис 'АвтоТранс' - Система учета заявок\nВерсия 1.0.0",
                "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}