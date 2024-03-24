namespace SvelteApp1.Server.Services
{
    public interface IEmailConfirmationService
    {
        Task SendConfirmationEmailAsync(string toEmail, string confirmationLink);
    }
}
