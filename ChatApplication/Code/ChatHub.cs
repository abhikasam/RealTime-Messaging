using ChatApplication.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Code
{
    public class ChatHub : Hub
    {
        private readonly ProducerService producerService;

        public ChatHub(ProducerService producerService)
        {
            this.producerService = producerService;
        }

        public async Task SendMessage(string from,string to,string message)
        {
            string topic = GuidExtensions.GenerateUniqueGuid(from, to).ToString();
            await Clients.All.SendAsync("ReceiveMessage",from,to, message);
            await this.producerService.SendMessageAsync(from,to,message);
        }
    }
}
