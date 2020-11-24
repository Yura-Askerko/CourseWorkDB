using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace HotelWebApp.Models
{
    public partial class Service
    {
        public int Id { get; set; }
        public decimal? Cost { get; set; }
        public int ServiceTypeId { get; set; }
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
        [Display(Name="Service type")]
        public virtual ServiceType ServiceType { get; set; }
    }
}
