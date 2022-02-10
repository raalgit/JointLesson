using JointLessonTerminal.MVVM.Model;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace JointLessonTerminal.Core.HTTPRequests
{
    
    public class RequestSender<TReq, TRes>
    {
        /// <summary>
        /// Отправка HTTP запроса на сервера
        /// </summary>
        /// <param name="requestModel">Данные запроса</param>
        /// <param name="route">адрес api</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<TRes> SendRequest(RequestModel<TReq> requestModel, string route)
        {
            TRes response = default;

            // Получение полного адреса нужного api 
            string serverUrl = ConfigurationManager.AppSettings["ServerUrl"].ToString();
            var uri = new Uri(serverUrl);
            uri = new Uri(uri, route);
            if (!string.IsNullOrEmpty(requestModel.UrlFilter)) uri = new Uri(uri, requestModel.UrlFilter);

            var result = (HttpResponseMessage)null;
            var requestParams = (HttpRequestMessage)null;

            using (var httpclient = new HttpClient())
            {
                // Создание HTTP запроса
                requestParams = new HttpRequestMessage();
                requestParams.RequestUri = uri;

                var settings = UserSettings.GetInstance();
                if (!string.IsNullOrEmpty(settings.JWT) && requestModel.UseCurrentToken) requestParams.Headers.Add("Authorization", "Bearer " + settings.JWT);

                // Проверка типа запроса
                switch (requestModel.Method)
                {
                    case Enums.RequestMethod.Post:
                        requestParams.Method = HttpMethod.Post;
                        string jsonPost = JsonSerializer.Serialize<TReq>(requestModel.Body);
                        var contentPost = new StringContent(jsonPost, Encoding.UTF8, "application/json");
                        requestParams.Content = contentPost;
                        result = await httpclient.SendAsync(requestParams);
                        break;

                    case Enums.RequestMethod.Get:
                        requestParams.Method = HttpMethod.Get;
                        requestParams.Headers.Add("Accept", "text/plain");
                        result = await httpclient.SendAsync(requestParams);
                        break;

                    case Enums.RequestMethod.Delete:
                        requestParams.Method = HttpMethod.Delete;
                        result = await httpclient.SendAsync(requestParams);
                        break;

                    case Enums.RequestMethod.Put:
                        requestParams.Method = HttpMethod.Put;
                        string jsonPut = JsonSerializer.Serialize<TReq>(requestModel.Body);
                        var contentPut = new StringContent(jsonPut, Encoding.UTF8, "application/json");
                        requestParams.Content = contentPut;
                        result = await httpclient.SendAsync(requestParams);
                        break;

                    default:
                        throw new ArgumentException(nameof(requestModel.Method));
                }
            }

            using (var streamResponse = await result.Content.ReadAsStreamAsync())
            {
                if (streamResponse != null)
                {
                    var responseStreamReader = new StreamReader(streamResponse, Encoding.UTF8);
                    var responseStr = responseStreamReader.ReadToEnd();
                    response = JsonSerializer.Deserialize<TRes>(responseStr);
                }
            }

            result.EnsureSuccessStatusCode();

            requestParams.Dispose();
            result.Dispose();

            return response;
        }
    }
}
