using ChatApplication.Code;
using ChatApplication.Models.Chat;
using ChatApplication.Models.Customer_Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatApplication.Pages
{
    public class ChatModel : PageModel
    {
        private readonly ChatService chatService;
        private readonly CustomerService customerService;

        public ChatModel(CustomerService customerService, ChatService chatService)
        {
            this.customerService = customerService;
            this.chatService = chatService;
        }
        [BindProperty]
        public IQueryable<Customer> Customers { get; set; }   
        public void OnGet()
        {
            Customers = customerService.Get().Where(i=> !string.IsNullOrWhiteSpace(i.FirstName) || !string.IsNullOrWhiteSpace(i.LastName));
        }

        public IActionResult OnGetMessages(string from,string to)
        {
            var topic=GuidExtensions.GenerateUniqueGuid(from,to).ToString();
            return new JsonResult(chatService.GetMessages(topic));
        }

    }
}
