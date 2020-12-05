using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Serialization;


namespace Customers.Models
{
    public class UserSys
    {
        private ILazyLoader LazyLoader { get; set; }
        private UserRole _userRole;

        public UserSys(ILazyLoader lazyLoader){
            LazyLoader = lazyLoader;
        }
        public int Id {get ; set; }
        public string Login {get ; set; }
        public string Email {get ; set; }
        public string Password {get ; set; }
        public UserRole UserRole {get => LazyLoader.Load(this, ref _userRole) ; set => _userRole = value; }

        [JsonConstructor]
        public UserSys(int Id, string Login, string Email, string Password, UserRole UserRole){
            this.Id = Id;
            this.Login = Login;
            this.Email = Email;
            this.Password = Password;
            this.UserRole = UserRole;
        }
    }
}