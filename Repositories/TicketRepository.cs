using System.Collections.Generic;
using System.Threading.Tasks;
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

    }
}