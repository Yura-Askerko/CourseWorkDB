using System;
using System.Collections.Generic;

namespace HotelWebApp.Models
{
    public partial class Service
    {
        public int Id { get; set; }
        public decimal? Cost { get; set; }
        public int ServicetypeId { get; set; }
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Servicetype Servicetype { get; set; }
    }
}
