using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace HotelWebApp.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        [Display(Name = "Check In Date")]
        public DateTime? CheckInDate { get; set; }
        [Display(Name = "Check Out Date")]
        public DateTime? CheckOut { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int RoomId { get; set; }

        public virtual Client Client { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Room Room { get; set; }
    }
}
