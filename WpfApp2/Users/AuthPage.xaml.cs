using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Sessions;

namespace WpfApp2.Users
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void Login()
        {
            string email = emailTb.Text;
            string pass = passTb.Password;
            Task.Run(async () =>
            {
                try
                {
                    var result = await AuthManager.Instance.Login(email, pass);

                    await Dispatcher.BeginInvoke(() =>
                    {
                        if (result)
                        {
                            NavigationService?.GoBack();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось авторизоваться. Проверьте корректность введенных данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void AuthBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }
    }
}
