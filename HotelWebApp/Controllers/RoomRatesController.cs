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
    public class RoomRatesController : Controller
    {
        private readonly HotelContext _context;
        private readonly CacheService _cache;
        private int pageSize = 10;
        private const string filterKey = "roomRates";

        public RoomRatesController(HotelContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: RoomRates
        public async Task<IActionResult> Index(SortState sortState = SortState.RrDateAsc, int page = 1)
        {
            RoomRatesFilterViewModel filter = HttpContext.Session.Get<RoomRatesFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new RoomRatesFilterViewModel
                {
                    Name = string.Empty
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(RoomRate).Name}-{page}-{sortState}-{filter.Name}";

            if (!_cache.TryGetValue(key, out RoomRateViewModel model))
            {
                model = new RoomRateViewModel();

                IQueryable<RoomRate> roomRates = GetSortedRoomRates(sortState, filter.Name);

                int count = roomRates.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.RoomRates = count == 0
                    ? new List<RoomRate>()
                    : roomRates.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.RoomRatesFilterViewModel = filter;

                _cache.Set(key, model);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(RoomRatesFilterViewModel filterModel, int page)
        {
            RoomRatesFilterViewModel filter = HttpContext.Session.Get<RoomRatesFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.Name = filterModel.Name;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }
        private IQueryable<RoomRate> GetSortedRoomRates(SortState sortState, string name)
        {
            IQueryable<RoomRate> roomRates = _context.RoomRates.AsQueryable();

            switch (sortState)
            {
                case SortState.RrCostAsc:
                    roomRates = roomRates.OrderBy(x => x.Cost);
                    break;
                case SortState.RrCostDesc:
                    roomRates = roomRates.OrderByDescending(x => x.Cost);
                    break;
                case SortState.RrDateAsc:
                    roomRates = roomRates.OrderBy(x => x.Date);
                    break;
                case SortState.RrDateDesc:
                    roomRates = roomRates.OrderByDescending(x => x.Date);
                    break;
            }


            return roomRates;
        }

        // GET: RoomRates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomRate = await _context.RoomRates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomRate == null)
            {
                return NotFound();
            }

            return View(roomRate);
        }

        // GET: RoomRates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomRates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cost,Date")] RoomRate roomRate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomRate);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            return View(roomRate);
        }

        // GET: RoomRates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomRate = await _context.RoomRates.FindAsync(id);
            if (roomRate == null)
            {
                return NotFound();
            }
            return View(roomRate);
        }

        // POST: RoomRates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cost,Date")] RoomRate roomRate)
        {
            if (id != roomRate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomRate);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomRateExists(roomRate.Id))
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
            return View(roomRate);
        }

        // GET: RoomRates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomRate = await _context.RoomRates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomRate == null)
            {
                return NotFound();
            }

            return View(roomRate);
        }

        // POST: RoomRates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomRate = await _context.RoomRates.FindAsync(id);
            _context.RoomRates.Remove(roomRate);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomRateExists(int id)
        {
            return _context.RoomRates.Any(e => e.Id == id);
        }
    }
}
