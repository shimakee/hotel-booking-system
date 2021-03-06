﻿using AvenueOne.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvenueOne.Interfaces.RepositoryInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IPersonRepository People { get; }
        ICustomerRepository Customers { get; }
        IAmenitiesRepository Amenities { get; }
        IRoomTypeRepository RoomType {get;}
        IRoomRepository Rooms { get;  }
        Task<int> CompleteAsync();
        int Complete();
    }
}
