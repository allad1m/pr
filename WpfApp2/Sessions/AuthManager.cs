using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebApplication1.Users;

namespace WpfApp2.Sessions
{
    internal class AuthManager : INotifyPropertyChanged
    {
        private static readonly Lazy<AuthManager> _instance = new(() => new AuthManager(), isThreadSafe: true);
        public static AuthManager Instance
        {
            get { return _instance.Value; }
        }
        // Создаем событие
        public event PropertyChangedEventHandler? PropertyChanged;
        // Создаем метод OnPropertyChanged, чтобы вызывать событие.
        // Имя вызывающего обхекта будет использоваться в качестве параметра.
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        //проверяем, есть ли токен. Если есть - пользователь авторизован.
        private bool _authorized = !string.IsNullOrEmpty(TokenManager.Instance.AccessToken);
        public bool Authorized
        {
            get { return _authorized; }
            set
            {
                _authorized = value;
                OnPropertyChanged();
            }
        }

        private UserDTO _userDTO;
        public UserDTO User
        {
            
            get { return _userDTO; }
            set
            {
                _userDTO = value;
                OnPropertyChanged();
            }
        }
        //идентификатор клиента
        private string userAgent
        {
            get
            {
                string appVersion = System.Reflection.Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? "unknown";
                string osVersion = Environment.OSVersion.ToString();
                return $"NewsPortal/{appVersion} ({osVersion})";
            }
        }
        private IAuthService service = NetworkManager.Instance.AuthService;
        public async Task<bool> Login(string email, string pass)
        {
            try
            {
                var result = await service.Login(new() { Email = email, Password = pass }, userAgent);
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    TokenManager.Instance.AccessToken = result.Token;
                    Authorized = true;
                });
                return true;
            }
            catch (Exception ex)
            {
                //раскомментировать если не понятно из за чего ошибка
                //MessageBox.Show(ex.Message);    
                return false;
            }
        }
        public async Task<bool> Logout()
        {
            try
            {
                await service.Logout();
                await ForceLogout();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message); 
                return false;
            }
        }
        //выход из аккаунта
        public async Task ForceLogout()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                TokenManager.Instance.AccessToken = null;
                Authorized = false;
            });
        }

        public async Task<bool> Register(string email, string pass, string name, string family, string patronymic)
        {
            try
            {
                var result = await service.Register(new() { Email = email, Password = pass, Name = name, Family = family, Patronymic = patronymic }, userAgent);
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    TokenManager.Instance.AccessToken = result.Token;
                    Authorized = true;
                });
                return true;
            }
            catch (Exception ex)
            {
                //раскомментировать если не понятно из за чего ошибка
                MessageBox.Show(ex.Message);    
                return false;
            }
        }

        public async Task<bool> ChangeEmail(string email)
        {
            try
            {
                var result = await service.ChangeEmail(new() { Email = email });
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    User = result;
                });
                return true;
            }
            catch (Exception ex)
            {
                //раскомментировать если не понятно из за чего ошибка
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public async Task<bool> ChangeFio(string family, string name, string patronymic)
        {
            try
            {
                var result = await service.ChangeFio(new() { Family = family, Name = name, Patronymic = patronymic });
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    User = result;
                });
                return true;
            }
            catch (Exception ex)
            {
                //раскомментировать если не понятно из за чего ошибка
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public async Task<bool> ChangePassword(string password, string repeat)
        {
            try
            {
                var result = await service.ChangePassword(new() { Password = password, Repeat= repeat });
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    User = result;
                });
                return true;
            }
            catch (Exception ex)
            {
                //раскомментировать если не понятно из за чего ошибка
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
