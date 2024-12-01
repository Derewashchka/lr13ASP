using Serilog;
using lr13.Models;

namespace lr13.Services;

public class UserService
{
    private readonly ILogger<UserService> _logger;

    public UserService(ILogger<UserService> logger)
    {
        _logger = logger;
    }

    public User CreateUser(string name, string email)
    {
        try
        {
            _logger.LogInformation("Creating user {UserName} with email {UserEmail}", name, email);

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Attempt to create user with empty name");
                throw new ArgumentException("Name cannot be empty");
            }

            var user = new User(name, email);

            _logger.LogDebug("User created successfully: {@User}", user);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating user {UserName}", name);
            throw;
        }
    }

    public void SimulateError()
    {
        try
        {
            throw new InvalidOperationException("Simulated critical error");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "A critical error has occurred");
        }
    }
}