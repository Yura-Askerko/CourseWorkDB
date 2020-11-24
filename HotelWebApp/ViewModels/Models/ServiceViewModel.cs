using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelWebApp.Models;
using HotelWebApp.ViewModels.FilterViewModels;

namespace HotelWebApp.ViewModels.Models
{
    public class ServiceViewModel
    {
        public IEnumerable<Service> Services { get; set; }

        public Service Service { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public ServicesFilterViewModel ServicesFilterViewModel { get; set; }
    }
}
