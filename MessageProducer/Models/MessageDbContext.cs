using Microsoft.EntityFrameworkCore;

namespace MessageProducer.Models
{
    public class MessageDbContext : DbContext
    {
        public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options) { }
        public DbSet<string> Messages { get; set; }
    }
}
