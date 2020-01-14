﻿using AvenueOne.Interfaces;
using AvenueOne.Models;
using AvenueOne.Properties;
using AvenueOne.ViewModels.WindowsViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AvenueOne.ViewModels.WindowsViewModels
{
    
    public abstract class WindowViewModel : AccountViewModel, IWindowViewModel
    {
        public Window Window { get; }

        public WindowViewModel(Window window)
        {
            this.Window = window;
        }
    }
}
