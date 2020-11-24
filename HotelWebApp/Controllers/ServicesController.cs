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
    public class ServicesController : Controller
    {
        private readonly HotelContext _context;
        private readonly CacheService _cache;
        private int pageSize = 15;
        private const string filterKey = "services";

        public ServicesController(HotelContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Services
        public async Task<IActionResult> Index(SortState sortState = SortState.ServiceCostAsc, int page = 1)
        {
            ServicesFilterViewModel filter = HttpContext.Session.Get<ServicesFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new ServicesFilterViewModel
                {
                    Name = string.Empty
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(Service).Name}-{page}-{sortState}-{filter.Name}";

            if (!_cache.TryGetValue(key, out ServiceViewModel model))
            {
                model = new ServiceViewModel();

                IQueryable<Service> services = getSortedServices(sortState, filter.Name);

                int count = services.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Services = count == 0 ? new List<Service>() : services.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.ServicesFilterViewModel = filter;

                _cache.Set(key, model);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ServicesFilterViewModel filterModel, int page)
        {
            ServicesFilterViewModel filter = HttpContext.Session.Get<ServicesFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.Name = filterModel.Name;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }
        private IQueryable<Service> getSortedServices(SortState sortState, string name)
        {
            IQueryable<Service> services =
                _context.Services.Include(p => p.Employee).Include(p => p.ServiceType).AsQueryable();

            switch (sortState)
            {
                case SortState.ServiceTypeAsc:
                    services = services.OrderBy(x => x.ServiceType.Name);
                    break;
                case SortState.ServiceTypeDesc:
                    services = services.OrderByDescending(x => x.ServiceType.Name);
                    break;
                case SortState.ServiceEmpAsc:
                    services = services.OrderBy(x => x.Employee.FullName);
                    break;
                case SortState.ServiceEmpDesc:
                    services = services.OrderByDescending(x => x.Employee.FullName);
                    break;
                case SortState.ServiceCostAsc:
                    services = services.OrderBy(x => x.Cost);
                    break;
                case SortState.ServiceCostDesc:
                    services = services.OrderByDescending(x => x.Cost);
                    break;
            }

            if (!string.IsNullOrEmpty(name))
            {
                services = services.Where(x => (x.ServiceType.Name).Contains(name))
                    .AsQueryable();
            }

            return services;
        }
        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Employee)
                .Include(s => s.ServiceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "Id", "Name");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cost,ServiceTypeId,EmployeeId")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", service.EmployeeId);
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "Id", "Id", service.ServiceTypeId);
            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", service.EmployeeId);
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "Id", "Name", service.ServiceTypeId);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cost,ServiceTypeId,EmployeeId")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", service.EmployeeId);
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "Id", "Id", service.ServiceTypeId);
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Employee)
                .Include(s => s.ServiceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
