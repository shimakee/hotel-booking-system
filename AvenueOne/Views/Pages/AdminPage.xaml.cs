﻿using AvenueOne.Core;
using AvenueOne.Core.Models;
using AvenueOne.Core.Models.Interfaces;
using AvenueOne.Interfaces;
using AvenueOne.Interfaces.RepositoryInterfaces;
using AvenueOne.Models;
using AvenueOne.Persistence;
using AvenueOne.Persistence.Repositories;
using AvenueOne.Services;
using AvenueOne.Services.Interfaces;
using AvenueOne.Utilities;
using AvenueOne.ViewModels;
using AvenueOne.ViewModels.Commands;
using AvenueOne.ViewModels.Commands.ClassCommands;
using AvenueOne.ViewModels.Commands.CustomerCommands;
using AvenueOne.ViewModels.Commands.RoomCommands;
using AvenueOne.ViewModels.Commands.UserCommands;
using AvenueOne.ViewModels.Commands.WindowCommands;
using AvenueOne.ViewModels.PagesViewModels;
using AvenueOne.ViewModels.TabViewModels;
using AvenueOne.ViewModels.WindowsViewModels;
using AvenueOne.ViewModels.WindowsViewModels.Interfaces;
using AvenueOne.Views.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AvenueOne.Views.Pages
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private PlutoContext _context;
        public Window Window { get;}
        public AdminPageViewModel ViewModel { get; private set; }
        private RoomTypeViewModel _roomTypeViewModel;
        private ObservableCollection<Amenities> _amenitiesCollection;
        public AdminPage(PlutoContext context, Window window)
        {
            if (context == null)
                throw new ArgumentNullException("Context cannot be null.");
            if (window == null)
                throw new ArgumentNullException("Window page caller must not be null.");

            InitializeComponent();

            this._context = context;
            this.Window = window;
            User User = new User();
            User.Person = new Person();
            
            IDisplayService displayService = new WpfDisplayService();

            #region userTab
            IGenericUnitOfWork<User> genericUnitOfWorkUser = new GenericUnitOfWork<User>(_context);
            BaseClassCommand<User> createUserCommand = new CreateUserCommand(genericUnitOfWorkUser, displayService);
            BaseClassCommand<User> updateUserCommand = new UpdateUserCommand(genericUnitOfWorkUser, displayService);
            BaseClassCommand<User> deleteUserCommand = new DeleteUserCommand(genericUnitOfWorkUser, displayService);
            ClearClassCommand<User> clearUserCommand = new ClearUserCommand();
            IUserViewModel userTab = new UserViewModel(new Person(), User, _context.Users.Local,
                                                                                                        createUserCommand,
                                                                                                        updateUserCommand,
                                                                                                        deleteUserCommand,
                                                                                                        clearUserCommand);
            userTab.Window = this.Window;
            #endregion

            #region Customer tab
                Customer Customer = new Customer();
            Customer.Person = new Person();
            IGenericUnitOfWork<Customer> genericUnitOfWorkCustomer = new GenericUnitOfWork<Customer>(context);
                BaseClassCommand<Customer> createCustomerCommand = new CreateClassCommand<Customer>(genericUnitOfWorkCustomer, displayService);
                BaseClassCommand<Customer> updateCustomerCommand = new UpdateCustomerCommand(genericUnitOfWorkCustomer, displayService);
                BaseClassCommand<Customer> deleteCustomerCommand = new DeleteClassCommand<Customer>(genericUnitOfWorkCustomer, displayService);
                ClearClassCommand<Customer> clearCustomerCommand = new ClearCustomerCommand();
                ICustomerViewModel customerTab = new CustomerViewModel(new Person(), Customer, _context.Customers.Local,
                                                                                                                            createCustomerCommand,
                                                                                                                            updateCustomerCommand,
                                                                                                                            deleteCustomerCommand,
                                                                                                                            clearCustomerCommand);
            customerTab.Window = this.Window;
            #endregion

            #region RoomTab
                #region Amenities view model


            GenericUnitOfWork<Amenities> genericUnitOfWorkAmenities = new GenericUnitOfWork<Amenities>(context);
            BaseClassCommand<Amenities> createAmenitiesCommand = new CreateClassCommand<Amenities>(genericUnitOfWorkAmenities, displayService);
            BaseClassCommand<Amenities> updateAmenitiesCommand = new UpdateClassCommand<Amenities>(genericUnitOfWorkAmenities, displayService);
            BaseClassCommand<Amenities> deleteAmenitiesCommand = new DeleteClassCommand<Amenities>(genericUnitOfWorkAmenities, displayService);
            ClearClassCommand<Amenities> clearAmenitiesCommand = new ClearClassCommand<Amenities>();
            IBaseObservableViewModel<Amenities> amenitiesViewModel = new BaseObservableViewModel<Amenities>(new Amenities(),
                                                                                                            context.Amenities.Local,
                                                                                                            createAmenitiesCommand,
                                                                                                            updateAmenitiesCommand,
                                                                                                            deleteAmenitiesCommand,
                                                                                                            clearAmenitiesCommand);
                #endregion

                #region RoomType view model

                GenericUnitOfWork<RoomType> genericUnitOfWorkRoomType = new GenericUnitOfWork<RoomType>(context);
                BaseClassCommand<RoomType> createRoomTypeCommand = new CreateRoomTypeCommand(genericUnitOfWorkRoomType, displayService);
            BaseClassCommand<RoomType> updateRoomTypeCommand = new UpdateClassCommand<RoomType>(genericUnitOfWorkRoomType, displayService);
                BaseClassCommand<RoomType> deleteRoomTypeCommand = new DeleteClassCommand<RoomType>(genericUnitOfWorkRoomType, displayService);
                ClearClassCommand<RoomType> clearRoomTypeCommand = new ClearClassCommand<RoomType>();
                LinkAmenityCommand linkAmenityCommand = new LinkAmenityCommand(genericUnitOfWorkRoomType, displayService);
                DetachAmenityCommand detachAmenityCommand = new DetachAmenityCommand(genericUnitOfWorkRoomType, displayService);
                RoomTypeViewModel roomTypeViewModel = new RoomTypeViewModel(new RoomType(),
                                                                                _context.RoomType.Local,
                                                                                createRoomTypeCommand,
                                                                                updateRoomTypeCommand,
                                                                                deleteRoomTypeCommand,
                                                                                clearRoomTypeCommand,
                                                                                linkAmenityCommand,
                                                                                detachAmenityCommand);
                #endregion

                #region Room view model

                IGenericUnitOfWork<Room> genericUnitOfWorkRoom = new GenericUnitOfWork<Room>(context);
                BaseClassCommand<Room> createRoomCommand = new CreateClassCommand<Room>(genericUnitOfWorkRoom, displayService);
                BaseClassCommand<Room> updateRoomCommand = new UpdateClassCommand<Room>(genericUnitOfWorkRoom, displayService);
                BaseClassCommand<Room> deleteRoomCommand = new DeleteClassCommand<Room>(genericUnitOfWorkRoom, displayService);
                ClearClassCommand<Room> clearRoomCommand = new ClearClassCommand<Room>();
                IRoomViewModel roomViewModel = new RoomViewModel(new Room(),
                                                                                                                context.Room.Local,
                                                                                                                context.RoomType.Local,
                                                                                                                createRoomCommand,
                                                                                                                updateRoomCommand,
                                                                                                                deleteRoomCommand,
                                                                                                                clearRoomCommand);
                #endregion

                RoomTabViewModel roomTab = new RoomTabViewModel(amenitiesViewModel,
                                                                    roomTypeViewModel,
                                                                    roomViewModel);
            #endregion

            BaseWindowCommand closeWindowCommand = new CloseWindowCommand();
            AdminPageViewModel _adminViewModel = new AdminPageViewModel(this.Window, 
                                                                        closeWindowCommand,
                                                                        userTab,
                                                                        customerTab,
                                                                        roomTab);

            this.ViewModel = _adminViewModel;
            DataContext = _adminViewModel;
            _roomTypeViewModel = roomTypeViewModel;
            _amenitiesCollection = amenitiesViewModel.ModelList;
        }

        private void Button_OpenUserWindow(object sender, RoutedEventArgs e)
        {
            UserWindow userWindow = new UserWindow(_context);
            userWindow.Owner = this.Window;
            userWindow.ShowDialog();

        }

        private void Button_ChangeVisibilityCustomer(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.UserAccount.IsAdmin)
            {
                if(CustomerCard.Visibility == Visibility.Visible)
                {
                    CustomerCard.Visibility = Visibility.Collapsed;
                    CustomerForm.Visibility = Visibility.Visible;
                    EditCustomer.Content = "Close";
                }
                else
                {
                    CustomerCard.Visibility = Visibility.Visible;
                    CustomerForm.Visibility = Visibility.Collapsed;
                    EditCustomer.Content = "Edit";
                }
            }
        }

        private void Button_ChangeVisibilityUser(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.UserAccount.IsAdmin)
            {
                if (UserCard.Visibility == Visibility.Visible)
                {
                    UserCard.Visibility = Visibility.Collapsed;
                    UserForm.Visibility = Visibility.Visible;
                    EditButton.Content = "Close";
                }
                else
                {
                    UserCard.Visibility = Visibility.Visible;
                    UserForm.Visibility = Visibility.Collapsed;
                    EditButton.Content = "Edit";
                }
            }
        }

        private void Button_OpenCustomerWindow(object sender, RoutedEventArgs e)
        {
            CustomerWindow customerWindow = new CustomerWindow(_context);
            customerWindow.Owner = this.Window;
            customerWindow.ShowDialog();
        }

        private void EditAmenitiesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.UserAccount.IsAdmin)
            {
                if (AmenityCard.Visibility == Visibility.Visible)
                {
                    AmenityCard.Visibility = Visibility.Collapsed;
                    AmenityForm.Visibility = Visibility.Visible;
                    EditAmenitiesButton.Content = "Close";
                }
                else
                {
                    AmenityCard.Visibility = Visibility.Visible;
                    AmenityForm.Visibility = Visibility.Collapsed;
                    EditAmenitiesButton.Content = "Edit";
                }
            }
        }

        private void Button_OpenAmenityWindow(object sender, RoutedEventArgs e)
        {
            AmenityWindow amenityWindow = new AmenityWindow(_context);
            amenityWindow.Owner = this.Window;
            amenityWindow.ShowDialog();
        }

        private void Button_OpenRoomTypeWindow(object sender, RoutedEventArgs e)
        {
            RoomTypeWindow roomTypeWindow = new RoomTypeWindow(_roomTypeViewModel, _amenitiesCollection);
            roomTypeWindow.Owner = this.Window;
            roomTypeWindow.ShowDialog();
        }

        private void EditRoomTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.UserAccount.IsAdmin)
            {
                if (RoomTypeCard.Visibility == Visibility.Visible)
                {
                    RoomTypeCard.Visibility = Visibility.Collapsed;
                    RoomTypeForm.Visibility = Visibility.Visible;
                    EditRoomTypeButton.Content = "Close";
                }
                else
                {
                    RoomTypeCard.Visibility = Visibility.Visible;
                    RoomTypeForm.Visibility = Visibility.Collapsed;
                    EditRoomTypeButton.Content = "Edit";
                }
            }
        }
    }
}
