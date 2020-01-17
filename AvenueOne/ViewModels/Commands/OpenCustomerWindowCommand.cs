﻿using AvenueOne.Interfaces;
using AvenueOne.Persistence.Repositories;
using AvenueOne.ViewModels.WindowsViewModels.Interfaces;
using AvenueOne.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvenueOne.ViewModels.Commands
{
    public class OpenCustomerWindowCommand : ICommand
    {

        public ICustomerTabViewModel ViewModel { get; set; }
        private PlutoContext _plutoContext;

        public OpenCustomerWindowCommand(PlutoContext plutoContext)
        {
            this._plutoContext = plutoContext ?? throw new ArgumentNullException("Pluto context cannot be null.");
        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {

            AddCustomerWindow customerWindow = new AddCustomerWindow(_plutoContext);
            customerWindow.ShowDialog();
        }
    }
}
