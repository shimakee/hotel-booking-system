﻿using AvenueOne.Core;
using AvenueOne.Interfaces;
using AvenueOne.Models;
using AvenueOne.Services;
using AvenueOne.Services.Interfaces;
using AvenueOne.ViewModels.Commands.ClassCommands;
using AvenueOne.ViewModels.TabViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AvenueOne.ViewModels.Commands.UserCommands
{
    public class UpdateUserCommand : UpdateClassCommand<User>, IBaseClassCommand<User>
    {
        public UpdateUserCommand(IGenericUnitOfWork<User> genericUnitOfWork, IDisplayService displayService)
            : base(genericUnitOfWork, displayService)
        {

        }

        public override async void Execute(object parameter)
        {
            try
            {
                Validate();
                if (ViewModel.ModelSelected.Person == null)
                        throw new NullReferenceException("No profile to update.");
                if (!ViewModel.ModelSelected.IsValid || !ViewModel.ModelSelected.Person.IsValid)
                    throw new InvalidOperationException("Invalid entry in user account.");


                UserViewModel viewmodel = (UserViewModel)ViewModel;

                //get parameters
                object[] values = (object[])parameter ?? throw new NullReferenceException("parameter cannot be null, you need to pass password and password confirmbox");

                //CheckBox IsPasswordIncludedCheckBox = (CheckBox)values[0];
                PasswordBox passwordBox = (PasswordBox)values[0];
                PasswordBox passwordConfirmBox = (PasswordBox)values[1];

                //is password included in update
                //bool IsPasswordIncluded = IsPasswordIncludedCheckBox.IsChecked.GetValueOrDefault();
                bool IsPasswordIncluded = viewmodel.IsPasswordIncluded;
                //retain password
                ViewModel.ModelSelected.Password = ViewModel.Model.Password;
                ViewModel.ModelSelected.PasswordConfirm = ViewModel.Model.PasswordConfirm;

                if (IsPasswordIncluded)
                {
                    if (!ViewModel.ModelSelected.IsValidProperty("Password"))
                        throw new InvalidOperationException("Invalid entry on password.");
                    if (!ViewModel.ModelSelected.IsValidProperty("PasswordConfirm"))
                        throw new InvalidOperationException("Invalid entry on password confirmation.");

                    ViewModel.ModelSelected.Password = HashService.Hash(passwordBox.Password);
                    ViewModel.ModelSelected.PasswordConfirm = ViewModel.ModelSelected.Password;
                }

                PreUpdate();
                int n = await Update();

                if (n <= 0)
                    throw new InvalidOperationException("Could not edit accout.");
                _displayService.MessageDisplay($"Edited account: {ViewModel.Model.Username}.\nAffected rows: {n}.");
            }
            catch (DbEntityValidationException dbEx)
            {
                _displayService.ErrorDisplay(dbEx.Message, "Validation exception.");
            }
            catch (NullReferenceException nullEx)
            {
                _displayService.ErrorDisplay(nullEx.Message, "Null reference exception.");
            }
            catch(InvalidOperationException inEx)
            {
                _displayService.ErrorDisplay(inEx.Message, "Invalid operation exception.");
            }
            catch (Exception ex)
            {
                _displayService.ErrorDisplay(ex.Message, "Error");
                throw;
            }
        }

        protected override async Task<int> Update()
        {
            //to retain atleast 1 admin user when updating.
            List<User> adminUsers = await Task.Run(() => _genericUnitOfWork.Repositories[typeof(User)].Find(u => u.IsAdmin == true).ToList());
            if (adminUsers.Count <= 1 && ViewModel.ModelSelected.IsAdmin == false)
                ViewModel.ModelSelected.IsAdmin = true;

            User user = await Task.Run(() => _genericUnitOfWork.Repositories[typeof(User)].GetAsync(ViewModel.Model.Id)) ?? throw new NullReferenceException("Account does not exist.");

            user.Username = ViewModel.ModelSelected.Username;
            user.IsAdmin = ViewModel.ModelSelected.IsAdmin;
            user.Password = ViewModel.ModelSelected.Password;
            user.PasswordConfirm = ViewModel.ModelSelected.PasswordConfirm;
            ViewModel.ModelSelected.Person.Id = user.Person.Id;
            ViewModel.ModelSelected.Person.DateAdded = user.Person.DateAdded;
            ViewModel.ModelSelected.Person.User = user;
            ViewModel.ModelSelected.Person.DeepCopyTo(user.Person);

            return await Task.Run(() => _genericUnitOfWork.CompleteAsync());
        }

        protected override void PreUpdate()
        {
            base.PreUpdate();
            ViewModel.ModelSelected.Person.DateModified = DateTime.Now;
        }
    }
}
