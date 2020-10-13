using System;
using System.Collections.Generic;

namespace HotelWebApp.Models
{
    public partial class Info
    {
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOut { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Middlename { get; set; }
        public string Surname { get; set; }
        public string PassportData { get; set; }
        public string NameOfRoom { get; set; }
        public string List { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
