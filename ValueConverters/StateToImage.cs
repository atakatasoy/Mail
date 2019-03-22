using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MailSender
{
    [ValueConversion(typeof(bool), typeof(BitmapImage))]
    public class StateToImage : IValueConverter
    {
        public static StateToImage Instance = new StateToImage();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var deger=(bool)value;

            if (deger) return new BitmapImage(new Uri("pack://application:,,,/Images/Icons/success.png"));

            return new BitmapImage(new Uri("pack://application:,,,/Images/Icons/unchecked.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
