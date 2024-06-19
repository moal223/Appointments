
using Appointement.Interfaces;
using Appointement.Models;
using NuGet.Protocol.Core.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Appointement.Services
{
    public class AppointmentReminderService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<AppointmentReminderService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);
        public AppointmentReminderService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, ILogger<AppointmentReminderService> logger)
        {
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAppointmentAsync(stoppingToken);
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
        private async Task CheckAppointmentAsync(CancellationToken stoppingToken)
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var appointmentRepository = scope.ServiceProvider.GetRequiredService<IAppointmentRepository>();

                var upCommingAppointments = (await appointmentRepository.GetAll())
                .Where(a => a.ExpiDate > DateTime.Now && a.ExpiDate <= DateTime.Now.AddMinutes(30) && !a.ReminderSent).ToList();

                foreach (var appointment in upCommingAppointments)
                {
                    //await SendSmsReminder(appointment);
                    _logger.LogInformation($"{appointment.Title}");
                    appointment.ReminderSent = true;
                    await appointmentRepository.Update(appointment);
                }
            }
            

            
        }
        private async Task SendSmsReminder(Appointment appointment)
        {
            var accountSid = _configuration["Twilio:AccountSid"];
            var authToken = _configuration["Twilio:AuthToken"];
            var fromNumber = _configuration["Twilio:FromNumber"];

            TwilioClient.Init(accountSid, authToken);

            var message = await MessageResource.CreateAsync(
            body: $"Reminder: You have an appointment at {appointment.ExpiDate}.",
            from: new PhoneNumber(fromNumber),
            to: new PhoneNumber(appointment.ApplicationUser.PhoneNumber)
        );
        }
    }
}
