using HotelBookingApi.Models;

namespace HotelBookingApi.IRepository
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAllAsync();
        Task<Hotel?> GetByIdAsync(int id);
        Task AddAsync(Hotel hotel);
        Task EditAsync(Hotel hotel);
        Task RemoveAsync(int id);
        Task SaveAsync();

        Task<(List<Hotel> Hotels, int TotalCount)> SearchAndPaginateAsync(
            string? searchTerm,
            int page,
            int pageSize,
            string sortBy,
            string sortDirection);

        Task<IEnumerable<Room>> GetRoomsByHotelAsync(int hotelId);
    }
}
