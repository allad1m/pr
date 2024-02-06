using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Sessions
{
    public class TokenManager
    {
        // создаем Singleton-класс с потокобезопасностью
        private static readonly Lazy<TokenManager> _instance = new(() => new TokenManager(), isThreadSafe: true);
        public static TokenManager Instance
        {
            get { return _instance.Value; }
        }
        //токены будем хранить в папке с данными приложения
        private readonly string tokenPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.txt");
        private string? _token;
        private static readonly object sync = new();
        public string? AccessToken
        {
            get
            {
                _token ??= ReadToken();
                return _token;
            }
            set
            {
                lock (sync)
                {
                    _token = value;
                }
                WriteToken(value);
            }
        }

        private string? ReadToken()
        {
            try
            {
                return File.ReadAllText(tokenPath);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void WriteToken(string? token)
        {
            if (token == null)
            {
                File.Delete(tokenPath);
            }
            else
            {
                File.WriteAllText(tokenPath, token);
            }
        }
    }
}
