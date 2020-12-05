using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Customers.Models
{
    public class Customer
    {
        private ILazyLoader LazyLoader { get; set; }
        private City _city;
        private Region _region;
        private UserSys _user;
        private Gender _gender;
        private Classification _classification;

        public Customer(ILazyLoader lazyLoader){
            LazyLoader = lazyLoader;
        }

        public int Id {get ; set; }
        public string Name {get ; set; }
        public string Phone {get ; set; }
        [DataType(DataType.Date)]
        public DateTime LastPurchase {get ; set; }
        public Gender Gender {get => LazyLoader.Load(this, ref _gender) ; set => _gender = value; }
        public City City {get => LazyLoader.Load(this, ref _city) ; set => _city = value; }
        public Region Region {get => LazyLoader.Load(this, ref _region) ; set => _region = value; }
        public UserSys User {get => LazyLoader.Load(this, ref _user) ; set => _user = value; }
        public Classification Classification {get => LazyLoader.Load(this, ref _classification) ; set => _classification = value; }
    }

}