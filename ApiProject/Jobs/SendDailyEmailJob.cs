using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Quartz;

namespace ApiProject.Jobs
{
    public class SendDailyEmailJob : IJob
    {
        private readonly IFlowerRepository _flowerRepository;
        private readonly IEmailSender _emailSender;

        public SendDailyEmailJob(IFlowerRepository flowerRepository, IEmailSender emailSender)
        {
            _flowerRepository = flowerRepository;
            _emailSender = emailSender;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var flowers = await _flowerRepository.GetExpiringFlowers(DateTime.Now.AddDays(3));
            var randomFlower = flowers.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            if (randomFlower != null)
            {
                var users = await _flowerRepository.GetUsers(); 
                foreach (var user in users)
                {
                    await _emailSender.SendEmailAsync(user.Email, "Flower Expiry Alert", $"Discount will end in 3 days.");
                }
            }
        }
    }
}
