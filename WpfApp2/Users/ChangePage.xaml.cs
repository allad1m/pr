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
using Windows.System;
using WpfApp2.Sessions;
using WebApplication1.Users;
using WpfApp2.News;

namespace WpfApp2.Users
{
    /// <summary>
    /// Логика взаимодействия для ChangePage.xaml
    /// </summary>
    public partial class ChangePage : Page
    {
        private UserDTO user;
        public ChangePage()
        {
            InitializeComponent();
            passTb.ToolTip = "Пароль должен соответствовать требованиям, включать в себя заглавные и прописные английские буквы, и специальный символ, а также быть минимум 8 символов.";
            repeatTb.ToolTip = "Повторите пароль.";
            SetProfile();
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

        private void Change()
        {
            string email = emailTb.Text;
            string pass = passTb.Password;
            string name = nameTb.Text;
            string family = familyTb.Text;
            string patronymic = patronymicTb.Text;
            string repeat = repeatTb.Password;
            Task.Run(async () =>
            {
                try
                {
                    if (name != user.Name || family != user.Family || patronymic != user.Patronymic)
                    {
                        var result1 = await AuthManager.Instance.ChangeFio(name, family, patronymic);
                    }

                    if (email != user.Email)
                    {
                        var result2 = await AuthManager.Instance.ChangeEmail(email);
                    }
                    if (pass != "")
                    {
                        var result3 = await AuthManager.Instance.ChangePassword(pass, repeat);
                    }

                    await Dispatcher.BeginInvoke(() =>
                    {
                        if(true)
                        {
                            NavigationService.Navigate(new NewsPage());
                        }
                        else
                        {
                            MessageBox.Show("Не удалось зарегистрироваться. Проверьте корректность введенных данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (passTb.Password == repeatTb.Password)
            {
                Change();
            }
        }
    }
}
