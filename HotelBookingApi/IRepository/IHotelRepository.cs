using HotelBookingApi.Models;

namespace HotelBookingApi.IRepository
{
    public interface IHotelRepository
    {
        public List<Hotel> GetAll();
        public Hotel GetById(int id);
        public void add(Hotel hotel);
        public void edit(Hotel hotel);
        public void remove(int id);
        public void save();
        //List<Hotel> SearchAndPaginate(string? searchTerm, int page, int pageSize, out int totalCount);
        List<Hotel> SearchAndPaginate(
         string? searchTerm,
         int page,
         int pageSize,
         string sortBy,
         string sortDirection,
         out int totalCount);


    }
}
