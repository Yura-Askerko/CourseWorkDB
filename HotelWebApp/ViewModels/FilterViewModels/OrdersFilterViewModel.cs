using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApp.ViewModels.FilterViewModels
{
    public class OrdersFilterViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        public int? Capacity { get; set; }

        public string Type { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
