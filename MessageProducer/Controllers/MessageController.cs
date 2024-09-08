using Confluent.Kafka;
using MessageProducer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MessageProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ProducerService producerService;
        private readonly IConfiguration configuration;

        public MessageController(ProducerService producerService,IConfiguration configuration)
        {
            this.producerService = producerService;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            try
            {
                await producerService.ProduceAsync("messages",message);
                return Ok(new { Message = $"Message sent successfully : {message}" });
            }
            catch (ProduceException<Null, string> e)
            {
                return StatusCode(500, $"Error producing message: {e.Error.Reason}");
            }
        }

    }
}
