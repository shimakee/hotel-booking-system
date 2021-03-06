﻿using AvenueOne.Core.IRepositories;
using AvenueOne.Models;
using AvenueOne.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvenueOne.Persistence.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }
        public CustomerRepository(PlutoContext context)
            : base(context)
        {
           
        }
    }
}
