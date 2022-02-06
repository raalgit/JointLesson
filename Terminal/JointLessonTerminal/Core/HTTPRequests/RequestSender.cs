using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Configuration;

namespace JointLessonTerminal.Core.HTTPRequests
{
    public class RequestSender<TReq, TRes>
    {
        public static async Task<TRes> SendRequest(RequestModel<TReq> requestModel, string url)
        {
            TRes response = default;

            string serverUrl = ConfigurationManager.AppSettings["ServerUrl"].ToString();

            var httpRequest = (HttpWebRequest)WebRequest.Create(serverUrl + url);
            httpRequest.Method = requestModel.Method.ToString();

            string json = JsonSerializer.Serialize<TReq>(requestModel.Object);

            httpRequest.ContentLength = json.Length;
            httpRequest.ContentType = "application/json"; // TODO: вынести тип контента в настройку
            if (!string.IsNullOrEmpty(requestModel.JWT)) httpRequest.Headers.Add("Authorization", "Bearer " + requestModel.JWT);

            using (Stream postStream = httpRequest.GetRequestStream())
            {
                var jsonData = Encoding.UTF8.GetBytes(json);
                postStream.Write(jsonData, 0, jsonData.Length);

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
            }

            return response;
        }
    }
}
