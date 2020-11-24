using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelWebApp.Models;
using HotelWebApp.ViewModels.FilterViewModels;

namespace HotelWebApp.ViewModels.Models
{
    public class RoomRateViewModel
    {
        public IEnumerable<RoomRate> RoomRates { get; set; }

        public RoomRate RoomRate { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public RoomRatesFilterViewModel RoomRatesFilterViewModel { get; set; }
    }
}
