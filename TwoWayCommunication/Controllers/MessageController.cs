using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoWayCommunication.Services;

namespace TwoWayCommunication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ProducerService _producerService;
        private readonly ConsumerManager _consumerManager;

        public MessagesController(ProducerService producerService, ConsumerManager consumerManager)
        {
            _producerService = producerService;
            _consumerManager = consumerManager;
        }

        [HttpPost("{user1}/{user2}")]
        public async Task<IActionResult> Post(string user1, string user2, [FromBody] string message)
        {
            await _producerService.SendMessageAsync(user1,user2, message);
            _consumerManager.CreateConsumer(user1,user2);
            return Ok();
        }

        [HttpGet("{user1}/{user2}")]
        public async Task<IActionResult> Get(string user1,string user2)
        {
            return Ok(_consumerManager.GetUserMessages(user1,user2));
        }

        [HttpDelete("{user1}/{user2}")]
        public IActionResult Delete(string user1, string user2)
        {
            _consumerManager.StopConsumer(user1,user2);
            return Ok();
        }
    }
}
