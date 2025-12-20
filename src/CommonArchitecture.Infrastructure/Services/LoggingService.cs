using CommonArchitecture.Core.Entities;
using CommonArchitecture.Core.Interfaces;
using CommonArchitecture.Infrastructure.Persistence;

namespace CommonArchitecture.Infrastructure.Services;

public class LoggingService : ILoggingService
{
 private readonly ApplicationDbContext _db;

 public LoggingService(ApplicationDbContext db)
 {
 _db = db;
 }

 public async Task LogErrorAsync(ErrorLog error)
 {
 _db.ErrorLogs.Add(error);
 await _db.SaveChangesAsync();
 }

 public async Task LogRequestResponseAsync(RequestResponseLog log)
 {
 _db.RequestResponseLogs.Add(log);
 await _db.SaveChangesAsync();
 }
}
