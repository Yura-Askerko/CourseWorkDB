using System;
using System.Collections.Generic;

namespace HotelWebApp.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOut { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int RoomId { get; set; }

        public virtual Client Client { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Room Room { get; set; }
    }
}
