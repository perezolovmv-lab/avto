using System;
using System.Globalization;
using System.Windows.Data;
using AutoServiceApp.Domain.Enums;

namespace AutoServiceApp.Converters
{
    /// <summary>
    /// Конвертер статуса заявки в текст для отображения в UI.
    /// </summary>
    public class StatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RequestStatus status)
            {
                return status switch
                {
                    RequestStatus.New => "Новая",
                    RequestStatus.InProgress => "В процессе",
                    RequestStatus.Completed => "Завершена",
                    _ => value.ToString() ?? string.Empty
                };
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string statusText)
            {
                return statusText switch
                {
                    "Новая" => RequestStatus.New,
                    "В процессе" => RequestStatus.InProgress,
                    "Завершена" => RequestStatus.Completed,
                    _ => Binding.DoNothing
                };
            }

            return Binding.DoNothing;
        }
    }
}
