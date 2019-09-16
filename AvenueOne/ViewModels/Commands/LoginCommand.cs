﻿using AvenueOne.Interfaces;
using AvenueOne.Models;
using AvenueOne.Properties;
using AvenueOne.Services.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AvenueOne.ViewModels.Commands
{
    public class LoginCommand : ICommand
    {
        public ILoginViewModel ViewModel { get; set; }
        //public IUser User { get; set; }
        private ILoginService _loginService;
        private IDisplayService _displayService;

        //public LoginCommand(ILoginViewModel loginWindowViewModel)
        public LoginCommand(ILoginService loginService, IDisplayService displayService)
        {
            //_viewModel = loginWindowViewModel;
            //_user = user;
            _loginService = loginService;
            _displayService = displayService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("Need object as parameter with values username and password.");
            try
            {
                //get parameters
                object[] values = (object[])parameter;

                //get username
                TextBox usernameTextBlock = (TextBox)values[0];
                string username = usernameTextBlock.Text;
                //get password
                PasswordBox passwordBox = (PasswordBox)values[1];
                string password = passwordBox.Password;

                IUser user = ViewModel.User;
                //_viewModel.Login(username, password);
                if (user == null)
                    throw new NullReferenceException("User cannot be null");

                //User.Username = username;
                user.Password = password;

                if (!user.IsValidProperty("Password") || !user.IsValidProperty("Username") || !user.IsValid)
                {
                    _displayService.MessageDisplay("Invalid input");
                }
                else
                {
                    IUser userLogin = _loginService.Login(user);

                    if (userLogin == null)
                    {
                        _displayService.MessageDisplay("Invalid login");
                    }
                    else
                    {

                        userLogin.Password = null; //TODO: redundancy, i think no longer necessary since it is already null in the loginService;
                        Settings.Default.UserAccount = userLogin as User;
                        Settings.Default.UserProfile = userLogin.Person;
                        Settings.Default.Save();
                        _displayService.MessageDisplay($"Welcome {userLogin.Person.FullName} using account {userLogin.Username}.");

                        Window mainWindow = _displayService.CreateMainWindow();
                        mainWindow.Show();

                        if(ViewModel != null)
                            ViewModel.Window.Close();
                    }
                }

            }
            catch (Exception)
            {
                //perhaps log something here.
                throw;
            }
        }
    }
}
