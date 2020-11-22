using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticket2U.API.Data;
using Ticket2U.API.Models;

namespace Ticket2U.API.Repositories
{
    public class TicketRepository
    {
        private readonly DataContext _context;
        public TicketRepository(DataContext context)
        {
            _context = context;
        }

        
        public async Task BuyTicket(List<Ticket> tickets)
        {

            tickets.ForEach( tkt => _context.Tickets.AddAsync(tkt));
            await _context.SaveChangesAsync();
        } 

        public async Task RequestCashback(Cashback cashback)
        {
            await _context.Cashbacks.AddAsync(cashback);
            await _context.SaveChangesAsync();
        } 

        public async Task ApproveCashback(Cashback cashback)
        {
            var cashbackLocal = await _context.Cashbacks.Where( c => c.CashbackId == cashback.CashbackId).FirstOrDefaultAsync();
            cashbackLocal.DateCashback = DateTime.UtcNow;
            cashbackLocal.Status = "APROVADO";
            var ticketLocal = await _context.Tickets.Where( x => x.CashbackId == cashbackLocal.CashbackId ).Include( x => x.LotCategory ).FirstOrDefaultAsync();
            var userLocal = await _context.Users.Where( x => ticketLocal.UserId == x.UserId ).FirstOrDefaultAsync();
            userLocal.Credit = userLocal.Credit + ticketLocal.LotCategory.PriceCategory;
            await _context.SaveChangesAsync();
        } 

        public async Task DenyCashback(Cashback cashback)
        {
            var cashbackLocal = await _context.Cashbacks.Where( c => c.CashbackId == cashback.CashbackId).FirstOrDefaultAsync();
            cashbackLocal.Status = "NEGADO";
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cashback>> GetUsersTicketCashback()
        {
            var cashbacks = await _context.Cashbacks.Include(x => x.Ticket).ThenInclude( x => x.User).ToListAsync();            
            return cashbacks;
        }

        public async Task<int> GetTicketsSoldByEvent(int eventId)
        {
            int query = await _context.Tickets.Where(x => x.EventId == eventId).CountAsync();
            return query;
        }
    }
}