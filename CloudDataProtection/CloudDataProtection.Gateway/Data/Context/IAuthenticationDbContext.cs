using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Data.Context
{
    public interface IAuthenticationDbContext
    {
        DbSet<Entities.User> User { get; set; }
        DbSet<Entities.UserDeletionHistory> UserDeletionHistory { get; set; }
        DbSet<Entities.ChangeEmailRequest> ChangeEmailRequest { get; set; }
        DbSet<Entities.ResetPasswordRequest> RequestPasswordRequest { get; set; }

        Task<bool> SaveAsync();
    }
}