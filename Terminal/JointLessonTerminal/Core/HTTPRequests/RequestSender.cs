using JointLessonTerminal.MVVM.Model;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        public static async Task<TRes> SendRequest(RequestModel<TReq> requestModel, string route)
        {
            TRes response = default;

            // Получение полного адреса нужного api 
            string serverUrl = ConfigurationManager.AppSettings["ServerUrl"].ToString();
            var uri = new Uri(serverUrl);
            uri = new Uri(uri, route);
            if (!string.IsNullOrEmpty(requestModel.UrlFilter)) uri = new Uri(uri, requestModel.UrlFilter);

            string apiUrl = uri.ToString();

            // Создание HTTP запроса
            var httpRequest = (HttpWebRequest)WebRequest.Create(apiUrl);

            // Установка метода запроса
            httpRequest.Method = requestModel.Method.ToString();

            var settings = UserSettings.GetInstance();
            if (!string.IsNullOrEmpty(settings.JWT) && requestModel.UseCurrentToken) httpRequest.Headers.Add("Authorization", "Bearer " + settings.JWT);

            // Проверка типа запроса
            switch (requestModel.Method)
            {
                // Если POST запрос, то производим запись данных в тело запроса
                case Enums.RequestMethod.Post:

                    // Устновка типа передаваемых данных
                    httpRequest.ContentType = "application/json";
                    
                    string json = JsonSerializer.Serialize<TReq>(requestModel.Object);

                    httpRequest.ContentLength = json.Length;
                    
                    using (Stream postStream = httpRequest.GetRequestStream())
                    {
                        var jsonData = Encoding.UTF8.GetBytes(json);
                        postStream.Write(jsonData, 0, jsonData.Length);
                    }
                    break;

                // Если GET запрос, то 
                case Enums.RequestMethod.Get:

                    // Устновка типа передаваемых данных
                    httpRequest.ContentType = "text/plain";
                    break;

                // Если DELETE запрос, то 
                case Enums.RequestMethod.Delete:
                    break;

                // Если PUT запрос, то 
                case Enums.RequestMethod.Put:
                    break;

                default:
                    throw new ArgumentException(nameof(requestModel.Method));
            }

            WebResponse httpResponse = await httpRequest.GetResponseAsync();
            using (var responseStream = httpResponse.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var responseStreamReader = new StreamReader(responseStream, Encoding.Default);
                    var responseStr = responseStreamReader.ReadToEnd();
                    response = JsonSerializer.Deserialize<TRes>(responseStr);
                }
            }

            return response;
        }
    }
}
