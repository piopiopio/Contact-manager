using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        MainWindow a;
        bool validatedEmail = false;
        Brush brush;
        public RegistrationWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelBase();
            brush = RegisterEmail.BorderBrush;
        }

        private void RegisterCancel_Click(object sender, RoutedEventArgs e)
        {
            Owner.Opacity = 1;
            this.Close();
        }

        private void RegisterRegster_Click(object sender, RoutedEventArgs e)
        {
            a = (MainWindow)Owner;
            if ((RegisterPassword.Password == RegisterConfirmPassword.Password) && validatedEmail && RegisterPassword.Password != "" && RegisterLogin.Text != "")
            {
                a.RegisterUser(RegisterLogin.Text, RegisterPassword.Password);
                Owner.Opacity = 1;
            }
            else
            {
                MessageBox.Show("You have to fill all fields", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            bool result = regex.IsMatch(RegisterEmail.Text);


            if (result)
            {
                RegisterEmailGrid.Visibility = Visibility.Hidden;
                validatedEmail = true;
                RegisterEmail.BorderThickness = new Thickness(1);
                RegisterEmail.BorderBrush = brush;
                //RegisterEmail.BorderBrush = SystemColors.ActiveBorderBrush;

            }
            else
            {
                validatedEmail = false;
                RegisterEmailGrid.Visibility = Visibility.Visible;
                RegisterEmail.BorderThickness = new Thickness(2);
                RegisterEmail.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));


            }

        }







    }
}
