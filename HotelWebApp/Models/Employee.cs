using System;
using System.Collections.Generic;

namespace HotelWebApp.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Orders = new HashSet<Order>();
            Services = new HashSet<Service>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Middlename { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
