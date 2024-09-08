using ChatApplication.Models.Customer_Data;
using ChatApplication.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Code
{
    public class ChatHub : Hub
    {
        private readonly ProducerService producerService;
        private readonly CustomerService customerService;

        public ChatHub(ProducerService producerService,CustomerService customerService)
        {
            this.producerService = producerService;
            this.customerService = customerService;
        }

        public async Task SendMessage(string from,string to,string message)
        {
            var sender = customerService.GetCustomerById(from);
            var receiver = customerService.GetCustomerById(to);
            var topic = GuidExtensions.GenerateUniqueGuid(from, to).ToString();
            await Clients.All.SendAsync($"ReceiveMessage-"+topic,sender,receiver, message);
            await this.producerService.SendMessageAsync(from,to,message);
        }
    }
}
