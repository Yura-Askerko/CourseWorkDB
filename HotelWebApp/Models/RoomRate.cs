using System;
using System.Collections.Generic;

#nullable disable

namespace HotelWebApp.Models
{
    public partial class RoomRate
    {
        public RoomRate()
        {
            Rooms = new HashSet<Room>();
        }
        public int Id { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? Date { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
