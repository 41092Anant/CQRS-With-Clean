using CommonArchitecture.Core.Entities;

namespace CommonArchitecture.Core.Interfaces;

public interface ILoggingService
{
    Task LogErrorAsync(ErrorLog error);
    Task LogRequestResponseAsync(RequestResponseLog log);
    
    // Read methods for Admin
    Task<(IEnumerable<RequestResponseLog> Logs, int TotalCount)> GetLogsAsync(
        int pageNumber, 
        int pageSize, 
        string? searchTerm = null, 
        string? sortBy = null, 
        string sortOrder = "desc",
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? statusCode = null,
        string? method = null);
        
    Task<RequestResponseLog?> GetLogByIdAsync(int id);
}
