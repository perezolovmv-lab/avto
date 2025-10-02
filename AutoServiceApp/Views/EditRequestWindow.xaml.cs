using AutoServiceApp.Domain.Entities;
using AutoServiceApp.Domain.Enums;
using System;
using System.Linq;
using System.Windows;

namespace AutoServiceApp
{
    public partial class EditRequestWindow : Window
    {
        public RepairRequest Request { get; private set; }

        public EditRequestWindow()
        {
            InitializeComponent();
            Request = new RepairRequest();
            InitializeFields();
        }

        public EditRequestWindow(RepairRequest request) : this()
        {
            if (request != null)
            {
                Request = request;
                FillFormData();
            }
        }

        private void InitializeFields()
        {
            CreatedDateText.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(RequestStatus)).Cast<RequestStatus>();
            StatusComboBox.SelectedItem = RequestStatus.New; 
        }


        private void FillFormData()
        {
            RequestIdText.Text = Request.Id.ToString();
            CreatedDateText.Text = Request.CreatedDate.ToString("dd.MM.yyyy HH:mm");
            CarBrandTextBox.Text = Request.CarBrand;
            CarModelTextBox.Text = Request.CarModel;
            ClientNameTextBox.Text = Request.ClientName;
            PhoneTextBox.Text = Request.PhoneNumber;
            ProblemTextBox.Text = Request.ProblemDescription;
            StatusComboBox.SelectedItem = Request.Status;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateData())
            {
                try
                {
                    Request.CarBrand = CarBrandTextBox.Text.Trim();
                    Request.CarModel = CarModelTextBox.Text.Trim();
                    Request.ClientName = ClientNameTextBox.Text.Trim();
                    Request.PhoneNumber = PhoneTextBox.Text.Trim();
                    Request.ProblemDescription = ProblemTextBox.Text.Trim();
                    Request.Status = (RequestStatus)StatusComboBox.SelectedItem;

                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(CarBrandTextBox.Text))
            {
                MessageBox.Show("Введите вид авто", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                CarBrandTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(CarModelTextBox.Text))
            {
                MessageBox.Show("Введите модель авто", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                CarModelTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(ClientNameTextBox.Text))
            {
                MessageBox.Show("Введите ФИО клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                ClientNameTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                MessageBox.Show("Введите номер телефона", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                PhoneTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(ProblemTextBox.Text))
            {
                MessageBox.Show("Введите описание проблемы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                ProblemTextBox.Focus();
                return false;
            }

            return true;
        }
    }
}