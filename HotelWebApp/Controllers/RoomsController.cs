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
    public class RoomsController : Controller
    {
        private readonly HotelContext _context;
        private readonly CacheService _cache;
        private int pageSize = 10;
        private const string filterKey = "rooms";

        public RoomsController(HotelContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(SortState sortState = SortState.RoomsTypeAsc, int page = 1)
        {
            RoomsFilterViewModel filter = HttpContext.Session.Get<RoomsFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new RoomsFilterViewModel
                {
                    Name = string.Empty
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(Room).Name}-{page}-{sortState}-{filter.Name}";

            if (!_cache.TryGetValue(key, out RoomViewModel model))
            {
                model = new RoomViewModel();

                IQueryable<Room> rooms = GetSortedRooms(sortState, filter.Name);

                int count = rooms.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Rooms = count == 0
                    ? new List<Room>()
                    : rooms.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.RoomsFilterViewModel = filter;

                _cache.Set(key, model);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(RoomsFilterViewModel filterModel, int page)
        {
            RoomsFilterViewModel filter = HttpContext.Session.Get<RoomsFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.Name = filterModel.Name;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }

        private IQueryable<Room> GetSortedRooms(SortState sortState, string name)
        {
            IQueryable<Room> rooms = _context.Rooms.Include(x => x.RoomRate).AsQueryable();

            switch (sortState)
            {
                case SortState.RoomsTypeAsc:
                    rooms = rooms.OrderBy(x => x.Type);
                    break;
                case SortState.RoomsTypeDesc:
                    rooms = rooms.OrderByDescending(x => x.Type);
                    break;
                case SortState.RoomsCapacityAsc:
                    rooms = rooms.OrderBy(x => x.Capacity);
                    break;
                case SortState.RoomsCapacityDesc:
                    rooms = rooms.OrderByDescending(x => x.Capacity);
                    break;
                case SortState.RoomsSpecificationAsc:
                    rooms = rooms.OrderBy(x => x.Specification);
                    break;
                case SortState.RoomsSpecificationDesc:
                    rooms = rooms.OrderByDescending(x => x.Specification);
                    break;
            }

            if (!string.IsNullOrEmpty(name))
            {
                rooms = rooms.Where(x => x.Type.Contains(name)).AsQueryable();
            }

            return rooms;
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.Include(x => x.RoomRate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Capacity,Specification,RoomRateId")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Capacity,Specification,RoomRateId")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
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
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(x => x.RoomRate).FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
