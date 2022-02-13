using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Data.Context;
using CloudDataProtection.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Data
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IAuthenticationDbContext _context;
        
        public AuthenticationRepository(IAuthenticationDbContext context)
        {
            _context = context;
        }
        
        public async Task<User> Get(long id)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> Get(string email)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ICollection<User>> GetAll(UserRole role)
        {
            return await _context.User
                .Where(u => u.Role == role)
                .ToArrayAsync();
        }

        public async Task Update(User user)
        {
            _context.User.Update(user);

            await _context.SaveAsync();
        }

        public async Task Create(User user)
        {
            _context.User.Add(user);

            await _context.SaveAsync();
        }

        public async Task Delete(User user)
        {
            _context.User.Remove(user);

            await _context.SaveAsync();
        }

        public async Task Create(ResetPasswordRequest request)
        {
            _context.RequestPasswordRequest.Add(request);

            await _context.SaveAsync();
        }

        public async Task<ResetPasswordRequest> GetResetPasswordRequest(string token)
        {
            return await _context.RequestPasswordRequest
                .FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task<IEnumerable<ResetPasswordRequest>> GetResetPasswordRequests(long userId)
        {
            return await _context.RequestPasswordRequest
                .AsNoTracking()
                .Where(r => r.UserId == userId)
                .ToArrayAsync();
        }

        public async Task Update(ResetPasswordRequest request)
        {
            _context.RequestPasswordRequest.Update(request);

            await _context.SaveAsync();
        }

        public async Task Update(IEnumerable<ResetPasswordRequest> requests)
        {
            _context.RequestPasswordRequest.UpdateRange(requests);
            
            await _context.SaveAsync();
        }

        public async Task Create(ChangeEmailRequest request)
        {
            _context.ChangeEmailRequest.Add(request);
            
            await _context.SaveAsync();
        }

        public async Task Update(ChangeEmailRequest request)
        {
            _context.ChangeEmailRequest.Update(request);
            
            await _context.SaveAsync();
        }

        public async Task Update(IEnumerable<ChangeEmailRequest> requests)
        {
            _context.ChangeEmailRequest.UpdateRange(requests);
            
            await _context.SaveAsync();
        }

        public async Task<IEnumerable<ChangeEmailRequest>> GetAll(long userId)
        {
            return await _context.ChangeEmailRequest
                .AsNoTracking()
                .Where(r => r.UserId == userId)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<ChangeEmailRequest>> GetAll(string email)
        {
            return await _context.ChangeEmailRequest
                .AsNoTracking()
                .Where(r => r.NewEmail == email)
                .ToArrayAsync();
        }

        public async Task<ChangeEmailRequest> GetEmailRequest(string token)
        {
            return await _context.ChangeEmailRequest
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Token == token);
        }
    }
}