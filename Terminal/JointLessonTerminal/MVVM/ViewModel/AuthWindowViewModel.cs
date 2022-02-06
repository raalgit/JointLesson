using JointLessonTerminal.Core;
using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.MVVM.Model;
using JointLessonTerminal.MVVM.Model.HttpModels.Request;
using JointLessonTerminal.MVVM.Model.HttpModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class AuthWindowViewModel : ObservableObject
    {
        /// <summary>
        /// Пример отправки HTTP запроса
        /// </summary>
        public RelayCommand SendRequestCommand { get; set; }


        public AuthWindowViewModel()
        {
            SendRequestCommand = new RelayCommand(async x =>
            {
                var loginRequest = new RequestModel<LoginRequest>()
                {
                    Method = Core.HTTPRequests.Enums.RequestMethod.Post,
                    Object = new LoginRequest()
                    {
                        Login = "test",
                        Password = "test"
                    },
                    UseCurrentToken = false
                };

                var logoutRequest = new RequestModel<object>()
                {
                    Method = Core.HTTPRequests.Enums.RequestMethod.Get,
                    UseCurrentToken = true
                };

                try
                {
                    var responsePost = await RequestSender<LoginRequest, LoginResponse>.SendRequest(loginRequest, "/auth/login");
                    var settings = UserSettings.GetInstance();
                    if (responsePost.isSuccess) settings.JWT = responsePost.jwt;

                    var responseGet = await RequestSender<object, LogoutResponse>.SendRequest(logoutRequest, "/auth/logout");
                    if (responseGet.isSuccess) settings.JWT = string.Empty;

                    responseGet = await RequestSender<object, LogoutResponse>.SendRequest(logoutRequest, "/auth/logout");
                    if (responseGet.isSuccess) settings.JWT = string.Empty;
                }
                catch (Exception er)
                {
                    // TODO: вывод ошибки
                }
            });
        }
    }
}
