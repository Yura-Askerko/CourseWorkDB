using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelWebApp.Models;
using HotelWebApp.ViewModels.FilterViewModels;

namespace HotelWebApp.ViewModels.Models
{
    public class EmployeeViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }

        public Employee Employee { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public EmployeesFilterViewModel EmployeesFilterViewModel { get; set; }
    }
}
