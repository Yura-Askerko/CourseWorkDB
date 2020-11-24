using System.Collections.Generic;
using HotelWebApp.Models;
using HotelWebApp.ViewModels.FilterViewModels;

namespace HotelWebApp.ViewModels.Models
{
    public class OrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }

        public Order Order { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public OrdersFilterViewModel OrdersFilterViewModel { get; set; }
    }
}
