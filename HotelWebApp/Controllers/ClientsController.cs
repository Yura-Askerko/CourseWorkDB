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
    public class ClientsController : Controller
    {
        private readonly HotelContext _context;
        private readonly CacheService _cache;
        private int pageSize = 10;
        private const string filterKey = "clients";

        public ClientsController(HotelContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Clients
        public async Task<IActionResult> Index(SortState sortState = SortState.ClientsNameAsc, int page = 1)
        {
            ClientsFilterViewModel filter = HttpContext.Session.Get<ClientsFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new ClientsFilterViewModel
                {
                    Name = string.Empty
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(Client).Name}-{page}-{sortState}-{filter.Name}";

            if (!_cache.TryGetValue(key, out ClientViewModel model))
            {
                model = new ClientViewModel();

                IQueryable<Client> clients = GetSortedClients(sortState, filter.Name);

                int count = clients.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Clients = count == 0
                    ? new List<Client>()
                    : clients.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.ClientsFilterViewModel = filter;

                _cache.Set(key, model);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ClientsFilterViewModel filterModel, int page)
        {
            ClientsFilterViewModel filter = HttpContext.Session.Get<ClientsFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.Name = filterModel.Name;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }
        private IQueryable<Client> GetSortedClients(SortState sortState, string name)
        {
            IQueryable<Client> clients = _context.Clients.AsQueryable();

            switch (sortState)
            {
                case SortState.ClientsNameAsc:
                    clients = clients.OrderBy(x => x.FullName);
                    break;
                case SortState.ClientsNameDesc:
                    clients = clients.OrderByDescending(x => x.FullName);
                    break;
                case SortState.ClientNameOfRoomAsc:
                    clients = clients.OrderBy(x => x.NameOfRoom);
                    break;
                case SortState.ClientNameOfRoomDesc:
                    clients = clients.OrderByDescending(x => x.NameOfRoom);
                    break;
                case SortState.ClientCostAsc:
                    clients = clients.OrderBy(x => x.TotalCost);
                    break;
                case SortState.ClientCostDesc:
                    clients = clients.OrderByDescending(x => x.TotalCost);
                    break;
                case SortState.ClientPassportAsc:
                    clients = clients.OrderBy(x => x.PassportData);
                    break;
                case SortState.ClientPassportDesc:
                    clients = clients.OrderByDescending(x => x.PassportData);
                    break;
                case SortState.ClientListAsc:
                    clients = clients.OrderBy(x => x.List);
                    break;
                case SortState.ClientListDesc:
                    clients = clients.OrderByDescending(x => x.List);
                    break;
            }

            if (!string.IsNullOrEmpty(name))
            {
                clients = clients.Where(x => x.FullName.Contains(name)).AsQueryable();
            }

            return clients;
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,PassportData,NameOfRoom,List,TotalCost")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,PassportData,NameOfRoom,List,TotalCost")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
