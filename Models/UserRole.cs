using System;
using System.ComponentModel.DataAnnotations;

namespace Customers.Models
{
    public class UserRole
    {
        public int Id {get ; set; }
        public string Name {get ; set; }
        public bool isAdmin {get ; set; }
    }

}