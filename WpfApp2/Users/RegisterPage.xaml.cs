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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
            passTb.ToolTip = "Пароль должен соответствовать требованиям, включать в себя заглавные и прописные английские буквы, и специальный символ, а также быть минимум 8 символов.";
            pass2Tb.ToolTip = "Повторите пароль.";

        }

        bool CheckPass()
        {
            
            if (passTb.Password != pass2Tb.Password)
            {
                MessageBox.Show("Пароль не совпадает", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Register()
        {
            string email = emailTb.Text;
            string pass = passTb.Password;
            string name = nameTb.Text;
            string family = familyTb.Text;
            string patronymic = patronymicTb.Text;
            Task.Run(async () =>
            {
                try
                {
                    var result = await AuthManager.Instance.Register(email, pass, name, family, patronymic);

                    await Dispatcher.BeginInvoke(() =>
                    {
                        if (result)
                        {
                            NavigationService?.GoBack();
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

            if(CheckPass()) 
            {
                Register();
            }
        }

        //private void passTb_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    if ((passTb.Password) is (@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
        //    {
                
        //        passTb.SetCurrentValue(Control.BackgroundProperty, Brushes.White);
        //    }
        //    else
        //    {
               
        //    }
        //}
    }
}
