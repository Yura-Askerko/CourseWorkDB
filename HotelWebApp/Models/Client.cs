using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace HotelWebApp.Models
{
    public partial class Client
    {
        public Client()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Passport data")]
        public string PassportData { get; set; }
        [Display(Name = "Name of room")]
        public string NameOfRoom { get; set; }
        public string List { get; set; }
        [Display(Name = "Total cost")]
        public decimal? TotalCost { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
