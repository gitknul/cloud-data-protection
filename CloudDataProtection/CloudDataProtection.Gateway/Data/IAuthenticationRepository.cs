﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CloudDataProtection.Entities;

namespace CloudDataProtection.Data
{
    public interface IAuthenticationRepository
    {
        Task<User> Get(long id);
        Task<User> Get(string email);
        Task<ICollection<User>> GetAll(UserRole role);
        Task Update(User user);

        Task Create(User user);
        Task Delete(User user);

        Task Create(ResetPasswordRequest request);
        Task<ResetPasswordRequest> GetResetPasswordRequest(string token);
        Task<IEnumerable<ResetPasswordRequest>> GetResetPasswordRequests(long userId);
        Task Update(ResetPasswordRequest request);
        Task Update(IEnumerable<ResetPasswordRequest> requests);

        Task Create(ChangeEmailRequest request);
        Task Update(ChangeEmailRequest request);
        Task Update(IEnumerable<ChangeEmailRequest> requests);

        Task<IEnumerable<ChangeEmailRequest>> GetAll(long userId);
        Task<IEnumerable<ChangeEmailRequest>> GetAll(string email);
        Task<ChangeEmailRequest> GetEmailRequest(string inputToken);
    }
}