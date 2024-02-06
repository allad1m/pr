using LoadingIndicators.WPF;
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
using WebApplication1.Users;
using WpfApp2.Sessions;

namespace WpfApp2.Users
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private UserDTO user;
        public ProfilePage()
        {
            InitializeComponent();
            SetProfile();
            familyTb.IsReadOnly = true;
            nameTb.IsReadOnly = true;
            patronymicTb.IsReadOnly = true;
            emailTb.IsReadOnly = true;


        }

        private void SetProfile()
        {

            Task.Run(async () =>
            {
                try
                {
                    var service = NetworkManager.Instance.AuthService;
                    var response = await service.Profile();
                    await Dispatcher.BeginInvoke(() =>
                    {
                        if (response == null)
                        {
                          
                        }
                        else
                        {
                            user = response.User;
                            familyTb.Text = user.Family;
                            nameTb.Text = user.Name;
                            patronymicTb.Text = user.Patronymic;
                            emailTb.Text = user.Email;
                        }
                    });
                }
                catch (Exception ex)
                {
                    await Dispatcher.BeginInvoke(() =>
                    {
                       
                    });
                }
                finally
                {
                    await Dispatcher.BeginInvoke(() =>
                    {
                        
                    });
                }
            });
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthManager.Instance.ForceLogout();
            NavigationService?.GoBack();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ChangePage());
        }
    }
}
