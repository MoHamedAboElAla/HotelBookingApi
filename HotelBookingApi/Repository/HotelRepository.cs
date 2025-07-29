using HotelBookingApi.Data;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Repository
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AppDbContext _context;

        public HotelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Hotel>> GetAllAsync()
        {
            //return await _context.Hotels
            //    .Include(h => h.Rooms)
            //    .Include(h => h.Seasons)
            //    .Include(h => h.Bookings)
            //    .Include(h => h.Agents)
            //    .ToListAsync();
            return await _context.Hotels
                .Include(h => h.Rooms)
                //.Include(h => h.Seasons)

                .Include(h => h.Bookings)
                
                .ToListAsync();
        }

        public async Task<Hotel?> GetByIdAsync(int id)
        {
            //return await _context.Hotels
            //    .Include(h => h.Rooms)
            //    .Include(h => h.Seasons)
            //    .Include(h => h.Bookings)
            //    .Include(h => h.Agents)
            //    .FirstOrDefaultAsync(h => h.Id == id);

            return await _context.Hotels
                .Include(h => h.Rooms)

                .Include(h => h.Bookings)
                
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task AddAsync(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
        }

        public Task EditAsync(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            return Task.CompletedTask;
        }

        public async Task RemoveAsync(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel is not null)
            {
                _context.Hotels.Remove(hotel);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<(List<Hotel> Hotels, int TotalCount)> SearchAndPaginateAsync(
            string? searchTerm,
            int page,
            int pageSize,
            string sortBy,
            string sortDirection)
        {
            //var query = _context.Hotels
            //    .Include(h => h.Rooms)
            //    .Include(h => h.Seasons)
            //    .Include(h => h.Bookings)
            //    .Include(h => h.Agents)
            //    .AsQueryable();

            var query = _context.Hotels

                .Include(h => h.Rooms)        
                .Include(h => h.Bookings)
                
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(h =>
                    h.Name!.Contains(searchTerm) ||
                    h.Location!.Contains(searchTerm) ||
                    h.Country!.Contains(searchTerm));
            }

            // Total count
            var totalCount = await query.CountAsync();

            // Sorting
            query = sortBy.ToLower() switch
            {
                "name" => sortDirection == "desc" ? query.OrderByDescending(h => h.Name) : query.OrderBy(h => h.Name),
                "country" => sortDirection == "desc" ? query.OrderByDescending(h => h.Country) : query.OrderBy(h => h.Country),
                "stars" => sortDirection == "desc" ? query.OrderByDescending(h => h.Stars) : query.OrderBy(h => h.Stars),
                _ => query.OrderBy(h => h.Id)
            };

            // Pagination
            var hotels = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (hotels, totalCount);
        }
    }
}
