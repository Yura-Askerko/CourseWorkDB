using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelWebApp.Models;
using HotelWebApp.ViewModels.FilterViewModels;

namespace HotelWebApp.ViewModels.Models
{
    public class RoomViewModel
    {
        public IEnumerable<Room> Rooms { get; set; }

        public Room Room { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public RoomsFilterViewModel RoomsFilterViewModel { get; set; }

    }
}
