using ChatApplication.Code;
using ChatApplication.Models.Chat;
using ChatApplication.Models.Customer_Data;
using ChatApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ChatApplication.Pages
{
    public class ChatModel : PageModel
    {
        private readonly ChatService chatService;
        private readonly CustomerService customerService;
        private readonly CustomerCache customerCache;

        public ChatModel(CustomerService customerService, ChatService chatService,CustomerCache customerCache)
        {
            this.customerService = customerService;
            this.chatService = chatService;
            this.customerCache = customerCache;
        }
        [BindProperty]
        public IQueryable<Customer> Customers { get; set; }   
        public void OnGet()
        {
            Customers = customerCache.GetCustomers()
                    .Where(i => !string.IsNullOrWhiteSpace(i.FirstName) || !string.IsNullOrWhiteSpace(i.LastName));
        }

        public IActionResult OnGetMessages(string from,string to)
        {
            var topic=GuidExtensions.GenerateUniqueGuid(from,to).ToString();
            var messages = chatService.GetMessages(topic);
            foreach (var item in messages)
            {
                item.FromCustomer = customerService.GetCustomerById(item.From);
                item.ToCustomer = customerService.GetCustomerById(item.To);
            }
            return new JsonResult(messages);
        }

        public IActionResult OnGetTopic(string from,string to)
        {
            return new JsonResult(GuidExtensions.GenerateUniqueGuid(from, to).ToString());
        }

    }
}
