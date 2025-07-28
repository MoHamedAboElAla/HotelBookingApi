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

        public void save();




    }
}
