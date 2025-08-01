using HotelBookingApi.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Services
{
    public class RoomAvailabilityService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
      

        public RoomAvailabilityService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
           
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var now = DateTime.Now;

                var expiredBookings = await context.Bookings
                    .Where(b => b.CheckOutDate <= now)
                    .ToListAsync();

                var roomIds = expiredBookings.Select(b => b.RoomId).Distinct();

                var rooms = await context.Rooms
                    .Where(r => roomIds.Contains(r.Id) && !r.IsAvailable)
                    .ToListAsync();

                foreach (var room in rooms)
                {
                    room.IsAvailable = true;
                }

                await context.SaveChangesAsync();

              
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
