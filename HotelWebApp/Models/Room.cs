using System;
using System.Collections.Generic;

namespace HotelWebApp.Models
{
    public partial class Room
    {
        public Room()
        {
            Orders = new HashSet<Order>();
            RoomRates = new HashSet<RoomRate>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public int? Capacity { get; set; }
        public string Specification { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<RoomRate> RoomRates { get; set; }
    }
}
