using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System.IO;
namespace SchoolDance.Chat.Infrastructure
{
    public class ChatDbContextFactory : IDesignTimeDbContextFactory<ChatDbContext>
    {
        public ChatDbContext CreateDbContext(string[] args)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables() // Это позволит подхватывать данные из системы
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ChatDbContext>();
            
            // Берем строку подключения из конфига
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);

            return new ChatDbContext(optionsBuilder.Options);
        }
    }
}