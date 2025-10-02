using AutoServiceApp.Domain.Entities;
using AutoServiceApp.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls; 

namespace AutoServiceApp
{
    public partial class ReportWindow : Window
    {
        // --- Приватные поля ---
        // Список заявок, на основе которых будут генерироваться отчеты.
        private List<RepairRequest> requests;

        // --- Конструктор окна ---
        public ReportWindow(List<RepairRequest> requests)
        {
            InitializeComponent(); 
            this.requests = requests; 
            ReportTypeComboBox.SelectedIndex = 0; 
            GenerateReport(); 
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            GenerateReport(); 
        }

        // Обработчик изменения выбранного типа отчета в ComboBox.
        private void ReportTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GenerateReport();
        }
        
        private void ExportReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал экспорта отчета в разработке", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

       
        private void GenerateReport()
        {
            switch (ReportTypeComboBox.SelectedIndex)
            {
                case 0: 
                    GenerateStatusReport();
                    break;
                case 1: 
                    GenerateFinancialReport();
                    break;
                case 2: 
                    GenerateMechanicsReport();
                    break;
            }
        }
        private void GenerateStatusReport()
        {
            var statusGroups = requests
                .GroupBy(r => r.Status)
                .Select(g => new
                {
                    Status = g.Key switch
                    {
                        RequestStatus.New => "Новая",
                        RequestStatus.InProgress => "В процессе",
                        RequestStatus.Completed => "Завершена",
                        _ => g.Key.ToString()
                    },
                    Count = g.Count(),
                    Percentage = (double)g.Count() / requests.Count * 100
                })
                .ToList();
            ReportDataGrid.ItemsSource = statusGroups;
            SummaryText.Text = $"Всего заявок: {requests.Count}";
        }

        private void GenerateFinancialReport()
        {
            var financialData = requests
                .Where(r => r.Status == RequestStatus.Completed);              
}
        private void GenerateMechanicsReport()
        {
            var mechanicsData = requests
                .GroupBy(r => "Все заявки")
                .Select(g => new
                {
                    Категория = g.Key,
                    ВсегоЗаявок = g.Count(),
                    Новых = g.Count(r => r.Status == RequestStatus.New),
                    ВПроцессе = g.Count(r => r.Status == RequestStatus.InProgress),
                    Завершенных = g.Count(r => r.Status == RequestStatus.Completed)
                })
                .ToList();

            ReportDataGrid.ItemsSource = mechanicsData; 
            SummaryText.Text = $"Статистика по всем заявкам"; 
        }
    }
}