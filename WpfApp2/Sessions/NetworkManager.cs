using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Comments;
using WebApplication1.Comments;
using WpfApp2.News;

namespace WpfApp2.Sessions
{
    internal class NetworkManager
    {
        private static readonly Lazy<NetworkManager> _instance = new(() => new NetworkManager(), isThreadSafe: true);
        public static NetworkManager Instance
        {
            get { return _instance.Value; }
        }
        //указываем хост
        private static readonly string baseUrl = "http://localhost:5221";
        //создаем клиент
        private readonly HttpClient client = new(new AuthTokenHandler())
        {
            BaseAddress = new(baseUrl),
        };
        //создаем сервисы
        public IAuthService AuthService
        {
            get { return RestService.For<IAuthService>(client); }
        }
        public INewsService NewsService
        {
            get { return RestService.For<INewsService>(client); }
        }
        public ICommentsService CommmentsService
        {
            get { return RestService.For<ICommentsService>(client); }
        }
    }

}
