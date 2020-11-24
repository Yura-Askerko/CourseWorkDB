using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelWebApp.Data;
using HotelWebApp.Infrastructure;
using HotelWebApp.Models;
using HotelWebApp.Services;
using HotelWebApp.ViewModels;
using HotelWebApp.ViewModels.FilterViewModels;
using HotelWebApp.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelWebApp.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly HotelContext _context;
        private readonly CacheService _cache;
        private int pageSize = 15;
        private const string filterKey = "orders";

        public OrdersController(HotelContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Orders
        public async Task<IActionResult> Index(SortState sortState = SortState.OrdersCheckInAsc, int page = 1)
        {
            OrdersFilterViewModel filter = HttpContext.Session.Get<OrdersFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new OrdersFilterViewModel
                {
                    Name = string.Empty,
                    Capacity = null,
                    ToDate = null,
                    FromDate = null,
                    Type = string.Empty
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(Order).Name}-{page}-{sortState}-{filter.Name}-{filter.Capacity}-{filter.FromDate}-{filter.ToDate}-{filter.Type}";

            if (!_cache.TryGetValue(key, out OrderViewModel model))
            {
                model = new OrderViewModel();

                IQueryable<Order> orders = GetSortedOrders(sortState, filter.Name, filter.Capacity, filter.FromDate, filter.ToDate, filter.Type);

                int count = orders.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Orders = count == 0 ? new List<Order>() : orders.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.OrdersFilterViewModel = filter;

                _cache.Set(key, model);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(OrdersFilterViewModel filterModel, int page)
        {
            OrdersFilterViewModel filter = HttpContext.Session.Get<OrdersFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.Name = filterModel.Name;
                filter.FromDate = filterModel.FromDate;
                filter.ToDate = filterModel.ToDate;
                filter.Type = filterModel.Type;
                filter.Capacity = filterModel.Capacity;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }

        private IQueryable<Order> GetSortedOrders(SortState sortState, string name, int? capacity, DateTime? fromDate, DateTime? toDate, string type)
        {
            IQueryable<Order> orders = 
                _context.Orders.Include(p => p.Client)
                .Include(p => p.Employee).Include(p => p.Room).ThenInclude(x => x.RoomRate).AsQueryable();

            switch (sortState)
            {
                case SortState.OrdersCheckInAsc:
                    orders = orders.OrderBy(x => x.CheckInDate);
                    break;
                case SortState.OrdersCheckInDesc:
                    orders = orders.OrderByDescending(x => x.CheckInDate);
                    break;
                case SortState.OrdersCheckOutAsc:
                    orders = orders.OrderBy(x => x.CheckOut);
                    break;
                case SortState.OrdersCheckOutDesc:
                    orders = orders.OrderByDescending(x => x.CheckOut);
                    break;
                case SortState.OrdersEmpNameAsc:
                    orders = orders.OrderBy(x => x.Employee.FullName);
                    break;
                case SortState.OrdersEmpNameDesc:
                    orders = orders.OrderByDescending(x => x.Employee.FullName);
                    break;
                case SortState.OrdersClientAsc:
                    orders = orders.OrderBy(x => x.Client.FullName);
                    break;
                case SortState.OrdersClientDesc:
                    orders = orders.OrderByDescending(x => x.Client.FullName);
                    break;
                case SortState.OrdersRoomAsc:
                    orders = orders.OrderBy(x => x.Room.Type);
                    break;
                case SortState.OrdersRoomDesc:
                    orders = orders.OrderByDescending(x => x.Room.Type);
                    break;
            }

            if (!string.IsNullOrEmpty(name))
            {
                orders = orders.Where(x => (x.Client.FullName).Contains(name))
                    .AsQueryable();
            }
            if (!string.IsNullOrEmpty(type))
            {
                orders = orders.Where(x => (x.Room.Type).Contains(type))
                    .AsQueryable();
            }
            if (fromDate != null)
            {
                orders = orders.Where(x => x.CheckOut > fromDate)
                    .AsQueryable();
            }
            if (toDate != null)
            {
                orders = orders.Where(x => x.CheckOut < toDate)
                    .AsQueryable();
            }
            if (capacity != null)
            {
                orders = orders.Where(x => x.Room.Capacity == capacity)
                    .AsQueryable();
            }

            return orders;
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Employee)
                .Include(o => o.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName");
            ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "Id", "FullName");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Type");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CheckInDate,CheckOut,EmployeeId,ClientId,RoomId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", order.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "Id", "Id", order.EmployeeId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", order.RoomId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", order.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "Id", "FullName", order.EmployeeId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "Id", "Type", order.RoomId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CheckInDate,CheckOut,EmployeeId,ClientId,RoomId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", order.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "Id", "Id", order.EmployeeId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "Id", "Id", order.RoomId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Employee)
                .Include(o => o.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
