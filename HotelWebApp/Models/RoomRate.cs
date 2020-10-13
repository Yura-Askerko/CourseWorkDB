using System;
using System.Collections.Generic;

namespace HotelWebApp.Models
{
    public partial class RoomRate
    {
        public int Id { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? Date { get; set; }
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }
    }
}
