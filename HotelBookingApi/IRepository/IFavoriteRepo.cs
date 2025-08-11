using HotelBookingApi.Models;

namespace HotelBookingApi.IRepository
{
    public interface IFavoriteRepo
    {
        Task AddToFavoritesAsync(int userId, int roomId);
        Task RemoveFromFavoritesAsync(int userId, int roomId);
        Task<IEnumerable<Room>> GetFavoritesAsync(int userId);
        Task<bool> IsFavoriteAsync(int userId, int roomId);
    }
}
