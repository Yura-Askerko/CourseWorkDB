using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelWebApp.Models;
using HotelWebApp.ViewModels.FilterViewModels;

namespace HotelWebApp.ViewModels.Models
{
    public class ServiceTypeViewModel
    {
        public IEnumerable<ServiceType> ServiceTypes { get; set; }

        public ServiceType ServiceType { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public ServiceTypesFilterViewModel ServiceTypesFilterViewModel { get; set; }
    }
}
