using CleanArchitecture.Application.Models;

namespace CleanArchitecture.Application.Contracts.Infrastucture
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
