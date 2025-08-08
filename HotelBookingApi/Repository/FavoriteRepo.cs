using HotelBookingApi.Data;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Repository
{
    public class FavoriteRepo : IFavoriteRepo
    {
        private readonly AppDbContext _context;

        public FavoriteRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddToFavoritesAsync(int userId, int roomId)
        {
            var exists = await _context.Favorites
            .AnyAsync(f => f.UserId == userId && f.RoomId == roomId);
            if (exists)
            {
                throw new InvalidOperationException("This room is already in your favorites.");
            }
            var favorite = new Favorite
            {
                UserId = userId,
                RoomId = roomId
            };
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveFromFavoritesAsync(int userId, int roomId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.RoomId == roomId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<Room>> GetFavoritesAsync(int userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.Room!)
                .ToListAsync();
        }
        public async Task<bool> IsFavoriteAsync(int userId, int roomId)
        {
            return await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.RoomId == roomId);
        }

    }
}
