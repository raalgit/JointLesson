﻿using JointLessonTerminal.Core;
using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.MVVM.Model;
using JointLessonTerminal.MVVM.Model.HttpModels.Request;
using JointLessonTerminal.MVVM.Model.HttpModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class AuthWindowViewModel : ObservableObject
    {

        #region открытые поля
        public RelayCommand SendRequestCommand { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        #endregion


        public AuthWindowViewModel()
        {
            // Событие при нажатии на кнопку
            SendRequestCommand = new RelayCommand(async x =>
            {
                // Если пароль и/или логин не ввели
                if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
                {
                    var signal = new WindowEvent();
                    signal.Type = WindowEventType.AUTH_EMPTYLOGIN;
                    SendEventSignal(signal);
                    return;
                }

                // Создание модели http запроса
                var loginRequest = new RequestModel<LoginRequest>()
                {
                    Method = Core.HTTPRequests.Enums.RequestMethod.Post,
                    Body = new LoginRequest()
                    {
                        Login = Login,
                        Password = Password
                    },
                    UseCurrentToken = false
                };

                try
                {
                    // Отправка http запроса для авторизации
                    var sender = new RequestSender<LoginRequest, LoginResponse>();
                    var responsePost = await sender.SendRequest(loginRequest, "/auth/login");
                    var settings = UserSettings.GetInstance();

                    // Если авторизация прошла
                    if (responsePost.isSuccess)
                    {
                        // Сохранение токена авторизации
                        settings.JWT = responsePost.jwt;
                        settings.CurrentUser = responsePost.user;
                        settings.Roles = responsePost.roles.ToArray();

                        // Отправка сигнала главному окну о завершении авторизации
                        var signal = new WindowEvent();
                        signal.Type = WindowEventType.AUTHORIZED;
                        SendEventSignal(signal);
                    }
                    else
                    {
                        var signal = new WindowEvent();
                        signal.Type = WindowEventType.AUTH_ERROR;
                        signal.Argument = "Во время авторизации возникла ошибка!";
                        SendEventSignal(signal);
                    }
                }
                catch (Exception er)
                {
                    var signal = new WindowEvent();
                    signal.Type = WindowEventType.AUTH_ERROR;
                    signal.Argument = er.Message;
                    SendEventSignal(signal);
                }
            });
        }
    }
}
