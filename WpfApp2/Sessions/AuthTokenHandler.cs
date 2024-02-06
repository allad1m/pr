using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Sessions
{
    internal class AuthTokenHandler : DelegatingHandler
    {
        public AuthTokenHandler()
        {
            InnerHandler = new HttpClientHandler();
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //получаем токен
            string token = TokenManager.Instance.AccessToken;
            //добавляем его в заголовок запроса
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            //отправляем запрос
            var response = await base.SendAsync(request, cancellationToken);
            //если токен недействителен – удалаяем его, и разлогиниваем юзера.
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await AuthManager.Instance.ForceLogout();
            }
            return response;
        }

    }
}
