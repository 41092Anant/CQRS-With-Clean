using CommonArchitecture.Core.Entities;

namespace CommonArchitecture.Core.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<IEnumerable<User>> GetPagedAsync(string? searchTerm, string sortBy, string sortOrder, int pageNumber, int pageSize);
    Task<int> GetTotalCountAsync(string? searchTerm);
}

