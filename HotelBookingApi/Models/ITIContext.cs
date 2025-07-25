using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingApi.Models
{
    public class ITIContext : IdentityDbContext<ApplicationUser>
    {
        public ITIContext(DbContextOptions<ITIContext> options) : base(options)
        {
        }

    }
}
