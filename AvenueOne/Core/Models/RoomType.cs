﻿using AvenueOne.Core.Models.Interfaces;
using AvenueOne.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AvenueOne.Core.Models
{
    public class RoomType : BaseObservableModel<RoomType>, IRoomType
    {
        #region Properties
            private string _name;
            [Required(ErrorMessage ="Name is required.")]
            public string Name
            {
                get { return _name; }
                set { _name = value;
                    OnPropertyChanged();
                }
            }

            private string _details;
            public string Details
            {
                get { return _details; }
                set { _details = value;
                    OnPropertyChanged();
                }
            }

            private decimal _rate;
            [Required(ErrorMessage ="Rate is required.")]
            //[RegularExpression(@"^([0-9,.])*$", ErrorMessage = "invalid format.")]
            public decimal Rate
            {
                get { return _rate; }
                set
                {
                    _rate = value;
                    OnPropertyChanged();
                }
            }

            private RateType _rateType;
            public RateType RateType
            {
                get { return _rateType; }
                set
                {
                    _rateType = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(RateType));
                OnPropertyChanged("RateTypeAsInt");
            }
            }

        public int RateTypeAsInt { get { return (int)RateType; } }
        public int[] RateTypeValues { get { return (int[])Enum.GetValues(typeof(RateType)); } }
        #endregion

        #region  Reference
        public IList<Amenities> Amenities { get; set; }
        public IList<Room> Rooms { get; set; }
        #endregion

        #region Constructors
            public RoomType()
            :base()
            {
                this.Amenities = new List<Amenities>();
                //this.Id = GenerateId();
            }
            public RoomType(string name)
            :this()
            {
                this.Name = name;
            }


            public RoomType(string name, IList<Amenities> amenities)
                :this(name)
            {
                this.Amenities = amenities;
            }

        public RoomType(string name, decimal rate, RateType rateType)
            :this()
        {
            this.Name = name;
            this.Rate = rate;
            this.RateType = rateType;
        }
        #endregion

        #region Overrides
            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;

                if (!(obj is RoomType))
                    return false;

                RoomType roomType = (RoomType)obj;

            if (!String.IsNullOrWhiteSpace(this.Name) && !String.IsNullOrWhiteSpace(roomType.Name))
                return this.Name.ToLower() == roomType.Name.ToLower();
            if (String.IsNullOrWhiteSpace(this.Name) && String.IsNullOrWhiteSpace(roomType.Name))
                return true;
            return false;

            //return this.Id == roomType.Id &&
            //            this.Name == roomType.Name &&
            //            this.Details == roomType.Details &&
            //            this.Rate == roomType.Rate &&
            //            this.RateType == roomType.RateType;
        }

            public override int GetHashCode()
            {
            return
                this.Id.GetHashCode();
            }

        public override string ToString()
        {
            return this.Name.ToString();
        }
        #endregion
    }
}
