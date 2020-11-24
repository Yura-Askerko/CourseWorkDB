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
    public class EmployeesController : Controller
    {
        private readonly HotelContext _context;
        private readonly CacheService _cache;
        private int pageSize = 10;
        private const string filterKey = "employees";

        public EmployeesController(HotelContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Employees
        public async Task<IActionResult> Index(SortState sortState = SortState.EmployeeNameAsc, int page = 1)
        {
            EmployeesFilterViewModel filter = HttpContext.Session.Get<EmployeesFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new EmployeesFilterViewModel
                {
                    Name = string.Empty,
                    Position = string.Empty
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(Employee).Name}-{page}-{sortState}-{filter.Name}-{filter.Position}";

            if (!_cache.TryGetValue(key, out EmployeeViewModel model))
            {
                model = new EmployeeViewModel();

                IQueryable<Employee> employees = GetSortedEmp(sortState, filter.Name, filter.Position);

                int count = employees.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Employees = count == 0
                    ? new List<Employee>()
                    : employees.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.EmployeesFilterViewModel = filter;

                _cache.Set(key, model);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(EmployeesFilterViewModel filterModel, int page)
        {
            EmployeesFilterViewModel filter = HttpContext.Session.Get<EmployeesFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.Name = filterModel.Name;
                filter.Position = filterModel.Position;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }
        private IQueryable<Employee> GetSortedEmp(SortState sortState, string name, string position)
        {
            IQueryable<Employee> employees = _context.Employees.AsQueryable();

            switch (sortState)
            {
                case SortState.EmployeeNameAsc:
                    employees = employees.OrderBy(x => x.FullName);
                    break;
                case SortState.EmployeeNameDesc:
                    employees = employees.OrderByDescending(x => x.FullName);
                    break;
                case SortState.EmployeePosAsc:
                    employees = employees.OrderBy(x => x.Position);
                    break;
                case SortState.EmployeePosDesc:
                    employees = employees.OrderByDescending(x => x.Position);
                    break;
            }

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(position))
            {
                employees = employees.Where(x => x.FullName.Contains(name) && x.Position.Contains(position)).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(name))
            {
                employees = employees.Where(x => x.FullName.Contains(name)).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(position))
            {
                employees = employees.Where(x => x.Position.Contains(position)).AsQueryable();
            }

            return employees;
        }
        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Position")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Position")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
