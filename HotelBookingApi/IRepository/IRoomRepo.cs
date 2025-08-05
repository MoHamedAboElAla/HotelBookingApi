
using HotelBookingApi.Models;

namespace HotelBookingApi.IRepository
{
    public interface IRoomRepo
    {
        public List<Room> GetAll();
        public Room GetbyId(int id);
        public void Add(Room r);
        public void Update(Room r);
        public void Delete(Room r);
       
        public void Save();
       public  IEnumerable<Room> GetAvailableRoomsUsingDate(DateTime startDate, DateTime endDate, int hotelId);

        List<int> GetRoomNumbersForHotel(int hotelId);
        void AddRange(IEnumerable<Room> rooms);



    }
}
