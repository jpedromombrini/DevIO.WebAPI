using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevIO.API.Data
{
    public class AppllicationDbContext : IdentityDbContext
    {
        public AppllicationDbContext(DbContextOptions<AppllicationDbContext> options) : base(options)
        {
            
        }
        
    }
}