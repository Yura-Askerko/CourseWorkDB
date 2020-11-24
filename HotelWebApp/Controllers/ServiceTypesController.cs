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
    public class ServicetypesController : Controller
    {
        private readonly HotelContext _context;
        private readonly CacheService _cache;
        private int pageSize = 10;
        private const string filterKey = "serviceTypes";

        public ServicetypesController(HotelContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: ServiceTypes
        public async Task<IActionResult> Index(SortState sortState = SortState.StNameAsc, int page = 1)
        {
            ServiceTypesFilterViewModel filter = HttpContext.Session.Get<ServiceTypesFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new ServiceTypesFilterViewModel
                {
                    Name = string.Empty
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(ServiceType).Name}-{page}-{sortState}-{filter.Name}";

            if (!_cache.TryGetValue(key, out ServiceTypeViewModel model))
            {
                model = new ServiceTypeViewModel();

                IQueryable<ServiceType> serviceTypes = GetSortedServiceTypes(sortState, filter.Name);

                int count = serviceTypes.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.ServiceTypes = count == 0
                    ? new List<ServiceType>()
                    : serviceTypes.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.ServiceTypesFilterViewModel = filter;

                _cache.Set(key, model);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ServiceTypesFilterViewModel filterModel, int page)
        {
            ServiceTypesFilterViewModel filter = HttpContext.Session.Get<ServiceTypesFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.Name = filterModel.Name;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }

        private IQueryable<ServiceType> GetSortedServiceTypes(SortState sortState, string name)
        {
            IQueryable<ServiceType> serviceTypes = _context.ServiceTypes.AsQueryable();

            switch (sortState)
            {
                case SortState.StNameAsc:
                    serviceTypes = serviceTypes.OrderByDescending(x => x.Name);
                    break;
                case SortState.StNameDesc:
                    serviceTypes = serviceTypes.OrderBy(x => x.Name);
                    break;
                case SortState.StSpecAsc:
                    serviceTypes = serviceTypes.OrderByDescending(x => x.Specificaion);
                    break;
                case SortState.StSpecDesc:
                    serviceTypes = serviceTypes.OrderBy(x => x.Specificaion);
                    break;
            }

            if (!string.IsNullOrEmpty(name))
            {
                serviceTypes = serviceTypes.Where(x => x.Name.Contains(name)).AsQueryable();
            }

            return serviceTypes;
        }

        // GET: ServiceTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicetype = await _context.ServiceTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (servicetype == null)
            {
                return NotFound();
            }

            return View(servicetype);
        }

        // GET: ServiceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Specificaion")] ServiceType servicetype)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servicetype);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            return View(servicetype);
        }

        // GET: ServiceTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicetype = await _context.ServiceTypes.FindAsync(id);
            if (servicetype == null)
            {
                return NotFound();
            }
            return View(servicetype);
        }

        // POST: ServiceTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Specificaion")] ServiceType servicetype)
        {
            if (id != servicetype.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servicetype);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicetypeExists(servicetype.Id))
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
            return View(servicetype);
        }

        // GET: ServiceTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicetype = await _context.ServiceTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (servicetype == null)
            {
                return NotFound();
            }

            return View(servicetype);
        }

        // POST: ServiceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicetype = await _context.ServiceTypes.FindAsync(id);
            _context.ServiceTypes.Remove(servicetype);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicetypeExists(int id)
        {
            return _context.ServiceTypes.Any(e => e.Id == id);
        }
    }
}
