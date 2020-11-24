using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApp.ViewModels
{
    public enum SortState
    {
        //CLients
        ClientsNameAsc,
        ClientsNameDesc,
        ClientPassportAsc,
        ClientPassportDesc,
        ClientNameOfRoomAsc,
        ClientNameOfRoomDesc,
        ClientListAsc,
        ClientListDesc, 
        ClientCostAsc,
        ClientCostDesc,

        //Orders
        OrdersCheckInAsc,
        OrdersCheckInDesc,
        OrdersCheckOutAsc,
        OrdersCheckOutDesc,
        OrdersEmpNameAsc,
        OrdersEmpNameDesc,
        OrdersClientAsc,
        OrdersClientDesc,
        OrdersRoomAsc,
        OrdersRoomDesc,

        //Rooms
        RoomsTypeAsc,
        RoomsTypeDesc,
        RoomsCapacityAsc,
        RoomsCapacityDesc,
        RoomsSpecificationAsc,
        RoomsSpecificationDesc,

        //Employees
        EmployeeNameAsc,
        EmployeeNameDesc,
        EmployeePosAsc,
        EmployeePosDesc,

        //RoomRates
        RrCostAsc,
        RrCostDesc,
        RrDateAsc,
        RrDateDesc,
        RrRoomAsc,
        RrRoomDesc,

        //ServiceTypes
        StNameAsc,
        StNameDesc,
        StSpecAsc,
        StSpecDesc,

        //Services
        ServiceCostAsc,
        ServiceCostDesc,
        ServiceTypeAsc,
        ServiceTypeDesc,
        ServiceEmpAsc,
        ServiceEmpDesc
    }

    public class SortViewModel
    {
        public SortState CurrentState { get; set; }
        //Clients
        public SortState ClientsName { get; set; }
        public SortState ClientPassport { get; set; }
        public SortState ClientNameOfRoom { get; set; }
        public SortState ClientList { get; set; }
        public SortState ClientCost { get; set; }

        //Orders

        public SortState OrdersCheckIn { get; set; }
        public SortState OrdersCheckOut { get; set; }
        public SortState OrdersEmpName { get; set; }
        public SortState OrdersClient { get; set; }
        public SortState OrdersRoom { get; set; }

        //Rooms

        public SortState RoomsType { get; set; }
        public SortState RoomsCapacity { get; set; }
        public SortState RoomsSpecification { get; set; }

        //Employees
        public SortState EmployeeName { get; set; }
        public SortState EmployeePos { get; set; }

        //RoomRates
        public SortState RrCost { get; set; }
        public SortState RrDate { get; set; }
        public SortState RrRoom { get; set; }

        //ServiceTypes
        public SortState StName { get; set; }
        public SortState StSpec { get; set; }

        //Services
        public SortState ServiceCost { get; set; }
        public SortState ServiceType { get; set; }
        public SortState ServiceEmp { get; set; }

        public SortViewModel(SortState state)
        {
            ClientsName = state == SortState.ClientsNameAsc ? SortState.ClientsNameDesc : SortState.ClientsNameAsc;
            ClientPassport = state == SortState.ClientPassportAsc ? SortState.ClientPassportDesc : SortState.ClientPassportAsc;
            ClientNameOfRoom = state == SortState.ClientNameOfRoomAsc ? SortState.ClientNameOfRoomDesc : SortState.ClientNameOfRoomAsc;
            ClientList = state == SortState.ClientListAsc ? SortState.ClientListDesc : SortState.ClientListAsc;
            ClientCost = state == SortState.ClientCostAsc ? SortState.ClientCostDesc : SortState.ClientCostAsc;

            OrdersCheckIn = state == SortState.OrdersCheckInAsc ? SortState.OrdersCheckInDesc : SortState.OrdersCheckInAsc;
            OrdersCheckOut = state == SortState.OrdersCheckOutAsc ? SortState.OrdersCheckOutDesc : SortState.OrdersCheckOutAsc;
            OrdersEmpName = state == SortState.OrdersEmpNameAsc ? SortState.OrdersEmpNameDesc : SortState.OrdersEmpNameAsc;
            OrdersClient = state == SortState.OrdersClientAsc ? SortState.OrdersClientDesc : SortState.OrdersClientAsc;
            OrdersRoom = state == SortState.OrdersRoomAsc ? SortState.OrdersRoomDesc : SortState.OrdersRoomAsc;


            RoomsType = state == SortState.RoomsTypeAsc ? SortState.RoomsTypeDesc : SortState.RoomsTypeAsc;
            RoomsCapacity = state == SortState.RoomsCapacityAsc ? SortState.RoomsCapacityDesc : SortState.RoomsCapacityAsc;
            RoomsSpecification = state == SortState.RoomsSpecificationAsc ? SortState.RoomsSpecificationDesc : SortState.RoomsSpecificationAsc;

            EmployeeName = state == SortState.EmployeeNameAsc ? SortState.EmployeeNameDesc : SortState.EmployeeNameAsc;
            EmployeePos = state == SortState.EmployeePosAsc ? SortState.EmployeePosDesc : SortState.EmployeePosAsc;

            RrCost = state == SortState.RrCostAsc ? SortState.RrCostDesc : SortState.RrCostAsc;
            RrDate = state == SortState.RrDateAsc ? SortState.RrDateDesc : SortState.RrDateAsc;
            RrRoom = state == SortState.RrRoomAsc ? SortState.RrRoomDesc : SortState.RrRoomAsc;

            StName = state == SortState.StNameAsc ? SortState.StNameDesc : SortState.StNameAsc;
            StSpec = state == SortState.StSpecAsc ? SortState.StSpecDesc : SortState.StSpecAsc;

            ServiceCost = state == SortState.ServiceCostAsc ? SortState.ServiceCostDesc : SortState.ServiceCostAsc;
            ServiceType = state == SortState.ServiceTypeAsc ? SortState.ServiceTypeDesc : SortState.ServiceTypeAsc;
            ServiceEmp = state == SortState.ServiceEmpAsc ? SortState.ServiceEmpDesc : SortState.ServiceEmpAsc;

            CurrentState = state;
        }
    }
}
