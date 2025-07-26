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

    }
}
