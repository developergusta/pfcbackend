using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticket2U.API.Data;
using Ticket2U.API.Models;

namespace Ticket2U.API.Repositories
{
    public class UserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.Include(x => x.Login).Include(x => x.Addresses).Include(x => x.Tickets).Include(x => x.Image).Include(x => x.Phones).ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.Where(x => x.UserId == id).Include(x => x.Login).Include(x => x.Addresses).Include(x => x.Tickets).Include(x => x.Image).Include(x => x.Phones).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByCpf(string CPF)
        {
            return await _context.Users.Include(x => x.Login).FirstOrDefaultAsync(x => x.Cpf == CPF);
        }

        public async Task UpdateUser(User user)
        {
            try
            {
                var userLocal = _context.Users    
                    .Where(x => x.UserId == user.UserId)
                    .Include(x => x.Addresses)
                    .Include( x => x.Phones)
                    .Include( x => x.Events )
                    .Include(x => x.Image)
                    .FirstOrDefault();

                userLocal.Name = user.Name;
                userLocal.Cpf = user.Cpf;
                userLocal.Rg = user.Rg;
                userLocal.Login.Email = user.Login.Email;
                userLocal.DateBirth = user.DateBirth;
                userLocal.Image.Src = user.Image.Src;
                
                foreach (var item in user.Addresses)
                {
                    if(item.AddressId == 0)
                    {
                        item.UserId = user.UserId;
                        await _context.Addresses.AddAsync(item);
                    } 
                    else
                    {
                        var addr = userLocal.Addresses.Find( x => x.AddressId == item.AddressId );
                        addr.City = item.City;
                        addr.Complement = item.Complement;
                        addr.Country = item.Country;
                        addr.Num = item.Num;
                        addr.State = item.State;
                        addr.Street = item.Street;
                        addr.ZipCode = item.ZipCode;
                    }
                }

                foreach (var item in user.Phones)
                {
                    if(item.PhoneId == 0)
                    {
                        item.UserId = user.UserId;
                        await _context.Phones.AddAsync(item);
                    } 
                    else
                    {
                        var phon = userLocal.Phones.Find( x => x.PhoneId == item.PhoneId );
                        phon.Type = item.Type;
                        phon.Number = item.Number;
                    }
                }
                
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");
            }
        }


        public async Task RemoveUser(User user)
        {
            try
            {
                _context.Users.Remove(user);
                _context.Logins.Remove(user.Login);
                _context.Addresses.RemoveRange(user.Addresses);
                _context.Images.Remove(user.Image);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");
            }
        }

        public async Task DeleteAddressUser(Address addr)
        {
            var address = await _context.Addresses.Where( x => x.AddressId == addr.AddressId ).FirstOrDefaultAsync();
            _context.Addresses.Remove(address);
        }

        public async Task DeletePhoneUser(Phone phonObj)
        {
            var phone = await _context.Phones.Where( x => x.PhoneId == phonObj.PhoneId ).FirstOrDefaultAsync();
            _context.Phones.Remove(phone);
        }

        public async Task<User> Login(string email, string password)
        {
            IQueryable<User> query = _context.Users.Where(x => x.Login.Email.ToLower() == email.ToLower() && x.Login.Pass == password).Include(x => x.Login).Include(x => x.Addresses).Include(x => x.Phones).Include(x => x.Tickets).Include(x => x.Image).Include( x => x.Events);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Login> AlternPass(string email, string oldPass, string newPass)
        {
            try
            {
                Login login = await GetPass(email, oldPass);
                login.Pass = newPass;
                _context.Logins.Update(login);
                await _context.SaveChangesAsync();
                return login;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                throw;
            }
        }

        public async Task<Login> GetPass(string email, string pass)
        {
            IQueryable<Login> query = _context.Logins.Where(x => x.Email.ToLower() == email.ToLower() && x.Pass == pass);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Address>> GetAllAdresses()
        {
            return await _context.Addresses.ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByUser(int id)
        {
            IQueryable<Ticket> query = _context.Tickets.Where(x => x.UserId == id).Include(x => x.Event).Include(x => x.Event.Address);
            return await query.ToListAsync();
        }

        public async Task<Address> GetAddress(int id)
        {
            return await _context.Addresses.FirstOrDefaultAsync(x => x.AddressId == id);
        }

        public async Task UpdateSaldo(decimal valTotal, User userObj)
        {
            var userLocal = _context.Users    
                    .Where(x => x.UserId == userObj.UserId)                    
                    .FirstOrDefault();

                userLocal.Credit = (userLocal.Credit-valTotal);
        }
    }
}