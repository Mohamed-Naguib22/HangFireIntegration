using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HangFireIntegration.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class EmailSenderController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        public EmailSenderController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail() 
        {
            RecurringJob.AddOrUpdate(() => _emailSender.SendEmailAsync("mohamednageb20172@gmail.com", "Test", "HangFire Worked"), Cron.Minutely(), TimeZoneInfo.Utc);
            return Ok();
        }
    }
}
