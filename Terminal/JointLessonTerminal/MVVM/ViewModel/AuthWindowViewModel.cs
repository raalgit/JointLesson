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
                    }
                };
                try
                {
                    var response = await RequestSender<LoginRequest, LoginResponse>.SendRequest(loginRequest, "/auth/login");
                    var settings = UserSettings.GetInstance();
                    if (response.isSuccess) settings.JWT = response.jwt;
                }
                catch (Exception er)
                {
                    // TODO: вывод ошибки
                }
            });
        }
    }
}
