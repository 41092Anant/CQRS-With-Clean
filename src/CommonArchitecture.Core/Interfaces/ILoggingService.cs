using CommonArchitecture.Core.Entities;

namespace CommonArchitecture.Core.Interfaces;

public interface ILoggingService
{
 Task LogErrorAsync(ErrorLog error);
 Task LogRequestResponseAsync(RequestResponseLog log);
}
