using System;
using System.Collections.Generic;

#nullable disable

namespace HotelWebApp.Models
{
    public partial class Room
    {
        public Room()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public int? Capacity { get; set; }
        public string Specification { get; set; }
        public int RoomRateId { get; set; }

        public virtual RoomRate RoomRate { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
