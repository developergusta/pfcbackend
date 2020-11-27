using System.Collections.Generic;
using System.Linq;
using Ticket2U.API.Data;
using Ticket2U.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace Ticket2U.API.Repositories
{
    public class EventRepository
    {
        private readonly DataContext _context;
        public EventRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateEvent(Event eventObj)
        {
            try
            {
                _context.Events.Add(eventObj);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");
                return false;
            }
        }
        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            return await _context.Events
            .Include(x => x.Address)
            .Include(x => x.Images)
            .Include(x => x.Lots)
            .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetApprovedEvents()
        {
            return await _context.Events
                .Where(x => x.Status == "APROVADO")
                .Include(x => x.Address)
                .Include(x => x.Images)
                .Include(x => x.Lots)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetPendingEvents()
        {
            return await _context.Events
                .Where(x => x.Status == "PENDENTE")
                .Include(x => x.Address)
                .Include(x => x.Images)
                .Include(x => x.Lots)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetDeniedEvents()
        {
            return await _context.Events
                .Where(x => x.Status == "NEGADO")
                .Include(x => x.Address)
                .Include(x => x.Images)
                .Include(x => x.Lots)
                .ToListAsync();
        }

        public async Task<Event> GetEvent(int id)
        {
            return await _context.Events
                .Include(x => x.Address)
                .Include(x => x.Images)
                .Include(x => x.Tickets)
                .Include(x => x.Lots)
                .ThenInclude(x => x.LotCategories)
                .FirstOrDefaultAsync(x => x.EventId == id);
        }

        public async Task DeleteLot(Lot lot)
        {
            _context.Lots.Remove(lot);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLotCategory(LotCategory lotCateg)
        {
            _context.LotCategories.Remove(lotCateg);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Event>> GetMostSoldEventsOnYear()
        {
            DateTime inicioAno = new DateTime(DateTime.UtcNow.Year, 1, 1);
            var eventos = await _context.Events
                .Where(x => x.DateStart >= inicioAno)
                .Include(x => x.Tickets)
                .OrderByDescending(x => x.Tickets.Count)
                .Take(5)
                .ToListAsync();

            return eventos;
        }

        public async Task<IEnumerable<Event>> GetEventByCategory(string category)
        {
            IQueryable<Event> query = _context.Events
                .Include(e => e.Lots)
                .ThenInclude(f => f.LotCategories)
                .Include(e => e.Address);

            query = query.OrderByDescending(c => c.TitleEvent)
                .Where(c => c.Category.ToLower().Contains(category.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Lot> GetLotById(int lotId)
        {
            Lot lot = await _context.Lots
                .Where(x => x.LotId == lotId)
                .FirstOrDefaultAsync();
            return lot;
        }

        public async Task<LotCategory> GetLotCategoryById(int lotCatgId)
        {
            LotCategory lotCatg = await _context.LotCategories
                .Where(x => x.LotCategoryId == lotCatgId)
                .FirstOrDefaultAsync();
            return lotCatg;
        }
        public async Task<IEnumerable<Event>> GetEventsByUserId(int userId)
        {
            IQueryable<Event> query = _context.Events
                .Where(e => e.UserId == userId)
                .Include(e => e.Address)
                .Include(e => e.Lots)
                .ThenInclude(f => f.LotCategories);

            return await query.ToArrayAsync();
        }
        public async Task UpdateEvent(Event eventObj)
        {
            try
            {
                var eventLocal = await _context.Events
                    .Where(e => e.EventId == eventObj.EventId)
                    .Include(e => e.Images)
                    .Include(e => e.Address)
                    .Include(e => e.Lots)
                    .ThenInclude(e => e.LotCategories)
                    .FirstOrDefaultAsync();

                eventLocal.TitleEvent = eventObj.TitleEvent;
                eventLocal.Description = eventObj.Description;
                eventLocal.Category = eventObj.Category;
                eventLocal.Capacity = eventObj.Capacity;
                eventLocal.DateStart = eventObj.DateStart;
                eventLocal.DateEnd = eventObj.DateEnd;

                eventLocal.Address.City = eventObj.Address.City;
                eventLocal.Address.Complement = eventObj.Address.Complement;
                eventLocal.Address.Country = eventObj.Address.Country;
                eventLocal.Address.Num = eventObj.Address.Num;
                eventLocal.Address.State = eventObj.Address.State;
                eventLocal.Address.Street = eventObj.Address.Street;
                eventLocal.Address.ZipCode = eventObj.Address.ZipCode;

                await _context.SaveChangesAsync();

                foreach (var img in eventObj.Images)
                {
                    if (img.EventId == null || img.EventId == 0)
                    {
                        img.EventId = eventLocal.EventId;
                    }
                    if (img.IdImage == 0)
                    {
                        await _context.Images.AddAsync(img);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var imgLocal = eventLocal.Images.Find(x => x.IdImage == img.IdImage);
                        imgLocal.Src = img.Src;
                        await _context.SaveChangesAsync();
                    }
                }

                foreach (var newLot in eventObj.Lots)
                {
                    if (newLot.LotId == 0)
                    {
                        newLot.EventId = eventObj.EventId;
                        if (newLot.DateStart == null)
                        {
                            newLot.DateStart = eventObj.DateStart;
                        }
                        if (newLot.DateEnd == null)
                        {
                            newLot.DateEnd = eventObj.DateEnd;
                        }
                        await _context.Lots.AddAsync(newLot);
                        await _context.SaveChangesAsync();

                        foreach (var itemCatg in newLot.LotCategories)
                        {
                            itemCatg.LotId = newLot.LotId;
                            await _context.Lots.AddAsync(newLot);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        var oldLot = eventLocal.Lots.Find(x => x.LotId == newLot.LotId);
                        oldLot.DateStart = newLot.DateStart;
                        oldLot.DateEnd = newLot.DateEnd;
                        await _context.SaveChangesAsync();

                        foreach (var lotCatg in oldLot.LotCategories)
                        {
                            if (lotCatg.LotId != 0)
                            {
                                var existLotCatg = oldLot.LotCategories.Find(y => y.LotCategoryId == lotCatg.LotCategoryId);
                                var updtLotCatg = newLot.LotCategories.Find(y => y.LotCategoryId == lotCatg.LotCategoryId);
                                existLotCatg.Desc = updtLotCatg.Desc;
                                existLotCatg.PriceCategory = updtLotCatg.PriceCategory;
                                await _context.SaveChangesAsync();
                            }
                        }

                        if (oldLot.LotCategories.Count < newLot.LotCategories.Count)
                        {
                            foreach (var lotCatg in newLot.LotCategories)
                            {
                                if (lotCatg.LotCategoryId == 0)
                                {
                                    lotCatg.LotId = oldLot.LotId;
                                    await _context.LotCategories.AddAsync(lotCatg);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");
            }
        }

        public async Task RemoveEvent(Event eventObj)
        {
            try
            {
                _context.Events.Remove(eventObj);
                _context.Addresses.Remove(eventObj.Address);
                _context.Lots.RemoveRange(eventObj.Lots);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");
            }
        }
        public async Task<IEnumerable<Address>> GetAllAddresses()
        {
            return await _context.Addresses.ToListAsync();
        }

        public void CreateAddress(Address address)
        {
            _context.Addresses.Add(address);
            _context.SaveChanges();
        }

        public async Task<Address> GetAddress(int id)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(x => x.AddressId == id);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByEvent(Event eventObj)
        {
            IQueryable<Ticket> query = _context.Tickets
                .Where(x => x.EventId == eventObj.EventId)
                .Include(x => x.User);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsToday()
        {
            DateTime now = DateTime.UtcNow;
            var result = await _context.Events
            .Where(evt => now.Month == evt.DateStart.Month && now.Year == evt.DateStart.Year && evt.DateStart.Day == now.Day)
            .Include(x => x.Images)
            .Include(x => x.Address)
            .ToListAsync();
            return result;
        }

        public async Task ApproveEvent(Event evento)
        {
            try
            {
                var eventLocal = await _context.Events
                    .Where(e => e.EventId == evento.EventId)
                    .FirstOrDefaultAsync();

                eventLocal.Status = "APROVADO";

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");
            }
        }

        public async Task DenyEvent(Event evento)
        {
            try
            {
                var eventLocal = await _context.Events
                    .Where(e => e.EventId == evento.EventId)
                    .FirstOrDefaultAsync();

                eventLocal.Status = "NEGADO";

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");
            }
        }
    }
}