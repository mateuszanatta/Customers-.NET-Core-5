using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Customers.Models.Views
{
    public class UserSysView
    {
        public int Id {get ; set; }
        public string Login {get ; set; }
        public string Email {get ; set; }
        public UserRole UserRole {get; set;}

        [JsonConstructor]
        public UserSysView(int Id, string Login, string Email, UserRole UserRole){
            this.Id = Id;
            this.Login = Login;
            this.Email = Email;
            this.UserRole = UserRole;
        }

        public UserSysView(){}
        public IList<UserSysView> UserSysViewList(IList<UserSys> userSys){
            IList<UserSysView> userSysView = new List<UserSysView>();

            foreach(UserSys user in userSys){
                UserSysView userView = new UserSysView(user.Id, user.Login, user.Email, user.UserRole);
                userSysView.Add(userView);
            }

            return userSysView;
        }
    }
}
