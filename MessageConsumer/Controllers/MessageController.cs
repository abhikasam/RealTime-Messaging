using MessageConsumer.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessageConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ConsumerService consumerService;
        private readonly ILogger<MessageController> logger;

        public MessageController(ConsumerService consumerService,ILogger<MessageController> logger)
        {
            this.consumerService = consumerService;
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult ProcessMessages([FromBody] string message)
        {
            return Ok($"message received.");
        }
    }
}
