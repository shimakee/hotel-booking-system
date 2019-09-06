﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvenueOne.Interfaces
{
    public enum GenderType : byte
    {
        Male,
        Female
    }

    public enum CivilStatusType : byte
    {
        Single,
        Married,
        Widowed,
        Annuled,
        Divorced
    }

    public interface IPerson
    {
        string Id { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string MaidenName { get; set; }
        GenderType Gender { get; set; }
        CivilStatusType CivilStatus { get; set; }
        string Nationality { get; set; }
        DateTime? BirthDate { get; set; }

        string FullName { get; set; }
    }
}