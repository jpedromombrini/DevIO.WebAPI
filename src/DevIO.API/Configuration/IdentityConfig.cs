using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DevIO.API.Data;


namespace DevIO.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppllicationDbContext>(options =>            
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppllicationDbContext>()
                    .AddDefaultTokenProviders();

            return services;
        }
    }
}